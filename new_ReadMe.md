\# **Project Name: CIAO's Mixed Reality Driving Simulator**

**## Sub-Title: Mixed Reality Implementation Using Varjo XR-3 and HDRP in Unity**



\## **Table of Contents**

1\. \[About](#about)

2\. \[Features](#features)

3\. \[Tech Stack](#tech-stack)

4\. \[Environment Setup](#environment-setup)

5\. \[Setup](#setup)

6\. \[Usage](#usage)

7\. \[Troubleshooting / Known Issues](#troubleshooting-/-known Issues)

8\. \[Contributions](#contributions)

9\. \[Acknowledgments](#acknowledgments)

10\. \[Citation](#citation)

11\. \[Contact](#contact)



\## **About**

We are a Junior Research Group "CIAO" (Computational Interaction and Mobility) and work at the intersection of machine learning, human-computer interaction, and mobility. We are part of the Center for Scalable Data Analytics and Artificial Intelligence (ScaDS.AI) at Leipzig University.

ScaDS.AI Dresden/Leipzig is one of five new AI centers in Germany funded under the federal government’s AI strategy.









\### **Project Origin**

This project is a modified version of a Coupled simulator for research on driver-pedestrian interactions made in Unity originally developed by Dr. Pavlo Bazilinskyy at TU Eindhoven.



The original simulation was designed for academic research, offering insights into the interaction between pedestrians and (automated) vehicles.

We have forked Dr. Bazilinskyy's project and tailored it to meet our specific research needs, adding new features and expanding its use cases.Our version builds on the original by combining a fixed-based driving simulator with a simulated driving environment into a Mixed-Reality soltion powered by Varjo XR-3 HMD. This adaptation provides a platform for in-depth research into driver-interface interaction within a simulated driving context.





\### **Goal of the Project**

This project is open-source and aims to foster a community of researchers and developers interested in advancing mixed reality applications for mobility and interaction studies. We encourage contributions, collaborations, and discussions to enhance the simulator's capabilities and applicability in various research domains.



\## **Features**

 	- Eye Tracking

 	- Mixed Reality

 	- Head Tracking



\## **Tech Stack**

 	**Software:**
Unity Hub
Unity 2022.3.5f1

 		Varjo Base

 		Varjo Lab Tools

 		Steam and SteamVR

 		Varjo SDK for Unity (pre-installed in the project)

 		Blender 4.0 (or higher)

 		DirectX11



\## **Environment Setup**

The simulator environment is designed to replicate a real-world urban setting.  

To make the environment as realistic as possible, a variety of buildings, trees, and non-playable characters (NPCs) were added to the scene. NPCs perform various activities such as walking, running, or sitting to bring the world to life.  



| Key Components | Description |

|-----------------|-------------|

| Roads \& Sidewalks | Base navigation surfaces for vehicles and pedestrians |

| Pedestrians | Dynamic agents performing animations (walking, running, sitting) |

| Parked Vehicles | Static props that affect pathfinding and realism |

| Buildings | Environmental context and occlusion |

| Wind Turbines | Background scenery, optional interactive props |

| Trees | Decorative + cover elements |



\### Hierarchy of the Environment

(\*\*Insert Picture\*\*)



\## NPCs 

\### Hierarchy of Pedestrians

&nbsp;(\*\*Insert Picture\*\*)



\### Hierarchy of Idle\_Characters

(\*\*Insert Picture\*\*)



\### Extra

To hide aspects of the environment:

 	1. Select the object in the Main Scene Hierarchy.

 	2. In the Inspector, uncheck the box next to the object’s name.

If performance drops, consider hiding large environment groups (such as the buildings)





\## **Setup**

\### **Prerequisites**

 	**Hardware:**

 		A PC with Windows 10 or Windows 11

 		Compatible PC fulfilling Varjo requirements

 		GPU with two ports directly connected to it (e.g. 2 HDMI ports leading directly to the GPU). Some Laptops \& PCs don't have this feauture. Check Varjo XR-3 Requirements.

 		four SteamVR Basestations 2.0.

 		Fixed-Based Driving Simulator from Ergoneers



\### **Installation**

\### **Run Locally**



**--> Intermission: Pictures**



\## **Usage**



\## Troubleshooting / Known Issues  



\### Pedestrians Not Moving  



\*\*Problem:\*\* Pedestrians placed in the scene are not performing their animations.  

\*\*Possible Causes:\*\*  



---



\#### 1. Issue with Animation Controller  

\- Make sure there is an animation connected to the \*\*"Entry" Node\*\*.  

&nbsp; - Even if one is connected, delete it and re-drag the animation from the character’s folder into the Animation Controller. It should automatically connect to the Entry node.  



\- Keep in mind: not all characters have individual controllers.  

&nbsp; - Many pedestrians share the same \*\*"Pedestrian\_Controller"\*\*.  

&nbsp; - If multiple characters (e.g., \*Pedestrian\_Claudia\* and \*Pedestrian\_Eric\*) are not working, the issue is likely with the controller itself.  



\- If the animation is still not working, check the \*\*Rig settings\*\*:  

&nbsp; 1. Go to \*\*Assets → Models → People → <Character\_Name>\*\*.  

&nbsp; 2. Select the character’s `.fbx`.  

&nbsp; 3. In the \*\*Inspector\*\*, open the \*\*Rig\*\* tab.  

&nbsp;    - Animation Type: \*\*Humanoid\*\*  

&nbsp;    - Avatar Definition: \*\*Create From This Model\*\*  

&nbsp; 4. Re-apply these settings.  



\- Next, check the \*\*Animation file\*\* itself:  

&nbsp; 1. In the Project window, select the animation (📷 \*Insert screenshot of icon\*).  

&nbsp; 2. In the \*\*Rig\*\* tab:  

&nbsp;    - Animation Type: \*\*Humanoid\*\*  

&nbsp;    - Avatar Definition: \*\*Copy From Other Avatar\*\*  

&nbsp;    - Source: The character’s avatar  

&nbsp; 3. Re-apply these settings.  



---



\#### 2. Missing Script  

\- This issue only affects characters with \*\*"Pedestrian\_"\*\* in front of their names.  

\- \*\*Idle\_Characters\*\* do not use scripts — they only have an Animator.  



\- To confirm:  

&nbsp; - Select the pedestrian in the \*\*Inspector\*\*.  

&nbsp; - Compare to the reference screenshot (📷 \*Insert picture here\*).  



---

&nbsp;\*\*Tip:\*\* If several pedestrians are failing at once, it’s usually an Animator Controller problem. If only one pedestrian is failing, check the Rig or missing script.  



\### **Demo Video**



**## Contributions**



\## **Acknowledgments**

This project is part of ScaDS.AI Dresden/Leipzig, supported under the federal government's AI strategy. originally developed by Dr. Pavlo Bazilinskyy at TU Eindhoven.





\## **Citation**

If you utilize this modified simulator for academic purposes, please cite the original work:



Bazilinskyy, P., Kooijman, L., Dodou, D., \& De Winter, J. C. F. (2020). Coupled simulator for research on the interaction between pedestrians and (automated) vehicles. 19th Driving Simulation Conference (DSC). Antibes, France.



\## **Contact**

