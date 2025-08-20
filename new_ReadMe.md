\# CIAO's Mixed Reality Driving Simulator

\## Mixed Reality Implementation Using Varjo XR-3 and HDRP in Unity



---



## Table of Contents

1. [About](#about)

2. [Project Origin](#project-origin)

3. [Goal of the Project](#goal-of-the-project)

4. [Features](#features)

5. [Tech Stack](#tech-stack)

6. [Environment Setup](#environment-setup)

7. [NPCs](#npcs)

8. [Setup](#setup)

9. [Usage](#usage)

10. [Troubleshooting / Known Issues](#troubleshooting--known-issues)

11. [Demo Video](#demo-video)

12. [Contributions](#contributions)

13. [Acknowledgments](#acknowledgments)

14. [Citation](#citation)

15. [Contact](#contact)



---


## About

We are a Junior Research Group "CIAO" (Computational Interaction and Mobility) at Leipzig University. Our research lies at the intersection of machine learning, human-computer interaction, and mobility. We are part of the Center for Scalable Data Analytics and Artificial Intelligence (ScaDS.AI) Dresden/Leipzig, one of five new AI centers in Germany funded under the federal government’s AI strategy.



---

## Project Origin

This project is a modified version of a coupled simulator originally developed by Dr. Pavlo Bazilinskyy at TU Eindhoven for research on driver-pedestrian interactions.  



We have adapted the original simulator to a \*\*Mixed-Reality solution\*\* using the Varjo XR-3 HMD and HDRP in Unity, integrating a fixed-based driving simulator with a virtual driving environment. This allows for in-depth research on driver-interface interaction in realistic simulated conditions.



---



## Goal of the Project

This open-source project aims to foster a community of researchers and developers interested in advancing mixed reality applications for mobility and interaction studies. Contributions, collaborations, and discussions are encouraged to enhance the simulator's capabilities and research applicability.



---



## Features

- Eye Tracking

- Mixed Reality

- Head Tracking



---


## Tech Stack

**Software:**

- Unity Hub

- Unity 2022.3.5f1

- Varjo Base \& Varjo Lab Tools

- Steam \& SteamVR

- Varjo SDK for Unity (pre-installed)

- Blender 4.0 or higher

- DirectX11



---



## Environment Setup

The simulator environment replicates a real-world urban setting with buildings, trees, parked vehicles, and dynamic NPCs performing animations such as walking, running, or sitting.



| Key Components | Description |

|----------------|-------------|

| Roads \& Sidewalks | Base navigation surfaces for vehicles and pedestrians |

| Pedestrians | Dynamic agents performing animations |

| Parked Vehicles | Static props affecting pathfinding and realism |

| Buildings | Environmental context and occlusion |

| Wind Turbines | Background scenery, optional interactive props |

| Trees | Decorative and cover elements |



### Hierarchy of the Environment

(*Insert environment picture here*)



---



## NPCs

### Pedestrians

(*Insert hierarchy picture here*)



### Idle Characters

(*Insert hierarchy picture here*)



### Extra

To hide environment objects for performance:

1. Select the object in the Main Scene Hierarchy.

2. In the Inspector, uncheck the box next to the object’s name.



---



## Setup



### Prerequisites

**Hardware:**

- PC with Windows 10 or 11

- GPU with two ports connected directly (Varjo XR-3 requirement)

- Four SteamVR Base Stations 2.0

- Fixed-Based Driving Simulator (Ergoneers)



---



### Installation \& Running Locally

(*Insert step-by-step installation instructions and pictures here*)



---



## Usage

(*Insert instructions or usage examples here*)



---


## Troubleshooting / Known Issues


### Pedestrians Not Moving



**Problem:** Pedestrians in the scene are not performing animations.  



**Possible Causes \& Fixes:**



1. **Animation Controller Issue**

 - Ensure animations are connected to the **"Entry" Node**.
   - Delete and re-drag animations from the character folder into the Animation Controller.
   - Check Rig settings:
     - Select `.fbx` → Inspector → Rig tab  
     - Animation Type: Humanoid  
     - Avatar Definition: Create From This Model

2. **Missing Script**
   - Only affects characters prefixed with **"Pedestrian_"**.  
   - Idle_Characters use Animator only.
   - Compare inspector to reference screenshot to confirm setup.

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

## Contact
(*Insert contact info: email, GitHub, etc.*)