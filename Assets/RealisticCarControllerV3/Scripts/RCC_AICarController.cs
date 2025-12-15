//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------




using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Barmetler;
using HealthbarGames;
//using UnityEditor.UI;

/// <summary>
/// AI Controller of RCC. It's not professional, but it does the job. Follows all waypoints, or follows/chases the target gameobject.
/// </summary>
[RequireComponent(typeof(RCC_CarControllerV3))]
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/RCC AI Car Controller")]




public class RCC_AICarController : MonoBehaviour
{

    /// <summary>
    /// Car controller.
    /// </summary>
    public RCC_CarControllerV3 CarController
    {
        get
        {
            if (_carController == null)
                _carController = GetComponentInParent<RCC_CarControllerV3>();
            return _carController;
        }
    }
    private RCC_CarControllerV3 _carController;


    [Header("Distance Keeping")]
    public float safeDistance = 10f;
    public float brakingForce = 2f;


    /// <summary>
    /// Waypoints Container.
    /// </summary>
    public RCC_AIWaypointsContainer waypointsContainer;

    /// <summary>
    ///  Current index in Waypoint Container.
    /// </summary>
    public int currentWaypointIndex = 0;

    /// <summary>
    /// Search and chase Gameobjects with tags.
    /// </summary>
    public string targetTag = "Player";

    public NavigationMode navigationMode = NavigationMode.FollowWaypoints;

    /// <summary>
    /// AI Type.
    /// </summary>
    public enum NavigationMode { FollowWaypoints, ChaseTarget, FollowTarget }

    /// <summary>
    /// Raycast distances used for detecting obstacles at front of the AI vehicle.
    /// </summary>
    [Range(3f, 30f)] public float raycastLength = 3f;

    /// <summary>
    /// Raycast distances used for detecting obstacles at front of the AI vehicle.
    /// </summary>
    [Range(10f, 90f)] public float raycastAngle = 30f;

    /// <summary>
    /// Raycast distances used for detecting obstacles at front of the AI vehicle.
    /// </summary>
    public LayerMask obstacleLayers = -1;

    /// <summary>
    /// Current detected obstacle.
    /// </summary>
    public GameObject obstacle;

    /// <summary>
    /// Using forward and sideways raycasts to avoid obstacles.
    /// </summary>
    public bool useRaycasts = true;

    /// <summary>
    /// Raycast origin, offset.
    /// </summary>
    public Vector3 rayOrigin = new Vector3(0f, .1f, 2f);

    /// <summary>
    /// Total ray input affected by raycast distances.
    /// </summary>
    private float rayInput = 0f;

    /// <summary>
    /// Raycasts hits an obstacle now?
    /// </summary>
    private bool raycasting = false;

    private bool pedestrianDetected = false;

    /// <summary>
    /// Crosswalk Zone with Stop line
    /// </summary>
    private CrosswalkZone currentCrosswalkZone;


    private Transform stopLineTarget;

    /// <summary>
    /// Are we in a Crosswalk Zone?
    /// </summary>
    private bool inCrosswalkZone = false;

    private float distanceToStopLine = Mathf.Infinity;

    private bool mustStopForLight = false;

    private bool mustStopForCar = false;

    /// <summary>
    /// This timer was used for deciding go back or not, after crashing.
    /// </summary>
    private float resetTime = 0f;

    /// <summary>
    /// Reversing now?
    /// </summary>
    private bool reversingNow = false;

    // Steer, Motor, And Brake inputs. Will feed RCC_CarController with these inputs.

    /// <summary>
    /// Steer input.
    /// </summary>
    public float steerInput = 0f;

    /// <summary>
    /// Throttle input.
    /// </summary>
    public float throttleInput = 0f;

    /// <summary>
    /// Brake input.
    /// </summary>
    public float brakeInput = 0f;

    /// <summary>
    /// Handbrake input.
    /// </summary>
    public float handbrakeInput = 0f;

    /// <summary>
    /// Limit speed.
    /// </summary>
    public bool limitSpeed = false;

    /// <summary>
    /// Maximum speed.
    /// </summary>
    public float maximumSpeed = 100f;

    /// <summary>
    /// Smoothed steering.
    /// </summary>
    public bool smoothedSteer = true;

    // Counts laps and how many waypoints were passed.

    /// <summary>
    /// Total lap count.
    /// </summary>
    public int lap = 0;

    /// <summary>
    /// Stop after this lap.
    /// </summary>
    public bool stopAfterLap = false;

    /// <summary>
    /// Stop after this lap.
    /// </summary>
    public int stopLap = 10;

    /// <summary>
    /// Total waypoints passed.
    /// </summary>
    public int totalWaypointPassed = 0;

    /// <summary>
    /// Ignoring waypoint position due to an obstacle.
    /// </summary>
    public bool ignoreWaypointNow = false;

    /// <summary>
    /// Detector radius.
    /// </summary>
    public int detectorRadius = 200;

    /// <summary>
    /// Start to follow distance.
    /// </summary>
    public int startFollowDistance = 300;

    /// <summary>
    /// Stop to follow distance.
    /// </summary>
    public int stopFollowDistance = 30;

    /// <summary>
    /// Updating the targets.
    /// </summary>
    private bool updateTargets = false;
    private float lastUpdatedTargets = 0f;

    /// <summary>
    /// Unity's Navigator.
    /// </summary>
    private NavMeshAgent navigator;

    /// <summary>
    /// Detector with Sphere Collider. Used for finding target Gameobjects in chasing mode.
    /// </summary>
    public List<Transform> targetsInZone = new List<Transform>();
    public List<RCC_AIBrakeZone> brakeZones = new List<RCC_AIBrakeZone>();
    public List<RCC_AISlowZone> slowZones = new List<RCC_AISlowZone>();
    /// <summary>
    /// Target Gameobject for chasing.
    /// </summary>
    public Transform targetChase;

    /// <summary>
    /// Target brakezone.
    /// </summary>
    public RCC_AIBrakeZone targetBrake;

    /// <summary>
    /// Target brakezone.
    /// </summary>
    public RCC_AISlowZone targetSlow;

    /// <summary>
    /// Firing an event when each RCC AI vehicle spawned / enabled.
    /// </summary>
    /// <param name="RCCAI"></param>
    public delegate void onRCCAISpawned(RCC_AICarController RCCAI);
    public static event onRCCAISpawned OnRCCAISpawned;

    /// <summary>
    /// Firing an event when each RCC AI vehicle disabled / destroyed.
    /// </summary>
    /// <param name="RCCAI"></param>
    public delegate void onRCCAIDestroyed(RCC_AICarController RCCAI);
    public static event onRCCAIDestroyed OnRCCAIDestroyed;

    private void Awake()
    {

        // If Waypoints Container is not selected in Inspector Panel, find it on scene.
        if (!waypointsContainer)
            waypointsContainer = FindObjectOfType(typeof(RCC_AIWaypointsContainer)) as RCC_AIWaypointsContainer;

        // Creating our Navigator and setting properties.
        GameObject navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.SetParent(transform, false);
        navigator = navigatorObject.AddComponent<NavMeshAgent>();
        navigator.radius = 1;
        navigator.speed = 1;
        navigator.angularSpeed = 100000f;
        navigator.acceleration = 100000f;
        navigator.height = 1;
        navigator.avoidancePriority = 0;

    }

    private void OnEnable()
    {

        //  Setting external controller on enable.
        CarController.externalController = true;

        // Calling this event when AI vehicle spawned.
        if (OnRCCAISpawned != null)
            OnRCCAISpawned(this);

    }

    private void Update()
    {

        // If not controllable, no need to go further.
        if (!CarController.canControl)
            return;

        //  If limit speed is not enabled, maximum speed is same with vehicle's maximum speed.
        if (!limitSpeed)
            maximumSpeed = CarController.maxspeed;

        // Assigning navigator's position to front wheels of the vehicle
        navigator.transform.localPosition = Vector3.zero;
        navigator.transform.localPosition += Vector3.forward * CarController.FrontLeftWheelCollider.transform.localPosition.z;

        CheckTargets();     //  Checking targets if navigation mode is set to chase or follow target mode.
        CheckBrakeZones();      //  Checking existing brake zones in the scene.
        CheckSlowZones(); //ADDED

        if (!updateTargets)
            lastUpdatedTargets += Time.deltaTime;

        if (lastUpdatedTargets >= 1f)
            updateTargets = true;

    }

    private void FixedUpdate()
    {

        // If not controllable, no need to go further.
        if (!CarController.canControl)
            return;

        //  If enabled, raycasts will be used to avoid obstacles at runtime.
        if (useRaycasts)
            FixedRaycasts();        // Recalculates steerInput if one of raycasts detects an object front of AI vehicle.

        Navigation();       // Calculates steerInput based on navigator.
        CheckReset();       // Used for deciding go back or not after crashing.
        FeedRCC();      // Feeds inputs of the RCC.

    }
    private void Navigation()
    {

        // Navigator Input is multiplied by 1.5f for fast reactions.
        float navigatorInput = Mathf.Clamp(transform.InverseTransformDirection(navigator.desiredVelocity).x * 1f, -1f, 1f);

        if (navigatorInput > .4f)
            navigatorInput = 1f;

        if (navigatorInput < -.4f)
            navigatorInput = -1f;

        //  Navigation has three modes.
        switch (navigationMode)
        {

            case NavigationMode.FollowWaypoints:

                // If our scene doesn't have a Waypoint Container, stop and return with error.
                if (!waypointsContainer)
                {

                    Debug.LogError("Waypoints Container Couldn't Found!");
                    Stop();
                    return;

                }

                // If our scene has Waypoints Container and it doesn't have any waypoints, stop and return with error.
                if (waypointsContainer && waypointsContainer.waypoints.Count < 1)
                {

                    Debug.LogError("Waypoints Container Doesn't Have Any Waypoints!");
                    Stop();
                    return;

                }

                //	If stop after lap is enabled, stop at target lap.
                if (stopAfterLap && lap >= stopLap)
                {

                    Stop();
                    return;

                }

                // Next waypoint and its position.
                RCC_Waypoint currentWaypoint = waypointsContainer.waypoints[currentWaypointIndex];

                // Checks for the distance to next waypoint. If it is less than written value, then pass to next waypoint.
                float distanceToNextWaypoint = Vector3.Distance(transform.position, currentWaypoint.transform.position);

                float targetSpeed = currentWaypoint.targetSpeed;

                // Setting destination of the Navigator.
                if (navigator.isOnNavMesh)
                    navigator.SetDestination(waypointsContainer.waypoints[currentWaypointIndex].transform.position);

                //  If distance to the next waypoint is not 0, and close enough to the vehicle, increase index of the current waypoint and total waypoint.
                if (distanceToNextWaypoint != 0 && distanceToNextWaypoint < waypointsContainer.waypoints[currentWaypointIndex].radius)
                {

                    currentWaypointIndex++;
                    totalWaypointPassed++;

                    // If all waypoints were passed, sets the current waypoint to first waypoint and increase lap.
                    if (currentWaypointIndex >= waypointsContainer.waypoints.Count)
                    {

                        currentWaypointIndex = 0;
                        lap++;

                    }

                    // Setting destination of the Navigator. 
                    if (navigator.isOnNavMesh)
                        navigator.SetDestination(waypointsContainer.waypoints[currentWaypointIndex].transform.position);

                }


                if (inCrosswalkZone && currentCrosswalkZone != null)
                {
                    //Debug.Log(currentCrosswalkZone);
                    //Debug.Log(currentCrosswalkZone.GetPhase());
                    //Debug.Log(currentCrosswalkZone.GetPhase().GetState());
                    var phaseState = currentCrosswalkZone.GetPhase().GetState();

                    mustStopForLight = (phaseState == TrafficLightBase.State.Stop ||
                    phaseState == TrafficLightBase.State.PrepareToStop);
                    //if (mustStopForLight) { Debug.Log("RED!"); } 
                }

                if (inCrosswalkZone && stopLineTarget != null && (pedestrianDetected || mustStopForLight))
                {
                    distanceToStopLine = mustStopForCar ? 0f : Vector3.Distance(transform.position, stopLineTarget.position);
                    //Debug.Log(distanceToStopLine);

                    if (CarController.CompareTag("EventTruck"))
                    {
                        return;
                    }

                    else if (CarController.speed <= 1f)
                    {
                        throttleInput = 0f;
                        brakeInput = 0f;
                        handbrakeInput = 1f;
                        CarController.direction = 1;
                    }
                    else if (distanceToStopLine > 8f)
                    {
                        //Debug.Log("slowing down");
                        throttleInput = 0f;
                        brakeInput = Mathf.Lerp(brakeInput, 0f, Time.deltaTime * 2f);
                        CarController.direction = 1;
                    }
                    else if (distanceToStopLine > 3f)
                    {
                        //Debug.Log("slowing down hard");
                        throttleInput = 0f;
                        brakeInput = Mathf.Lerp(brakeInput, 0.4f, Time.deltaTime * 3f);
                        CarController.direction = 1;
                    }
                    else
                    {
                        //Debug.Log("STOP!");
                        throttleInput = 0f;
                        brakeInput = 0f;
                        steerInput = 0f;
                        handbrakeInput = 1f;
                        CarController.direction = 1;
                    }

                    //if (distanceToStopLine <= 1f && mustStopForLight)
                    //{
                        //Debug.Log("stop");
                        //CarController.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    //}

                    ignoreWaypointNow = true;
                    return;
                }

                // TO CHECK - should this be else if or if?

                //  If vehicle goes forward, calculate throttle and brake inputs.
                else if (!reversingNow)
                {
                    //throttleInput = (distanceToNextWaypoint < (currentWaypoint.radius * (CarController.speed / 30f)))
                    //? Mathf.Clamp01(targetSpeed - CarController.speed)
                    //: 1f;
                    throttleInput = Mathf.Clamp01(targetSpeed - CarController.speed);

                    throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, CarController.speed / maximumSpeed));

                    brakeInput = (distanceToNextWaypoint < (currentWaypoint.radius * (CarController.speed / 30f)))
                                 ? (Mathf.Clamp01(CarController.speed - targetSpeed)*0.5f)
                                 : 0f;


                    // +++ slow down in relation to cars in front to avoid crashes (WIP)
                    if (obstacle != null && obstacle.CompareTag("AICar"))
                    {
                        float distanceToCar = Vector3.Distance(transform.position, obstacle.transform.position) - 7f;
                        float safeDistance = 5f;
                        //Debug.Log(distanceToCar);

                        float brakeFactor = Mathf.Clamp01((safeDistance - distanceToCar) / safeDistance);

                        // Apply braking proportional to proximity

                        brakeInput = Mathf.Max(brakeInput, brakeFactor);
                        throttleInput = Mathf.Clamp01(throttleInput * (1f - brakeFactor));

                        ignoreWaypointNow = (CarController.speed <= 1f);
                    }

                    //Debug.Log("Throttle: " + throttleInput);
                    //Debug.Log("Brake: " + brakeInput);


                    //throttleInput = (distanceToNextWaypoint < (waypointsContainer.waypoints[currentWaypointIndex].radius * (CarController.speed / 30f))) ? (Mathf.Clamp01(targetSpeed - CarController.speed)) : 1f;
                    //throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, (CarController.speed) / maximumSpeed));
                    //brakeInput = (distanceToNextWaypoint < (waypointsContainer.waypoints[currentWaypointIndex].radius * (CarController.speed / 30f))) ? (Mathf.Clamp01(CarController.speed - targetSpeed)) : 0f;
                    handbrakeInput = 0f;

                    //  If vehicle speed is high enough, calculate them related to navigator input. This will reduce throttle input, and increase brake input on sharp turns.
                    if (CarController.speed > 30f)
                    {

                        throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                        //brakeInput += Mathf.Abs(navigatorInput) / 3f;

                    }

                    ignoreWaypointNow = false;

                }

                break;

            case NavigationMode.ChaseTarget:

                // If our scene doesn't have a target to chase, stop and return.
                if (!targetChase)
                {

                    Stop();
                    return;

                }

                // Setting destination of the Navigator. 
                if (navigator.isOnNavMesh)
                    navigator.SetDestination(targetChase.position);

                //  If vehicle goes forward, calculate throttle and brake inputs.
                if (!reversingNow)
                {

                    throttleInput = 1f;
                    throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, (CarController.speed) / maximumSpeed));
                    brakeInput = 0f;
                    handbrakeInput = 0f;

                    //  If vehicle speed is high enough, calculate them related to navigator input. This will reduce throttle input, and increase brake input on sharp turns.
                    if (CarController.speed > 30f)
                    {

                        throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                        //brakeInput += Mathf.Abs(navigatorInput) / 3f;

                    }

                }

                break;

            case NavigationMode.FollowTarget:

                // If our scene doesn't have a Waypoints Container, return with error.
                if (!targetChase)
                {

                    Stop();
                    return;

                }

                // Setting destination of the Navigator. 
                if (navigator.isOnNavMesh)
                    navigator.SetDestination(targetChase.position);

                // Checks for the distance to target. 
                float distanceToTarget = Vector3.Distance(transform.position, targetChase.position);

                //  If vehicle goes forward, calculate throttle and brake inputs.
                if (!reversingNow)
                {

                    throttleInput = distanceToTarget < (stopFollowDistance * Mathf.Lerp(1f, 5f, CarController.speed / 50f)) ? Mathf.Lerp(-5f, 1f, distanceToTarget / (stopFollowDistance / 1f)) : 1f;
                    throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, (CarController.speed) / maximumSpeed));
                    brakeInput = distanceToTarget < (stopFollowDistance * Mathf.Lerp(1f, 5f, CarController.speed / 50f)) ? Mathf.Lerp(5f, 0f, distanceToTarget / (stopFollowDistance / 1f)) : 0f;
                    handbrakeInput = 0f;

                    //  If vehicle speed is high enough, calculate them related to navigator input. This will reduce throttle input, and increase brake input on sharp turns.
                    if (CarController.speed > 30f)
                    {

                        throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                        //brakeInput += Mathf.Abs(navigatorInput) / 3f;

                    }

                    if (throttleInput < .05f)
                        throttleInput = 0f;
                    if (brakeInput < .05f)
                        brakeInput = 0f;

                }

                break;

        }

        //  If vehicle is in brake zone, apply brake input.
        if (targetBrake)
        {

            //  If vehicle is in brake zone and speed of the vehicle is higher than the target speed, apply brake input.
            if (Vector3.Distance(transform.position, targetBrake.transform.position) < targetBrake.distance && CarController.speed > targetBrake.targetSpeed)
            {

                throttleInput = 0f;
                brakeInput = 1f;

            }

        }


        // ADDED slow zone; no brake input, just no throttle input to not go any faster

        if (targetSlow)
        {
            //  If vehicle is in slow zone and speed of the vehicle is higher than the target speed, no more throttle but no brake.
            if (Vector3.Distance(transform.position, targetSlow.transform.position) < targetSlow.distance && CarController.speed > targetSlow.targetSpeed)
            {

                throttleInput = 0f;
                Debug.Log("SLOW ZONE");
                brakeInput = 0f;

            }

        }




        if (brakeInput > .25f)
            throttleInput = 0f;

        // Steer input.
        if (obstacle != null && obstacle.CompareTag("AICar"))
            steerInput = navigatorInput;  // ignore rayInput
        else
            steerInput = (ignoreWaypointNow ? rayInput : navigatorInput + rayInput);

        //steerInput = (ignoreWaypointNow ? rayInput : navigatorInput + rayInput);
        steerInput = Mathf.Clamp(steerInput, -1f, 1f) * CarController.direction;

        //  Clamping inputs.
        throttleInput = Mathf.Clamp01(throttleInput);
        brakeInput = Mathf.Clamp01(brakeInput);
        handbrakeInput = Mathf.Clamp01(handbrakeInput);

        //  If vehicle goes backwards, set brake input to 1 for reversing.
        if (reversingNow)
        {

            throttleInput = 0f;
            brakeInput = 1f;
            handbrakeInput = 0f;

        }
        else
        {

            if (CarController.speed < 5f && brakeInput >= .5f)
            {

                brakeInput = 0f;
                handbrakeInput = 1f;

            }

        }

    }

    /// <summary>
    /// Vehicle will try to go backwards if crashed or stucked.
    /// </summary>
    private void CheckReset()
    {
        //Debug.Log(CarController.name + CarController.speed);
        // +++ car is NOT stuck if it is waiting for a pedestrian to cross the road
        if (pedestrianDetected || mustStopForLight || mustStopForCar)
        {
            reversingNow = false;
            resetTime = 0f;
            return;
        }
        // +++

        //  If navigation mode is set to follow, this means vehicle may stop. If vehicle is stopped near the target, no need to go backwards.
        if (targetChase && navigationMode == NavigationMode.FollowTarget && Vector3.Distance(transform.position, targetChase.position) < stopFollowDistance)
        {

            reversingNow = false;
            resetTime = 0;
            return;

        }

        // If unable to move forward, puts the gear to R.
        if (CarController.speed <= 5 && transform.InverseTransformDirection(CarController.Rigid.velocity).z <= 1f)
            resetTime += Time.deltaTime;


        //  If car is stucked for 2 seconds, reverse now.
        if (resetTime >= 2)
        {
            //Debug.Log("stuck");
            reversingNow = true;
        }

        //  If car is stucked for 4 seconds, or speed exceeds 25, go forward.
        if (resetTime >= 4 || CarController.speed >= 25)
        {
            reversingNow = false;
            resetTime = 0;

        }
    }

    /// <summary>
    /// Using raycasts to avoid obstacles.
    /// </summary>
    private void FixedRaycasts()
    {

        //  Creating five raycasts with angles.
        int[] anglesOfRaycasts = new int[3];
        anglesOfRaycasts[0] = 0;
        anglesOfRaycasts[1] = Mathf.FloorToInt(raycastAngle / 3f);
        // anglesOfRaycasts[1] = Mathf.FloorToInt(raycastAngle / 1f);
        // anglesOfRaycasts[2] = -Mathf.FloorToInt(raycastAngle / 1f);
        anglesOfRaycasts[2] = -Mathf.FloorToInt(raycastAngle / 3f);

        // Ray pivot position.
        Vector3 pivotPos = transform.position + transform.TransformVector(rayOrigin);



        //  Ray hit.
        RaycastHit hit;
        rayInput = 0f;
        bool casted = false;

        //  Casting rays.
        for (int i = 0; i < anglesOfRaycasts.Length; i++)
        {

            //  Drawing normal gizmos.
            Debug.DrawRay(pivotPos, Quaternion.AngleAxis(anglesOfRaycasts[i], transform.up) * transform.forward * raycastLength, Color.white);

            //  Casting the ray. If ray hits another obstacle...
            if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(anglesOfRaycasts[i], transform.up) * transform.forward, out hit, raycastLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform)
            {

                switch (navigationMode)
                {

                    case NavigationMode.FollowWaypoints:

                        //  Drawing hit gizmos.
                        Debug.DrawRay(pivotPos, Quaternion.AngleAxis(anglesOfRaycasts[i], transform.up) * transform.forward * raycastLength, Color.red);
                        casted = true;

                        //  Setting ray input related to distance to the obstacle.
                        if (i != 0)
                            rayInput -= Mathf.Lerp(Mathf.Sign(anglesOfRaycasts[i]), 0f, (hit.distance / raycastLength));

                        break;

                    case NavigationMode.ChaseTarget:

                        if (targetChase && hit.transform != targetChase && !hit.transform.IsChildOf(targetChase))
                        {

                            //  Drawing hit gizmos.
                            Debug.DrawRay(pivotPos, Quaternion.AngleAxis(anglesOfRaycasts[i], transform.up) * transform.forward * raycastLength, Color.red);
                            casted = true;

                            //  Setting ray input related to distance to the obstacle.
                            if (i != 0)
                                rayInput -= Mathf.Lerp(Mathf.Sign(anglesOfRaycasts[i]), 0f, (hit.distance / raycastLength));

                        }

                        break;

                    case NavigationMode.FollowTarget:

                        //  Drawing hit gizmos.
                        Debug.DrawRay(pivotPos, Quaternion.AngleAxis(anglesOfRaycasts[i], transform.up) * transform.forward * raycastLength, Color.red);
                        casted = true;

                        //  Setting ray input related to distance to the obstacle.
                        if (i != 0)
                            rayInput -= Mathf.Lerp(Mathf.Sign(anglesOfRaycasts[i]), 0f, (hit.distance / raycastLength));

                        break;

                }

                //  If ray hits an obstacle, set obstacle. Otherwise set it to null.
                if (casted)
                {
                    obstacle = hit.transform.gameObject;

                    mustStopForCar = obstacle.CompareTag("AICar");

                }

                /*
                    // +++ if it detects a car in front, keep distance
                    if (hit.collider.CompareTag("AICar"))
                    {
                        float ratio = Mathf.Clamp01(hit.distance / raycastLength);
                        throttleInput *= ratio;
                        if (ratio < 0.5f)
                            brakeInput = Mathf.Lerp(brakeInput, 1f, Time.deltaTime * brakingForce);
                    }
                    // +++
                */

                else
                    obstacle = null;

            }

        }

        //  Ray hits an obstacle or not?
        raycasting = casted;

        //  If so, clamp the ray input.
        rayInput = Mathf.Clamp(rayInput, -1f, 1f);
        /*
        //  If ray input is high enough, ignore the navigator input and directly use the ray input for steering.
        if (raycasting && Mathf.Abs(rayInput) > .5f)
            ignoreWaypointNow = true;
        else
            ignoreWaypointNow = false;
        */
        // Only ignore waypoint steering for non-car obstacles
        // ignoreWaypointNow = raycasting && Mathf.Abs(rayInput) > .5f && !obstacle.CompareTag("AICar");
        if (raycasting && Mathf.Abs(rayInput) > .5f && !obstacle.CompareTag("AICar"))
            ignoreWaypointNow = true;
        //else if (raycasting && obstacle.CompareTag("AICar") && CarController.speed <= 1f)
        //ignoreWaypointNow = true;
        else
            //Debug.Log("here");
            ignoreWaypointNow = false;

        // + + +
        // Drawing a ray box to detect pedestrians 
        // + + +
        float pedestrianLength = 7f;
        float pedestrianWidth = 6f;
        float pedestrianHeight = 2f;
        Vector3 pedestrianBoxOffset = new Vector3(0.5f, 1.5f, 1.5f);

        Vector3 boxCenter = transform.position
                    + transform.forward * (pedestrianLength / 2f) * pedestrianBoxOffset.z   // forward distance
                    + transform.up * pedestrianBoxOffset.y        // vertical offset
                    + transform.right * pedestrianBoxOffset.x;    // sideways offset

        //Vector3 boxCenter = transform.position + transform.forward * (pedestrianLength / 2f) + Vector3.up * (pedestrianHeight / 2f);
        Vector3 halfExtents = new Vector3(pedestrianWidth / 2f, pedestrianHeight / 2f, pedestrianLength / 2f);
        Quaternion rot = transform.rotation;

        // Debug draw the zone
        Vector3[] corners = new Vector3[8];
        corners[0] = boxCenter + rot * new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
        corners[1] = boxCenter + rot * new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
        corners[2] = boxCenter + rot * new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z);
        corners[3] = boxCenter + rot * new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z);

        corners[4] = boxCenter + rot * new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
        corners[5] = boxCenter + rot * new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
        corners[6] = boxCenter + rot * new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);
        corners[7] = boxCenter + rot * new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z);

        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(corners[i], corners[(i + 1) % 4], Color.blue);         // bottom
            Debug.DrawLine(corners[i + 4], corners[((i + 1) % 4) + 4], Color.blue); // top
            Debug.DrawLine(corners[i], corners[i + 4], Color.blue);                 // verticals
        }


        pedestrianDetected = false;
        // Check if a pedestrian is in the zone
        Collider[] hits = Physics.OverlapBox(boxCenter, halfExtents, rot, obstacleLayers);

        foreach (var c in hits)
        {
            if (c.CompareTag("Pedestrian"))
            {
                pedestrianDetected = true;
                break;
            }
        }

        // + + +
    }


    /// <summary>
    /// Feeding the RCC with throttle, brake, steer, and handbrake inputs.
    /// </summary>
    private void FeedRCC()
    {
        DebugSteerInputs();
        // Feeding throttleInput of the RCC.
        if (!CarController.changingGear && !CarController.cutGas)
            CarController.throttleInput = (CarController.direction == 1 ? Mathf.Clamp01(throttleInput) : Mathf.Clamp01(brakeInput));
        else
            CarController.throttleInput = 0f;

        if (!CarController.changingGear && !CarController.cutGas)
            CarController.brakeInput = (CarController.direction == 1 ? Mathf.Clamp01(brakeInput) : Mathf.Clamp01(throttleInput));
        else
            CarController.brakeInput = 0f;

        float before = CarController.steerInput;

        // Feeding steerInput of the RCC.
        if (smoothedSteer)
        {
            float after = Mathf.Lerp(before, steerInput, Time.deltaTime * 20f);
            CarController.steerInput = after;
            //Debug.Log($"[AI STEER] raw: {steerInput:F2}, " +
            //          $"prev: {before:F2}, " +
            //          $"smoothed→: {after:F2}"
            //    );

        }
        else
        {
            CarController.steerInput = steerInput;
            //Debug.Log($"[AI STEER] raw applied: {steerInput:F2}");
        }

        CarController.handbrakeInput = handbrakeInput;

    }


    private void DebugSteerInputs()
    {
        float navX = transform.InverseTransformDirection(navigator.desiredVelocity).x;
        //Debug.Log($"[AI INPUT] navX: {navX:F2}, " +
        //          $"rayInput: {rayInput:F2}, " +
        //          $"ignoreWP: {ignoreWaypointNow:F2}, " +
        //         $"computer steerInput: {steerInput:F2}"
        //        );


    }

    /// <summary>
    /// Stops the vehicle immediately.
    /// </summary>
    private void Stop()
    {

        throttleInput = 0f;
        brakeInput = 0f;
        steerInput = 0f;
        handbrakeInput = 1f;

    }

    /// <summary>
    /// Checks the near targets if navigation mode is set to follow or chase mode.
    /// </summary>
    private void CheckTargets()
    {

        if (!updateTargets)
            return;

        updateTargets = false;
        lastUpdatedTargets = 0f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectorRadius);

        for (int i = 0; i < colliders.Length; i++)
        {

            //  If a target in the zone, add it to the list.
            if (colliders[i].transform.root.CompareTag(targetTag))
            {

                if (!targetsInZone.Contains(colliders[i].transform.root))
                    targetsInZone.Add(colliders[i].transform.root);

            }

            //  If a brake zone in the zone, add it to the list.
            if (colliders[i].GetComponent<RCC_AIBrakeZone>())
            {

                if (!brakeZones.Contains(colliders[i].GetComponent<RCC_AIBrakeZone>()))
                    brakeZones.Add(colliders[i].GetComponent<RCC_AIBrakeZone>());

            }
            //  If a slow zone in the zone, add it to the list.
            if (colliders[i].GetComponent<RCC_AISlowZone>())
            {
                if (!slowZones.Contains(colliders[i].GetComponent<RCC_AISlowZone>()))
                    slowZones.Add(colliders[i].GetComponent<RCC_AISlowZone>());

            }

        }

        // Removing unnecessary targets in list first. If target is null or not active, remove it from the list.
        for (int i = 0; i < targetsInZone.Count; i++)
        {

            if (targetsInZone[i] == null)
                targetsInZone.RemoveAt(i);

            if (!targetsInZone[i].gameObject.activeInHierarchy)
                targetsInZone.RemoveAt(i);

            else
            {

                //  If distance to the target is far away, remove it from the list.
                if (Vector3.Distance(transform.position, targetsInZone[i].transform.position) > (detectorRadius * 1.1f))
                    targetsInZone.RemoveAt(i);

            }

        }

        // If there is a target in the zone, get closest enemy.
        if (targetsInZone.Count > 0)
            targetChase = GetClosestEnemy(targetsInZone.ToArray());
        else
            targetChase = null;

    }

    /// <summary>
    /// Checks the brake zones.
    /// </summary>
    private void CheckBrakeZones()
    {

        // Removing unnecessary brake zones in list. If brake zone is null or not active, remove it from the list.
        for (int i = 0; i < brakeZones.Count; i++)
        {

            if (brakeZones[i] == null)
                brakeZones.RemoveAt(i);

            if (!brakeZones[i].gameObject.activeInHierarchy)
                brakeZones.RemoveAt(i);

            else
            {

                //  If distance to the brake zone is far away, remove it from the list.
                if (Vector3.Distance(transform.position, brakeZones[i].transform.position) > (detectorRadius * 1.1f))
                    brakeZones.RemoveAt(i);

            }

        }

        // If there is a brake zone, get closest one.
        if (brakeZones.Count > 0)
            targetBrake = GetClosestBrakeZone(brakeZones.ToArray());
        else
            targetBrake = null;
    }

    private void CheckSlowZones()
    {

    // ADDED Removing unnecessary slow zones in list. If slow zone is null or not active, remove it from the list.
    for (int i = 0; i < slowZones.Count; i++)
    {

        if (slowZones[i] == null)
            slowZones.RemoveAt(i);

        if (!slowZones[i].gameObject.activeInHierarchy)
            slowZones.RemoveAt(i);

        else
        {

            //  If distance to the brake zone is far away, remove it from the list.
            if (Vector3.Distance(transform.position, slowZones[i].transform.position) > (detectorRadius * 1.1f))
                slowZones.RemoveAt(i);

        }

    }

        // ADDED If there is a slow zone, get closest one.
        if (slowZones.Count > 0)
        {
            targetSlow = GetClosestSlowZone(slowZones.ToArray());
        }
        else
            targetSlow = null;

    }

    /// <summary>
    /// Gets the closest enemy.
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    private Transform GetClosestEnemy(Transform[] enemies)
    {

        Transform bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in enemies)
        {

            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {

                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;

            }

        }

        return bestTarget;

    }

    /// <summary>
    /// Gets the closest brake zone.
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    private RCC_AIBrakeZone GetClosestBrakeZone(RCC_AIBrakeZone[] enemies)
    {

        RCC_AIBrakeZone bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (RCC_AIBrakeZone potentialTarget in enemies)
        {

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {

                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;

            }

        }

        return bestTarget;

    }

    private RCC_AISlowZone GetClosestSlowZone(RCC_AISlowZone[] enemies)
    {

        RCC_AISlowZone bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (RCC_AISlowZone potentialTarget in enemies)
        {

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {

                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;

            }

        }

        return bestTarget;

    }


    private void OnDisable()
    {

        //  Disabling external controller of the vehicle on disable.
        CarController.externalController = false;

        // Calling this event when AI vehicle is destroyed.
        if (OnRCCAIDestroyed != null)
            OnRCCAIDestroyed(this);

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrosswalkZone"))
        {
            currentCrosswalkZone = other.GetComponent<CrosswalkZone>();
            if (currentCrosswalkZone != null)
            {
                stopLineTarget = currentCrosswalkZone.stopLine;
                inCrosswalkZone = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CrosswalkZone"))
        {
            inCrosswalkZone = false;
            currentCrosswalkZone = null;
            stopLineTarget = null;
        }
    }

}












/*



//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2024 BoneCracker Games
// Buğra Özdoğanlar
//----------------------------------------------

using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// AI Controller of RCC. It's not professional, but it does the job. Follows all waypoints, or follows/chases the target gameobject.
/// </summary>
[RequireComponent(typeof(RCC_CarControllerV3))]
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/RCC AI Car Controller")]
public class RCC_AICarController : MonoBehaviour
{

    // ─────────────────────────────────────────────────────────────────────────────
    //  🔎  DEBUG SECTION
    // ─────────────────────────────────────────────────────────────────────────────
    [Header("DEBUG")]
    public bool enableDebugLogs = true;              // Inspector-Schalter
    [Range(0.1f, 10f)] public float debugPrintInterval = 1f;
    private float _debugPrintTimer;
    private const string _dbg = "<color=cyan>[RCC-AI]</color> ";

    private void DebugPrint(string msg)
    {
        if (!enableDebugLogs) return;
        Debug.Log($"{_dbg}{msg}", this);
    }
    // ─────────────────────────────────────────────────────────────────────────────

    public RCC_CarControllerV3 CarController
    {
        get
        {
            if (_carController == null)
                _carController = GetComponentInParent<RCC_CarControllerV3>();
            return _carController;
        }
    }
    private RCC_CarControllerV3 _carController;

    public RCC_AIWaypointsContainer waypointsContainer;
    public int currentWaypointIndex = 0;
    public string targetTag = "Player";

    public NavigationMode navigationMode = NavigationMode.FollowWaypoints;
    public enum NavigationMode { FollowWaypoints, ChaseTarget, FollowTarget }

    [Header("Raycast Settings")]
    [Range(5f, 30f)] public float raycastLength = 3f;
    [Range(10f, 90f)] public float raycastAngle = 30f;
    public LayerMask obstacleLayers = -1;

    public GameObject obstacle;
    public bool useRaycasts = true;
    public Vector3 rayOrigin = new Vector3(0f, .1f, 2f);

    private float rayInput = 0f;
    private bool raycasting = false;
    private float resetTime = 0f;
    private bool reversingNow = false;

    //  Inputs (als ReadOnly im Inspector)
    [Header("Inputs (Read-Only)")]
    [ReadOnlyField] public float steerInput = 0f;
    [ReadOnlyField] public float throttleInput = 0f;
    [ReadOnlyField] public float brakeInput = 0f;
    [ReadOnlyField] public float handbrakeInput = 0f;

    public bool limitSpeed = false;
    public float maximumSpeed = 100f;
    public bool smoothedSteer = true;

    [Header("Lap / Waypoint")]
    public int lap = 0;
    public bool stopAfterLap = false;
    public int stopLap = 10;
    public int totalWaypointPassed = 0;
    public bool ignoreWaypointNow = false;

    public int detectorRadius = 200;
    public int startFollowDistance = 300;
    public int stopFollowDistance = 30;

    private bool updateTargets = false;
    private float lastUpdatedTargets = 0f;
    private NavMeshAgent navigator;

    public List<Transform> targetsInZone = new List<Transform>();
    public List<RCC_AIBrakeZone> brakeZones = new List<RCC_AIBrakeZone>();

    public Transform targetChase;
    public RCC_AIBrakeZone targetBrake;

    public delegate void onRCCAISpawned(RCC_AICarController ai);
    public static event onRCCAISpawned OnRCCAISpawned;
    public delegate void onRCCAIDestroyed(RCC_AICarController ai);
    public static event onRCCAIDestroyed OnRCCAIDestroyed;

    // ─────────────────────────────────────────────────────────────────────────────
    //  A W A K E
    // ─────────────────────────────────────────────────────────────────────────────
    private void Awake()
    {

        if (!waypointsContainer)
            waypointsContainer = FindObjectOfType<RCC_AIWaypointsContainer>();

        GameObject navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.SetParent(transform, false);
        navigator = navigatorObject.AddComponent<NavMeshAgent>();
        navigator.radius = 1;
        navigator.speed = 1;
        navigator.angularSpeed = 100000f;
        navigator.acceleration = 100000f;
        navigator.height = 1;
        navigator.avoidancePriority = 0;

        DebugPrint($"Awake | WaypointsContainer: {(waypointsContainer ? "✔" : "✘")} " +
                   $"Count: {waypointsContainer?.waypoints.Count ?? 0}");
    }

    private void OnEnable()
    {
        CarController.externalController = true;
        OnRCCAISpawned?.Invoke(this);
        DebugPrint("Enabled & externalController = true");
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  U P D A T E
    // ─────────────────────────────────────────────────────────────────────────────
    private void Update()
    {

        if (!CarController.canControl) return;

        if (!limitSpeed) maximumSpeed = CarController.maxspeed;

        navigator.transform.localPosition = Vector3.zero;
        navigator.transform.localPosition += Vector3.forward *
                                             CarController.FrontLeftWheelCollider.transform.localPosition.z;

        CheckTargets();
        CheckBrakeZones();

        if (!updateTargets) lastUpdatedTargets += Time.deltaTime;
        if (lastUpdatedTargets >= 1f) updateTargets = true;

        if (enableDebugLogs)
        {
            _debugPrintTimer += Time.deltaTime;
            if (_debugPrintTimer >= debugPrintInterval)
            {
                _debugPrintTimer = 0f;
                DebugPrint($"Mode={navigationMode} | OnNavMesh={navigator.isOnNavMesh} | " +
                           $"desiredVel={navigator.desiredVelocity} | " +
                           $"Steer={steerInput:F2} Thr={throttleInput:F2} Brk={brakeInput:F2} " +
                           $"SPD={CarController.speed:F1}");
                if (waypointsContainer)
                    DebugPrint($"WpIdx={currentWaypointIndex}/{waypointsContainer.waypoints.Count - 1} " +
                               $"Lap={lap} Passed={totalWaypointPassed}");
            }
        }
    }

    private void FixedUpdate()
    {

        if (!CarController.canControl) return;

        if (useRaycasts) FixedRaycasts();
        Navigation();
        CheckReset();
        FeedRCC();
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  N A V I G A T I O N
    // ─────────────────────────────────────────────────────────────────────────────
    private void Navigation()
    {

        float navigatorInput = Mathf.Clamp(
            transform.InverseTransformDirection(navigator.desiredVelocity).x,
            -1f, 1f);

        if (navigatorInput > .4f) navigatorInput = 1f;
        if (navigatorInput < -.4f) navigatorInput = -1f;

        switch (navigationMode)
        {

            case NavigationMode.FollowWaypoints:

                if (!waypointsContainer) { DebugPrint("❌ Waypoints Container fehlt!"); Stop(); return; }
                if (waypointsContainer.waypoints.Count < 1) { DebugPrint("❌ Waypoints-Liste leer!"); Stop(); return; }
                if (stopAfterLap && lap >= stopLap) { Stop(); return; }

                RCC_Waypoint currentWaypoint = waypointsContainer.waypoints[currentWaypointIndex];
                float distanceToNextWaypoint = Vector3.Distance(transform.position, currentWaypoint.transform.position);

                if (navigator.isOnNavMesh)
                {
                    navigator.SetDestination(currentWaypoint.transform.position);
                    DebugPrint($"SetDestination → WP[{currentWaypointIndex}] Dist={distanceToNextWaypoint:F1}");
                }

                if (distanceToNextWaypoint != 0 && distanceToNextWaypoint < currentWaypoint.radius)
                {

                    currentWaypointIndex++;
                    totalWaypointPassed++;

                    if (currentWaypointIndex >= waypointsContainer.waypoints.Count)
                    {
                        currentWaypointIndex = 0;
                        lap++;
                        DebugPrint($"Lap++ → {lap}");
                    }

                    if (navigator.isOnNavMesh)
                        navigator.SetDestination(waypointsContainer.waypoints[currentWaypointIndex].transform.position);
                }

                if (!reversingNow)
                {

                    throttleInput = (distanceToNextWaypoint < currentWaypoint.radius * (CarController.speed / 30f))
                        ? Mathf.Clamp01(currentWaypoint.targetSpeed - CarController.speed) : 1f;

                    throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, CarController.speed / maximumSpeed));
                    brakeInput = (distanceToNextWaypoint < currentWaypoint.radius * (CarController.speed / 30f))
                        ? Mathf.Clamp01(CarController.speed - currentWaypoint.targetSpeed) : 0f;

                    handbrakeInput = 0f;

                    if (CarController.speed > 30f)
                    {
                        throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                        brakeInput += Mathf.Abs(navigatorInput) / 3f;
                    }
                }
                break;

            case NavigationMode.ChaseTarget:

                if (!targetChase) { Stop(); return; }
                if (navigator.isOnNavMesh) navigator.SetDestination(targetChase.position);

                if (!reversingNow)
                {
                    throttleInput = 1f * Mathf.Clamp01(Mathf.Lerp(10f, 0f, CarController.speed / maximumSpeed));
                    brakeInput = 0f; handbrakeInput = 0f;

                    if (CarController.speed > 30f)
                    {
                        throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                        brakeInput += Mathf.Abs(navigatorInput) / 3f;
                    }
                }
                break;

            case NavigationMode.FollowTarget:

                if (!targetChase) { Stop(); return; }
                if (navigator.isOnNavMesh) navigator.SetDestination(targetChase.position);

                float distanceToTarget = Vector3.Distance(transform.position, targetChase.position);

                if (!reversingNow)
                {

                    throttleInput = distanceToTarget < stopFollowDistance * Mathf.Lerp(1f, 5f, CarController.speed / 50f)
                        ? Mathf.Lerp(-5f, 1f, distanceToTarget / (stopFollowDistance / 1f)) : 1f;

                    throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, CarController.speed / maximumSpeed));
                    brakeInput = distanceToTarget < stopFollowDistance * Mathf.Lerp(1f, 5f, CarController.speed / 50f)
                        ? Mathf.Lerp(5f, 0f, distanceToTarget / (stopFollowDistance / 1f)) : 0f;

                    handbrakeInput = 0f;

                    if (CarController.speed > 30f)
                    {
                        throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                        brakeInput += Mathf.Abs(navigatorInput) / 3f;
                    }

                    if (throttleInput < .05f) throttleInput = 0f;
                    if (brakeInput < .05f) brakeInput = 0f;
                }
                break;
        }

        if (targetBrake && Vector3.Distance(transform.position, targetBrake.transform.position) < targetBrake.distance
                        && CarController.speed > targetBrake.targetSpeed)
        {
            throttleInput = 0f; brakeInput = 1f;
        }

        if (brakeInput > .25f) throttleInput = 0f;

        steerInput = (ignoreWaypointNow ? rayInput : navigatorInput + rayInput);
        steerInput = Mathf.Clamp(steerInput, -1f, 1f) * CarController.direction;

        throttleInput = Mathf.Clamp01(throttleInput);
        brakeInput = Mathf.Clamp01(brakeInput);
        handbrakeInput = Mathf.Clamp01(handbrakeInput);

        if (reversingNow)
        {
            throttleInput = 0f; brakeInput = 1f; handbrakeInput = 0f;
        }
        else if (CarController.speed < 5f && brakeInput >= .5f)
        {
            brakeInput = 0f; handbrakeInput = 1f;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  R E S T   (Reset-Check, Raycasts, FeedRCC, Targets, Gizmos …)
    // ─────────────────────────────────────────────────────────────────────────────
    private void CheckReset()
    {
        if (targetChase && navigationMode == NavigationMode.FollowTarget &&
            Vector3.Distance(transform.position, targetChase.position) < stopFollowDistance)
        {
            reversingNow = false; resetTime = 0; return;
        }

        if (CarController.speed <= 5 &&
            transform.InverseTransformDirection(CarController.Rigid.velocity).z <= 1f)
            resetTime += Time.deltaTime;

        if (resetTime >= 2) reversingNow = true;
        if (resetTime >= 4 || CarController.speed >= 25) { reversingNow = false; resetTime = 0; }
    }

    private void FixedRaycasts()
    {
        int[] angles = { 0, (int)(raycastAngle/3f), (int)raycastAngle,
                        -(int)raycastAngle, -(int)(raycastAngle/3f) };

        Vector3 pivot = transform.position + transform.TransformVector(rayOrigin);
        RaycastHit hit; rayInput = 0f; bool casted = false;

        foreach (int a in angles)
        {
            Vector3 dir = Quaternion.AngleAxis(a, transform.up) * transform.forward;
            Debug.DrawRay(pivot, dir * raycastLength, Color.white);

            if (Physics.Raycast(pivot, dir, out hit, raycastLength, obstacleLayers) &&
                !hit.collider.isTrigger && hit.transform.root != transform)
            {

                Debug.DrawRay(pivot, dir * raycastLength, Color.red);
                casted = true;
                if (a != 0)
                    rayInput -= Mathf.Lerp(Mathf.Sign(a), 0f, hit.distance / raycastLength);
                obstacle = hit.transform.gameObject;
            }
        }

        raycasting = casted;
        rayInput = Mathf.Clamp(rayInput, -1f, 1f);
        ignoreWaypointNow = raycasting && Mathf.Abs(rayInput) > .5f;
    }

    private void FeedRCC()
    {

        CarController.throttleInput = (!CarController.changingGear && !CarController.cutGas)
            ? (CarController.direction == 1 ? throttleInput : brakeInput) : 0f;

        CarController.brakeInput = (!CarController.changingGear && !CarController.cutGas)
            ? (CarController.direction == 1 ? brakeInput : throttleInput) : 0f;

        CarController.steerInput = smoothedSteer
            ? Mathf.Lerp(CarController.steerInput, steerInput, Time.deltaTime * 20f)
            : steerInput;

        CarController.handbrakeInput = handbrakeInput;
    }

    private void Stop()
    {
        throttleInput = brakeInput = steerInput = 0f;
        handbrakeInput = 1f;
    }

    private void CheckTargets()
    {

        if (!updateTargets) return;
        updateTargets = false; lastUpdatedTargets = 0f;

        Collider[] cols = Physics.OverlapSphere(transform.position, detectorRadius);
        foreach (var c in cols)
        {
            if (c.transform.root.CompareTag(targetTag) &&
                !targetsInZone.Contains(c.transform.root)) targetsInZone.Add(c.transform.root);
            var bz = c.GetComponent<RCC_AIBrakeZone>();
            if (bz && !brakeZones.Contains(bz)) brakeZones.Add(bz);
        }

        targetsInZone.RemoveAll(t => t == null || !t.gameObject.activeInHierarchy ||
                                     Vector3.Distance(transform.position, t.position) > detectorRadius * 1.1f);
        brakeZones.RemoveAll(bz => bz == null || !bz.gameObject.activeInHierarchy ||
                                   Vector3.Distance(transform.position, bz.transform.position) > detectorRadius * 1.1f);

        targetChase = targetsInZone.Count > 0 ? GetClosestEnemy(targetsInZone.ToArray()) : null;
    }

    private void CheckBrakeZones() =>
        targetBrake = brakeZones.Count > 0 ? GetClosestBrakeZone(brakeZones.ToArray()) : null;

    private Transform GetClosestEnemy(Transform[] arr)
    {
        Transform best = null; float bestSqr = float.PositiveInfinity;
        Vector3 pos = transform.position;
        foreach (var t in arr)
        {
            float sqr = (t.position - pos).sqrMagnitude;
            if (sqr < bestSqr) { bestSqr = sqr; best = t; }
        }
        return best;
    }

    private RCC_AIBrakeZone GetClosestBrakeZone(RCC_AIBrakeZone[] arr)
    {
        RCC_AIBrakeZone best = null; float bestSqr = float.PositiveInfinity;
        Vector3 pos = transform.position;
        foreach (var t in arr)
        {
            float sqr = (t.transform.position - pos).sqrMagnitude;
            if (sqr < bestSqr) { bestSqr = sqr; best = t; }
        }
        return best;
    }

    private void OnDisable()
    {
        CarController.externalController = false;
        OnRCCAIDestroyed?.Invoke(this);
        DebugPrint("Disabled & externalController = false");
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!enableDebugLogs) return;

        if (waypointsContainer && waypointsContainer.waypoints.Count > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                waypointsContainer.waypoints[currentWaypointIndex].transform.position);
            Gizmos.DrawSphere(
                waypointsContainer.waypoints[currentWaypointIndex].transform.position, 0.5f);
        }
        if (navigator)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(navigator.transform.position, navigator.radius);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(rayOrigin), .15f);
    }
#endif
}

// ─────────────────────────────────────────────────────────────────────────────
//  R e a d O n l y F i e l d   A t t r i b u t e   +   D r a w e r
// ─────────────────────────────────────────────────────────────────────────────
public class ReadOnlyFieldAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
public class ReadOnlyFieldDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
        EditorGUI.GetPropertyHeight(property, label, true);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

*/