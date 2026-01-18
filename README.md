
# URP Glitch Effect ported to Unity 6.3 LTS URP 17+
**URP Glitch** is a glitch effect for the Universal Render Pipeline (URP), originally based on [mao-test-h's project](https://github.com/mao-test-h/URPGlitch), which in turn was inspired by [keijiro's HDRP Glitch effect](https://github.com/keijiro/KinoGlitch). 
---

## Setup Instructions

1. **Add Glitch Render Feature**:
   - Locate your URP renderer asset in Unity.
   - Click **Add Render Feature**.
   - Choose the **Analog/Digital Glitch Feature**.
   - Set the **Shader** field by selecting an appropriate shader (click the eye icon to reveal hidden shaders).
   - In the **Render Pass** dropdown, choose **After Rendering Transparents**.

2. **Configure Global Volume**:
   - In the Scene Hierarchy, create a volume: **Right-click → Volume → Global Volume**.
   - Select the volume and create a **New Profile**.
   - Add an override and select the **Analog/Glitch Volume**.
   - Ensure **Post-Processing** is enabled on the main camera to apply the effect.

---
