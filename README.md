> **Note:** If you are coming from the older URP Glitch video, you need to download the older `2021.3.8f1+` package from the **Releases** section.

# URP Glitch Effect
**URP Glitch** is a glitch effect for the Universal Render Pipeline (URP), originally based on [mao-test-h's project](https://github.com/mao-test-h/URPGlitch), which in turn was inspired by [keijiro's HDRP Glitch effect](https://github.com/keijiro/KinoGlitch). 
 Install from Local Package
1. Download the package from the **Releases** section of the GitHub page.
2. In Unity, right-click and select **Import Package → Custom Package**.
3. Choose the downloaded package file. Beginners may want to import all files.

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

## Sample Scene Setup

1. Download and open the **Sample Scene**.
2. Find the URP asset file in the project.
3. In the **Renderer List**, click the **+** icon.
4. Assign the **2D Sample Renderer** or **Sample Universal Renderer**.
5. The effect should now be active. 

> The settings object in the sample scene includes a script that demonstrates how to use buttons to control the effects programmatically.

---

## License

This project is released under the same license as [keijiro/KinoGlitch](https://github.com/keijiro/KinoGlitch).
