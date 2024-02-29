# Mixed Reality Driving Simulation with Varjo XR-3

## Introduction

This project is a modified version of a Mixed Reality Driving Simulation originally developed by © 2024 Dr. Pavlo Bazilinskyy at TU Eindhoven. Our adaptation of this innovative simulation is conducted by the Junior Research Group CIAO (Computational Interaction and Mobility) at the Center for Scalable Data Analytics and Artificial Intelligence (ScaDS.AI), Leipzig University. Our work intersects machine learning, human-computer interaction, and mobility, focusing on simulating immersive driving experiences using the Varjo XR-3 headset. This adaptation allows for a unique integration of high-resolution virtual environments with real-world elements, providing a platform for in-depth research into driver-pedestrian interactions and user interface evaluation within a simulated driving context.

This project is open-source and aims to foster a community of researchers and developers interested in advancing mixed reality applications for mobility and interaction studies. We encourage contributions, collaborations, and discussions to enhance the simulator's capabilities and applicability in various research domains.

## Project Origin

The original simulation was designed for academic research, offering insights into the interaction between pedestrians and (automated) vehicles. We have forked Dr. Bazilinskyy's project and tailored it to meet our specific research needs, adding new features and expanding its use cases.

## Citation

If you utilize this modified simulator for academic purposes, please cite the original work:

> Bazilinskyy, P., Kooijman, L., Dodou, D., & De Winter, J. C. F. (2020). Coupled simulator for research on the interaction between pedestrians and (automated) vehicles. 19th Driving Simulation Conference (DSC). Antibes, France. 


## Features and Contributions

Our version builds on the original by incorporating:

- Enhanced mixed reality integration for more immersive driving simulations.
- Expanded vehicle models and environmental settings for comprehensive scenario testing.
- Improved networking and data logging capabilities to support complex experimental designs.

We welcome contributions from the broader research community, including feature enhancements, bug fixes, or documentation improvements. For collaboration inquiries, please contact us.

## Acknowledgments

This project is part of ScaDS.AI Dresden/Leipzig, supported under the federal government's AI strategy. It represents a collaborative effort to push the boundaries of research in machine learning, HCI, and mobility. Our team is committed to developing computational models that simulate human-like interaction behavior, contributing to a deeper understanding of user interfaces and interaction design.

Join us in advancing the field of mixed reality and driving simulation for more insightful and impactful research.


# Integration of Varjo XR-3 for Mixed Reality Driving Simulation

## Overview

This document provides step-by-step instructions for setting up and developing a mixed reality driving simulation using the Varjo XR-3 headset. The Varjo XR-3 offers cutting-edge mixed reality capabilities, combining high-resolution visuals and real-world pass-through to create immersive experiences. Our model leverages these features to simulate driving in a virtual environment while sitting in a real car model, which will be integrated into the system upon its arrival.

## Requirements

- **Hardware**: A PC with Windows 10 or Windows 11, equipped with a GPU that supports the required number of HDMI ports for the Varjo XR-3. The exact number of ports will be specified later.
- **Software**: Varjo Base, Varjo Lab Tools, Steam, and SteamVR.

## Initial Setup

### Varjo Software Installation

1. Install Varjo Base and Varjo Lab Tools from the Varjo website. These applications are essential for managing the headset and configuring the mixed reality settings.
2. Ensure your graphics drivers are up to date to avoid compatibility issues.

### Steam and SteamVR

1. Install Steam and SteamVR. SteamVR is necessary for utilizing the base stations, which enable motion tracking.

### Hardware Connections

1. Connect the Varjo XR-3 headset to your PC, ensuring you use the correct HDMI ports or adapters specified for your hardware configuration.

## Unity Project Configuration

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

