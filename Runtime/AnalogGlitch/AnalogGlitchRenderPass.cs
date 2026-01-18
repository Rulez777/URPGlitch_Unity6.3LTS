using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace URPGlitch.Runtime.AnalogGlitch
{
    /// <summary>
    /// RenderGraph-based analog glitch pass compatible with Unity 6.3 LTS (URP 17).
    /// Draws a fullscreen triangle to apply the effect without legacy Blit calls.
    /// </summary>
    sealed class AnalogGlitchRenderPass : ScriptableRenderPass, IDisposable
    {
        const string RenderPassName = "AnalogGlitch RenderPass";

        static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        static readonly int ScanLineJitterID = Shader.PropertyToID("_ScanLineJitter");
        static readonly int VerticalJumpID = Shader.PropertyToID("_VerticalJump");
        static readonly int HorizontalShakeID = Shader.PropertyToID("_HorizontalShake");
        static readonly int ColorDriftID = Shader.PropertyToID("_ColorDrift");

        readonly ProfilingSampler _profilingSampler = new(RenderPassName);
        readonly Material _material;

        AnalogGlitchVolume _volume;
        float _verticalJumpTime;

        static Mesh s_FullscreenTriangle;
        static Mesh FullscreenTriangle
        {
            get
            {
                if (s_FullscreenTriangle != null) return s_FullscreenTriangle;

                s_FullscreenTriangle = new Mesh { name = "Fullscreen Triangle" };
                s_FullscreenTriangle.SetVertices(new System.Collections.Generic.List<Vector3>
                {
                    new(-1f, -1f, 0f),
                    new(-1f, 3f, 0f),
                    new(3f, -1f, 0f)
                });
                s_FullscreenTriangle.SetIndices(new[] { 0, 1, 2 }, MeshTopology.Triangles, 0, false);
                s_FullscreenTriangle.UploadMeshData(false);

                return s_FullscreenTriangle;
            }
        }

        public AnalogGlitchRenderPass(Shader shader)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            _material = CoreUtils.CreateEngineMaterial(shader);

            var vm = VolumeManager.instance;
            if (vm != null && vm.stack != null)
            {
                _volume = vm.stack.GetComponent<AnalogGlitchVolume>();
            }
        }

        public void Dispose()
        {
            CoreUtils.Destroy(_material);

            if (s_FullscreenTriangle != null)
            {
                CoreUtils.Destroy(s_FullscreenTriangle);
                s_FullscreenTriangle = null;
            }
        }

        bool TryResolveVolume()
        {
            if (_volume != null) return true;

            var vm = VolumeManager.instance;
            if (vm == null || vm.stack == null) return false;

            _volume = vm.stack.GetComponent<AnalogGlitchVolume>();
            return _volume != null;
        }

        bool IsActive(UniversalCameraData cameraData)
        {
            if (_material == null) return false;
            if (cameraData.isSceneViewCamera || !cameraData.postProcessEnabled) return false;
            if (!TryResolveVolume()) return false;

            return _volume.IsActive;
        }

        sealed class PassData
        {
            internal TextureHandle source;
            internal TextureHandle destination;
            internal Material material;
            internal Vector2 scanLineJitter;
            internal Vector2 verticalJump;
            internal float horizontalShake;
            internal Vector2 colorDrift;
        }

        static void ExecutePass(PassData data, RasterGraphContext context)
        {
            data.material.SetVector(ScanLineJitterID, data.scanLineJitter);
            data.material.SetVector(VerticalJumpID, data.verticalJump);
            data.material.SetFloat(HorizontalShakeID, data.horizontalShake);
            data.material.SetVector(ColorDriftID, data.colorDrift);

            context.cmd.SetGlobalTexture(MainTexID, data.source);
            context.cmd.DrawMesh(FullscreenTriangle, Matrix4x4.identity, data.material, 0, 0);
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var cameraData = frameData.Get<UniversalCameraData>();
            if (!IsActive(cameraData))
                return;

            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogWarning($"{RenderPassName} requires an intermediate color texture. Enable \"Require Intermediate Texture\" on the renderer.");
                return;
            }

            var source = resourceData.activeColorTexture;
            var sourceDesc = renderGraph.GetTextureDesc(source);
            sourceDesc.clearBuffer = false;

            var destinationDesc = sourceDesc;
            destinationDesc.name = $"CameraColor-{RenderPassName}";
            var destination = renderGraph.CreateTexture(destinationDesc);

            var scanLineJitter = _volume.scanLineJitter.value;
            var verticalJump = _volume.verticalJump.value;
            var horizontalShake = _volume.horizontalShake.value;
            var colorDrift = _volume.colorDrift.value;

            _verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

            var slThresh = Mathf.Clamp01(1f - scanLineJitter * 1.2f);
            var slDisp = 0.002f + Mathf.Pow(scanLineJitter, 3f) * 0.05f;

            using (var builder = renderGraph.AddRasterRenderPass(RenderPassName, out PassData data, _profilingSampler))
            {
                data.source = source;
                data.destination = destination;
                data.material = _material;
                data.scanLineJitter = new Vector2(slDisp, slThresh);
                data.verticalJump = new Vector2(verticalJump, _verticalJumpTime);
                data.horizontalShake = horizontalShake * 0.2f;
                data.colorDrift = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);

                builder.UseTexture(source, AccessFlags.Read);
                builder.SetRenderAttachment(destination, 0);
                builder.AllowGlobalStateModification(true);
                builder.SetRenderFunc((PassData passData, RasterGraphContext ctx) => ExecutePass(passData, ctx));
            }

            resourceData.cameraColor = destination;
        }
    }
}
