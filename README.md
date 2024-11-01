# URPGlitch
urp glitch originally based on mao-test-h's project (which is a port of keijiro's hdrp glitch effect)

# steps for usage
1.A: install using the package manager
- open package manager by clicking on window -> package manager.
- click on the plus icon on the top left of the package manager window.
- select "install package from git URL..."
- in the text field that appears paste this URL: https://github.com/saimarei/URPGlitch.git
- click install
      
1.B: install using modifying the "manifest.json" file
- open your project's folder location in your file explorer (if you are inside unity you can right click and select "show in explorer")
- open the packages folder and open the "manifest.json" file
- Add the following line to the `"dependencies"` section:
    ```json
    "com.subbu.urp-glitch": "https://github.com/saimarei/URPGlitch.git",
    ```
4. You can place it:
   - **After the opening brace** (`{`) of `"dependencies"`, as shown below:
     ```json
     {
       "dependencies": {
         "com.subbu.urp-glitch": "https://github.com/saimarei/URPGlitch.git",
         "com.unity.collab-proxy": "2.5.2",
         ...
       }
     }
     ```
   - **After any of the other dependencies**, with a comma following the previous entry:
     ```json
     {
       "dependencies": {
         "com.unity.collab-proxy": "2.5.2",
         "com.subbu.urp-glitch": "https://github.com/saimarei/URPGlitch.git",
         ...
       }
     }
     ```
   - **At the end of the dependencies list** (without a comma after it):
     ```json
     {
       "dependencies": {
         ...
         "com.unity.modules.xr": "1.0.0",
         "com.subbu.urp-glitch": "https://github.com/saimarei/URPGlitch.git"
       }
     }
     ```
  
1.C: install a local package
- first download your relevant package from the releases section of the GitHub page.
- in unity right click and select import package -> custom package.
- select the files you deem necessary, if you are a beginner I recommend getting all the files.

2. setup
- find your urp renderer asset in unity
- click on add render feature
- select analog/digital glitch feature
- click on circle next to the shader field and choose the analog/digital shader. (if working with digital glitch effect choose the appropriate shader for normal and compact fields)
- click on the render pass dropdown and select after rendering transparents. (for some reason selecting after or before post process in render graph mode culls the pass making it not work)
- in your scene hierarchy (where main camera, other game objects reside) create a volume by right-clicking and selecting volume -> global volume.
- select the newly created volume and click on new profile.
- after creating a new profile click on add override and select the analog/glitch volume.
- now turn on post-processing in your main camera and everything should work.

3. Sample Scene Setup
- after downloading the sample scene, find the urp asset file
- in the Renderer List click on the plus icon
- click on the circle next to the empty field and select the sample renderer.
- things should work now.
- the settings object in the scene has the script attached for the two buttons which show you how to use a script to control the effects.

# License
- [keijiro/KinoGlitch](https://github.com/keijiro/KinoGlitch)
