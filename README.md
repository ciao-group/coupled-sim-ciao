# Mixed Reality Driving Simulation with Varjo XR-3

## Introduction

This project is a modified version of a Coupled simulator for research on driver-pedestrian interactions made in Unity originally developed by Dr. Pavlo Bazilinskyy at TU Eindhoven. 

Our version builds on the original by combining a a real-world car model with the simulated driving environment into a Mixed-Reality solution with the Varjo XR-3 HMD.
This adaptation provides a platform for in-depth research into driver-pedestrian interactions and user interface evaluation within a simulated driving context.

We are the Junior Research Group CIAO (Computational Interaction and Mobility) and work at the intersection of machine learning, human-computer interaction, and mobility.
We are part of the Center for Scalable Data Analytics and Artificial Intelligence (ScaDS.AI) at Leipzig University. ScaDS.AI Dresden/Leipzig is one of five new AI centers in Germany funded under the federal government’s AI strategy.

This project is open-source and aims to foster a community of researchers and developers interested in advancing mixed reality applications for mobility and interaction studies. 
We encourage contributions, collaborations, and discussions to enhance the simulator's capabilities and applicability in various research domains.

## Project Origin

The original simulation was designed for academic research, offering insights into the interaction between pedestrians and (automated) vehicles. We have forked Dr. Bazilinskyy's project and tailored it to meet our specific research needs, adding new features and expanding its use cases.


# Tutorial on Setup and Documentation

## Integration of Varjo XR-3 for Mixed Reality Driving Simulation

We provide step-by-step instructions for setting up and developing a mixed reality driving simulation using the Varjo XR-3 headset. The Varjo XR-3 offers cutting-edge mixed reality capabilities, combining high-resolution visuals and real-world pass-through to create immersive experiences. Our model leverages these features to simulate driving in a virtual environment while sitting in a real car model-

## Requirements

**Hardware**:
  - A PC with Windows 10 or Windows 11,
  - GPU with two ports directly connected to it (e.g. 2 HDMI ports leading directly to the GPU). Some Laptops&PCs don't have this feauture. Check Varjo XR-3 Requirements.

**Software**:
  - Varjo Base
  - Varjo Lab Tools
  - Steam and SteamVR
  - Unity Hub
  - Varjo SDK for Unity (pre-installed in the project) 

## Initial Setup 

>*Taken from the [original project](https://github.com/bazilinskyy/coupled-sim)*
>
>### Input
>The coupled simulator supports a keyboard and a gaming steering wheel as input sources for the driver of the manual car, a keyboard for the passenger of the AV to control the external human->machine interface, and a motion suit or a keyboard for the pedestrian. At the moment, supported motion suit is Xsens Motion Suit.
>
>### Output
>The simulator supports giving output to both a computer screen and a head-mounted display (HMD). It has been tested with Oculus Rift CV1, Varjo VR-2 Pro.
>
>
>
>
>
>### Installation
>The simulator was tested on Windows 11 and macOS Ventura 13.5. All functionality is supported by both platforms. However, support for input and output devices was tested only on Windows 10.
>
>After checking out this project, launch Unity Hub to run the simulator with the correct version of Unity (currently **2022.3.5f1**).
>
>## Running a project
> When opening this project for the first time, click `Add` and select your entire downloaded folder containing all the project files. In case you have already used this project before, select >the project from the Unity Hub projects list. Wait until the project loads in.
>
>Once the project is loaded into the Unity editor open StartScene scene. You do this by going to the project tab, and clicking on the StartScene icon in the Scenes folder. Now, we will explain >three different ways to run scenes, the locations for the first three steps for running as a host or client correlate with the numbers in the picture below. 
>
>![](ReadmeFiles/running.png)
>
>### Running simulation as a host
>1. Make sure that all three checkboxes (`Hide GUI`, `Run Trial Sequence Automatically`, `Record Videos`) in `NetworkingManager` component on `Managers` game object are unchecked.
>2. Press the Play button to run enter `Play Mode`.
>3. Once in `Play Mode`, press `Start Host` button. 
>4. If needed, wait for clients to join.
>5. Once all clients have connected to the host or in case the host is handling the only participant, select one of the experiments listed under `Experiment` section.
>6. Assign roles to participants in `Role binding` section. If no role is selected, the participant will run a "headless" mode.
>7. Select one of control modes listed under `Mode` section.
>8. Start an experiment with the `Initialise experiment` button - all clients will load selected experiment.
>9. Once all connected clients are ready - the experiment scene is fully loaded on each client, press `Start simulation` button.
>
>### Running simulation as a client
>1. Make sure that all three checkboxes - `Hide GUI`, `Run trial sequence automatically`, `Record videos` (`Managers` (game object) ➡️ `NetworkingManager` (component)), are unchecked.
>2. Press the Play button to run enter `Play mode`.
>3. Once in `Play Mode`, press `Start client` button.
>4. Enter the host IP address. 
>5. Press `Connect`.
>6. Once connected, select one of control modes listed under `Mode` section.
>7. Wait until host starts the simulation.
>   
>
>### Running simulation with multiple agents on one machine
>1. Run host agent inside of Unity (as described above).
>2. Note the IP address in the console of Unity.
>3. Run other  agents after building them `File` ➡️ `Build And Run`. Connect using the IP address from the (1).
>
>Providing input for multiple agents on one machine is not supported. Input is provided only to the agent, the window of which is selected. This mode is intended for testing/debugging.

### Varjo Software Installation
1. Install Varjo Base from [Varjo Website](https://developer.varjo.com/downloads#unity-developer-assets).  
2. Download Varjo Lab Tools from Varjo Website.

These applications are essential for managing the headset and configuring the mixed reality settings. 


### Steam and SteamVR

1. Install Steam and SteamVR.
   SteamVR is necessary for utilizing the base stations, which enable motion tracking. Follow the internal instructions for installation and setup.

### Further tracking solutions  
Motion and Hand Tracking can also be done with the Inside-Out-Tracking feauture of Varjo XR-3 (which is still in Beta). 
For this go in Varjo Base to `System`, and enable `Inside-Out-Tracking with Varjo (Beta)`.


### Hardware Connections

1. Connect the Varjo XR-3 headset to your PC, ensuring you use the correct HDMI ports or adapters specified for your hardware configuration.

### Varjo and Unity integration
More info is to be found on the [Varjo developer page](https://developer.varjo.com/docs/get-started/get-started).
Make sure to check all Menu Elements, as navigation on Varjo page isn't very clear.

## Unity Project Configuration

### Varjo SDK installation

### Varjo Plugin Integration

1. In your Unity project, navigate to `Project Settings > XR Plug-in Management` and select Varjo as the Plug-in Provider. This step is crucial for enabling Varjo-specific features within Unity.

### Camera Setup for Mixed Reality

1. Locate the `DrivableCommonObject` in your Unity scene. This object will serve as the basis for integrating the real car model with the virtual driving environment.
2. Manually insert an `XR Origin` component into the `Drivers Logic`. Due to the presence of multiple main cameras in the scene, an automated conversion is not feasible.
3. Under the component responsible for aligning the camera with the car (name to be specified later), add the XR Origin. Then, make the existing Main Camera a child of the XR Origin. This setup allows the camera to move in sync with the car.

### Motion Tracking Configuration

1. For head motion tracking, select a component (name to be specified later) and configure it to track a Generic XR Device. Initially, set it to track only the rotation. Position tracking may need to be added later, especially once the real car model is integrated, to ensure the virtual and real-world align accurately.
2. Adjustments to the camera's position relative to the floor may be necessary to match the real-world setup.

### Scripting for Camera Management

1. Implement a script to disable the second main camera, which is typically used for rendering the game menu. This ensures that the simulation uses the XR camera exclusively for the MR experience.

### Testing the Setup

1. With the above configuration, the simulator should now be playable in full VR mode. Test the setup to ensure that the virtual environment is correctly rendered through the Varjo XR-3 headset and that motion tracking functions as expected.

## Next Steps for Mixed Reality Implementation

In the following section, detailed instructions will be provided on achieving mixed reality with the Varjo XR-3, including configuring the real-world pass-through and integrating the physical car model into the simulation.














# Mixed Reality Implementation Using Varjo XR-3 and HDRP in Unity

## Overview

This guide explains the methodology behind integrating the Varjo XR-3 headset with Unity for mixed reality (MR) applications, specifically leveraging the High Definition Render Pipeline (HDRP) to render real-world video camera feeds onto Unity materials. This allows for the seamless blending of real and virtual worlds, enabling mixed reality experiences.

## Initial Steps

### Setting Up Varjo XR Plugin for Unity

1. **Install Varjo XR Plugin**: Follow the instructions in the "Getting Started with Varjo XR plugin for Unity" documentation.
2. **Configure Varjo XR Plugin Settings**: In Unity, go to `Project Settings > XR Plug-in Management`, select Varjo, and disable the Opaque option. You can automate this with a script using runtime functions provided by Varjo.

### Enabling Video Pass-Through

- **Start/Stop Rendering**: Use `VarjoMixedReality.StartRender()` to begin and `VarjoMixedReality.StopRender()` to cease rendering the video see-through image from the cameras.

### Setting the Camera

- To render the image from the video pass-through cameras, ensure your application renders 0 in the color buffer. This can be achieved using a stencil mask or setting the camera's Clear Flags to Solid Color with RGBA(0,0,0,0).

## Using HDRP for Mixed Reality

1. **Configure HDRP Settings**: Ensure you're using a color buffer format with an alpha channel. In `Project Settings > Quality`, under HDRP settings, select your HD Render Pipeline Asset and set the Color Buffer Format to R16G16B16A16.

2. **Camera Background**: Set the camera’s Background Type to Color with RGBA(0,0,0,0).

## Creating the MR Material

1. **Material Setup**: Varjo Compositor blends images based on the alpha value. Create a new material with a shader that supports alpha export and depth writing. Use HDRP’s Unlit shader, set Surface Type to Opaque, and Color to RGBA(0,0,0,0). Enable Alpha Clipping if necessary.

2. **Assign to Mesh**: Apply this material to a mesh that will act as a VST (video see-through) mask, such as a Quad in a doorway.

## Project Adaptations for MR

### Switching to HDRP

- **Update Project for HDRP**: Follow the HDRP wizard to resolve compatibility issues. Address any materials that turn pink by updating their shaders.

### Material Conversion to HDRP

- **Converting Materials**: Select faulty materials, change their shader to HDRP’s standard shader. Use `Window > Rendering > HDRP Wizard > Convert Selected Built-In Materials to HDRP` for conversion.

### Model and Scene Adjustments

- **3D Model Preparation**: Simplify the real-life car model in Blender by reducing vertex count and removing unnecessary parts. Integrate this model with your Unity project, ensuring the front panel is set up for MR with the camera feed.

- **Lighting Adjustments in HDRP**: Reduce the maximum allowed reflections to address lighting issues.

## Final Steps

1. **Re-import the Car Model**: After making adjustments, import the simplified car model into Unity.
2. **Assign Textures and Materials**: Apply the correct textures and materials to the car model. Use the MR material for the front panel to enable real-world video pass-through.



## Acknowledgments

This project is part of ScaDS.AI Dresden/Leipzig, supported under the federal government's AI strategy. 
originally developed by Dr. Pavlo Bazilinskyy at TU Eindhoven. 

## Citation

If you utilize this modified simulator for academic purposes, please cite the original work:

> Bazilinskyy, P., Kooijman, L., Dodou, D., & De Winter, J. C. F. (2020). Coupled simulator for research on the interaction between pedestrians and (automated) vehicles. 19th Driving Simulation Conference (DSC). Antibes, France. 


