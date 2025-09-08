
# CIAO's Mixed Reality Driving Simulator

## Mixed Reality Implementation Using Varjo XR-3 and HDRP in Unity

  <p>
    <img src="ReadMeImgs/ENV2.png" width="500" alt="Hierarchy_Environment">
    <img src="ReadMeImgs/ENV1.png" width="500" alt="Hierarchy_Environment">
  </p>

---



## Table of Contents

1. [About](#about)

2. [Project Origin](#project-origin)

3. [Goal of the Project](#goal-of-the-project)

4. [Tech Stack](#tech-stack)

5. [Environment](#environment)

6. [NPCs](#npcs)

7. [Dashboard](#dashboard)

8. [IVIS](#ivis)

9. [AI Driver](#ai-driver)

10. [Setup](#setup)

11. [Usage](#usage)

12. [Troubleshooting / Known Issues](#troubleshooting--known-issues)

13. [Demo Video](#demo-video)

14. [Contributions](#contributions)

15. [Acknowledgments](#acknowledgments)

16. [Citation](#citation)




---

## About

We are a Junior Research Group "CIAO" (Computational Interaction and Mobility) at Leipzig University. Our research lies at the intersection of machine learning, human-computer interaction, and mobility. We are part of the Center for Scalable Data Analytics and Artificial Intelligence (ScaDS.AI) Dresden/Leipzig, one of five new AI centers in Germany funded under the federal government’s AI strategy.

This project reflects our interest in advancing mixed-reality applications for mobility research, providing a platform for studying driver-interface interaction in simulated environments. It is designed for researchers, developers, and students interested in exploring human-computer interaction in realistic driving scenarios.


---


## Project Origin

This project is a heavily modified version of a coupled simulator originally developed by Dr. Pavlo Bazilinskyy at TU Eindhoven for research on driver-pedestrian interactions. 

Our adaptation combines a fixed-based driving simulator with a virtual environment into a Mixed-Reality solution powered by the Varjo XR-3 HMD. It features two additional screens (dashboard and IVIS) and has been migrated to the HDRP in Unity. This setup provides a platform for in-depth research on driver-interface interaction under realistic simulated conditions.



---



## Goal of the Project

This project aims to serve as a valuable contribution to the open-source landscape directed at researchers and developers interested in advancing mixed reality applications for mobility and interaction studies. Further contributions, collaborations, and discussions are encouraged to enhance the simulator's capabilities and research applicability.



---


## Tech Stack

**Software:**

- Unity 2022.3.5f1

- Varjo Base \& Varjo Lab Tools

- Steam \& SteamVR

- Varjo SDK for Unity (pre-installed)

- DirectX11

- vJoy


---



## Environment

The simulator replicates a realistic urban environment, integrating roads, buildings, trees, and animated pedestrians. The non-playable characters (NPCs) exhibit a range of behaviors including walking, resting, and interacting to help create a dynamic and immersive scene.

| Key Components    | Description                                           |
|------------------|-------------------------------------------------------|
| Roads & Sidewalks | Base navigation surfaces for vehicles and pedestrians |
| Pedestrians       | Dynamic agents performing animations                  |
| Parked Vehicles   | Static props affecting realism                       |
| Buildings         | Environmental context                                |
| Wind Turbines     | Background scenery, optional interactive props       |
| Trees             | Decorative elements                                  |


### Hierarchy of the Environment
  <p>
    <img src="ReadMeImgs/Hierarchy.png" height="500" alt="Hierarchy_Environment">
  </p>


---

## NPCs

There are both idle and active pedestrians featured in the scene. The active pedestrians follow their own Waypoint cycles and currently do no have colliders.

---

## Dashboard


  <p>
    <img src="ReadMeImgs/Dashboard.png" width="700" alt="Hierarchy_Environment">
  </p>


The dashboard display shows key driving information:

- Top: Time, date, mock temperature.
- Left side: Brake and throttle input are displayed as vertical bars; Current gear is highlighted, along with inactive gears.
- Center: The steering wheel angle and current speed.
- Right side: Additional driving metrics including speed, elapsed time, distance traveled, and Mock Consumption.

There is a range of buttons, mostly for decoration. Only the left and right blinking icons are mapped to inputs; all shortly light up, then fade out on start up.

---

## IVIS

  <p align="center">
    <img src="ReadMeImgs/IVIS.png" width="700" alt="IVIS">
  </p>
  
  The IVIS features a Birds-Eye-View of the Player Car in a simplified environment. Pedestrians and other vehicles are marked in light green. There are sub-menus to play music and adjust volume.
  
  <p>
    <img src="ReadMeImgs/IVIS_Start.png" width="500" alt="Hierarchy_Environment"><img src="ReadMeImgs/IVIS_Home.png" width="500" alt="Hierarchy_Environment">
  </p>
  
There are two additional screens: A Start screen, and a Home screen. These currently are mock ups only, and clicking buttons will only toggle their visibility.


---

## AI Driver

The player car can either be manually driven, or control can be handed over to an AI driver. The AI driver will follow a Waypoint Cycle (RCC AI driver script), and move the physical steering wheel of the car.


## Setup



### Prerequisites

**Hardware:**

- PC with Windows 10 or 11

- GPU with two ports connected directly (Varjo XR-3 requirement)

- Four SteamVR Base Stations 2.0

- Fixed-Based Driving Simulator (Ergoneers)

**Software:**

- Realistic Car Controller (Bonecracker Games) -- A license for this asset is needed. You can buy it in the [Unity Asset Store](https://assetstore.unity.com/packages/tools/physics/realistic-car-controller-16296?srsltid=AfmBOorEkki7rHdWpDJJYPRQbDawsVYkARHaRmIliXc-epry_dH2PXpR)



This Simulation was build for use with the Ergoneers Fixed-Based Driving Simulator. If you do not have this, the simulation should be adaptable to run without by modifiying the input system and Mixed-Reality set up.


---



### Installation \& Running Locally



---



## Usage




---


## Troubleshooting / Known Issues


### Pedestrians Not Moving



**Problem:** Pedestrians in the scene are not performing animations.  



**Possible Causes \& Fixes:**



1. **Animation Controller Issue**
 - Ensure animations are connected to the **"Entry" Node**.
   - Delete and re-drag animations from the character folder into the Animation Controller.
 - Check **Character's Rig** settings:
    1. Go to **Assets → Models → People → <Character_Name>**. 
    2. Select the character’s .fbx. 
    3. In the **Inspector**, open the **Rig** tab.
        - Animation Type: **Humanoid** 
        - Avatar Definition: **Create From This Model** 
    4. Re-apply these settings. 
    
  - Check **Animation's Rig** settings: 
    1. In the Project window, select the animation 
    2. In the **Rig** tab: 
        - Animation Type: **Humanoid** 
        - Avatar Definition: **Copy From Other Avatar** 
        - Source: The character’s avatar 
    3. Re-apply these settings.

2. **Missing Script**
   - Only affects characters prefixed with **"Pedestrian_"**.  
   - Idle_Characters use Animator only.
   - Compare inspector to reference screenshot to confirm setup.
    <p>
      <img src="READme_Images/Pedestrian_Inspector_Overview.png" height="500" alt="Pedestrian inspector overview">
    </p>
  **Figure 1**: *This is what the Inspector of your pedestrian should look like. If any scripts are missing, make sure to find them in the project folder and add them to the Inspector. This is also important if you want to add new characters to the scene.*

**Tip:** Multiple pedestrian failures usually indicate a controller problem; single failures may be rig or missing script issues.

---

## Demo Video
(*Insert link or embedded video here*)

---

## Contributions
We welcome contributions! Please fork the repository, make changes, and submit a pull request. Ensure coding standards and documentation are maintained.

---

## Acknowledgments
- ScaDS.AI Dresden/Leipzig, supported under Germany’s AI strategy.
- Original simulator by Dr. Pavlo Bazilinskyy at TU Eindhoven.

---

## Citation
If using this simulator for research, cite:

Bazilinskyy, P., Kooijman, L., Dodou, D., & De Winter, J. C. F. (2020). Coupled simulator for research on the interaction between pedestrians and (automated) vehicles. 19th Driving Simulation Conference (DSC). Antibes, France.

---
