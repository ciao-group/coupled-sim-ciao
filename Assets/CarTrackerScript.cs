using UnityEngine;
using static EdyCommonTools.RotationController;

/// <summary>
/// Three-phase calibration with a 'cameraHolder' parent object for the actual camera:
/// 1) Phase1 (phase1Duration): Car is kinematic, cameraHolder not parented to the car,
///    forcibly align carTrackerPivot to the real tracker from cameraHolder's vantage.
/// 2) Phase2 (phase2Duration): Re-parent cameraHolder under carRoot, forcibly setting its
///    local position/rotation to (0.56,0.02,2.55) / (0,90,0). Still kinematic, forced alignment,
///    offset tweaks remain possible.
/// 3) Phase3: Turn off alignment, set car to normal physics so user can drive. 
/// 
/// Each second, logs distances for debugging. 
/// 
/// Setup:
/// - carRoot: transform with RCC_CarControllerV3 + Rigidbody
/// - carTrackerPivot: child transform for real tracker's mount
/// - cameraHolder: parent object for your VR camera
/// - realTracker: HTC tracker transform
/// - offsetPos/offsetRot: user offsets
/// 
/// During Phase1 & Phase2, the script forcibly aligns the car pivot to the real tracker
/// from the cameraHolder vantage. Once Phase3 starts, no more alignment, RCC physics is on.
/// </summary>
public class CarTrackerThreePhaseCalibratorCameraHolder : MonoBehaviour
{
    // --- Enum for phases ---
    public enum CalibrationPhase
    {
        Phase1_Adjustment,   // Car kinematic, cameraHolder not parented, offset tweaks
        Phase2_CameraParent, // Car still kinematic, cameraHolder parented, offset tweaks
        Phase3_Done          // Car normal physics, no forced alignment
    }

    public ExperimentConfigs configs;
    //public Vector3 posit;

    [Header("Car & RCC")]
    [Tooltip("RCC Car Controller for your simulated vehicle.")]
    public RCC_CarControllerV3 rccCar;

    [Tooltip("Root transform of the car (the same object that has RCC_CarControllerV3 + Rigidbody).")]
    public Transform carRoot;

    [Tooltip("A child transform inside carRoot that corresponds to the real tracker's mount point.")]
    public Transform carTrackerPivot;

    [Header("Camera Holder & Real Tracker")]
    [Tooltip("A parent object holding your actual VR camera as a child.")]
    public Transform cameraHolder;

    [Tooltip("The transform for the real HTC tracker on the physical car.")]
    public Transform realTracker;

    [Header("Offsets for Mounting")]
    [Tooltip("Position offset in local space for how the real tracker is physically placed.")]
    public Vector3 offsetPos;

    [Tooltip("Rotation offset (Euler angles) for the tracker's orientation on the car.")]
    public Vector3 offsetRot;

    [Header("Phase Durations")]
    [Tooltip("Phase1: car is kinematic, cameraHolder not parented, you can tweak offsets.")]
    public float phase1Duration = 10f;

    [Tooltip("Phase2: re-parent cameraHolder, still kinematic, forced alignment continues, can tweak offsets.")]
    public float phase2Duration = 10f;

    [Header("Alignment Settings")]
    [Tooltip("Lerp speed for smoothing alignment during phases 1 & 2.")]
    public float alignmentLerpSpeed = 5f;

    [Tooltip("If true, logs debug info each second about distances / alignment.")]
    public bool debugEverySecond = true;

    // Internals
    private CalibrationPhase currentPhase = CalibrationPhase.Phase1_Adjustment;
    private float phaseTimer = 0f;

    // For debug logs
    private float debugTimer = 0f;
    private float debugInterval = 1f;  // log each second

    private void OnEnable()
    {
        //posit = rccCar.transform.position;
        StartPhase1();
    }

    // ================== PHASE 1 START ==================
    private void StartPhase1()
    {
        currentPhase = CalibrationPhase.Phase1_Adjustment;
        phaseTimer = phase1Duration;
        debugTimer = 0f;

        // Car is kinematic
        if (rccCar && rccCar.Rigid)
            rccCar.Rigid.isKinematic = true;

        // If cameraHolder is parented to carRoot, un-parent it
        if (cameraHolder && carRoot && cameraHolder.parent == carRoot)
        {
            cameraHolder.SetParent(null, true); // keep world position
            Debug.Log("[Phase1] Un-parented cameraHolder from carRoot for initial calibration.");
        }

        Debug.Log("[Phase1] Car is kinematic, offset tweaks allowed, cameraHolder not parented.");
    }

    // ================== PHASE 2 START ==================
    private void StartPhase2()
    {
        currentPhase = CalibrationPhase.Phase2_CameraParent;
        phaseTimer = phase2Duration;
        debugTimer = 0f;

        // Still kinematic
        if (rccCar && rccCar.Rigid)
            rccCar.Rigid.isKinematic = true;

        // Re-parent cameraHolder with specified local position/rotation
        ParentCameraHolderWithFixedLocal();

        Debug.Log("[Phase2] Car still kinematic, cameraHolder now parented, offset tweaks remain allowed.");
    }

    // ================== PHASE 3 START ==================
    private void StartPhase3()
    {
        currentPhase = CalibrationPhase.Phase3_Done;
        debugTimer = 0f;
        rccCar.transform.position = configs.startPosition;
        rccCar.transform.rotation = configs.startRotation;
        // Car normal physics
        if (rccCar && rccCar.Rigid)
            rccCar.Rigid.isKinematic = false;



        Debug.Log("[Phase3] Car physics ON, no more forced alignment. Calibration done!");
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case CalibrationPhase.Phase1_Adjustment:
                Phase1Logic();
                break;

            case CalibrationPhase.Phase2_CameraParent:
                Phase2Logic();
                break;

            case CalibrationPhase.Phase3_Done:
                // No forced alignment
                break;
        }
    }

    // ================== PHASE 1 LOGIC ==================
    private void Phase1Logic()
    {
        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
        {
            // go to phase2
            StartPhase2();
            return;
        }

        // Forced alignment
        //AlignCarToTrackerOffset();

        // Debug logs each second
        if (debugEverySecond)
        {
            debugTimer += Time.deltaTime;
            if (debugTimer >= debugInterval)
            {
                debugTimer = 0f;
                PrintDebugInfo("Phase1");
            }
        }
    }

    // ================== PHASE 2 LOGIC ==================
    private void Phase2Logic()
    {
        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
        {
            // go to phase3
            StartPhase3();
            return;
        }

        // We STILL do forced alignment
        AlignCarToTrackerOffset();

        // Debug logs each second
        if (debugEverySecond)
        {
            debugTimer += Time.deltaTime;
            if (debugTimer >= debugInterval)
            {
                debugTimer = 0f;
                PrintDebugInfo("Phase2");
            }
        }
    }

    // ================== ALIGN CAR  ==================
    private void AlignCarToTrackerOffset()
    {
        if (!carRoot || !carTrackerPivot || !cameraHolder || !realTracker)
            return;

        // 1) Real offset in position
        Vector3 realPosDiff = realTracker.position - cameraHolder.position;
        //Debug.Log(realTracker.position);
        // 2) Real offset in rotation
        Quaternion realRotDiff = realTracker.rotation * Quaternion.Inverse(cameraHolder.rotation);

        // 3) user offset
        Quaternion offsetQ = Quaternion.Euler(offsetRot);
        Vector3 offsetPosGlobal = offsetQ * offsetPos;

        // 4) desired pivot in world
        Vector3 desiredPivotPos = cameraHolder.position + realPosDiff + offsetPosGlobal;
        Quaternion desiredPivotRot = (cameraHolder.rotation * realRotDiff) * offsetQ;

        // 5) move car so pivot is at desiredPivotPos/desiredPivotRot
        MoveRootSoChildMatches(carRoot, carTrackerPivot, desiredPivotPos, desiredPivotRot, alignmentLerpSpeed);
    }

    /// <summary>
    /// Moves/rotates 'root' so 'childPivot' is at (desiredPos, desiredRot) in world space, with smoothing.
    /// </summary>
    private void MoveRootSoChildMatches(
        Transform root, Transform childPivot,
        Vector3 desiredPos, Quaternion desiredRot,
        float lerpSpeed
    )
    {
        Quaternion pivotRotNow = childPivot.rotation;
        Quaternion rootRotNow = root.rotation;

        // We want pivotRotNow -> desiredRot => newRootRot = desiredRot * Inverse(pivotRotNow)
        Quaternion targetRootRot = desiredRot * Quaternion.Inverse(pivotRotNow);

        // slerp rotation
        Quaternion finalRootRot = Quaternion.Slerp(rootRotNow, targetRootRot, Time.deltaTime * lerpSpeed);
        root.rotation = finalRootRot;

        // after rotation, pivot pos changes
        Vector3 pivotPosAfterRot = childPivot.position;
        Vector3 moveDelta = desiredPos - pivotPosAfterRot;

        Vector3 rootPosNow = root.position;
        Vector3 targetRootPos = rootPosNow + moveDelta;
        Vector3 finalRootPos = Vector3.Lerp(rootPosNow, targetRootPos, Time.deltaTime * lerpSpeed);

        root.position = finalRootPos;
    }

    // ================== PARENT CAMERA HOLDER WITH FIXED OFFSET ==================
    /// <summary>
    /// Phase2: re-parent cameraHolder under carRoot with local pos=(0.56,0.02,2.55) & local rot=(0,90,0).
    /// No interpolation, forcibly sets them. Logs info about the re-parenting.
    /// </summary>
    private void ParentCameraHolderWithFixedLocal()
    {
        if (!carRoot || !cameraHolder)
            return;

        // old world
        Vector3 oldPos = cameraHolder.position;
        Quaternion oldRot = cameraHolder.rotation;

        // parent
        cameraHolder.SetParent(carRoot, false);

        // forcibly set local pos/rot
        //cameraHolder.localPosition = new Vector3(0.426f, 0f, 2.67f);
        //cameraHolder.localEulerAngles = new Vector3(0f, 85.5f, 0f);
        //cameraHolder.localPosition = new Vector3(-1.63f, 0.025f, 1.53f);
        //cameraHolder.localPosition = new Vector3(-1.79f, 0.026f, 1.374f);
        cameraHolder.localPosition = new Vector3(-1.26f, 0.02f, 1.411f);
        //cameraHolder.localPosition = new Vector3(-1.774f, 0.023f, 1.368f);
        //cameraHolder.localEulerAngles = new Vector3(0f, 16.54f, 0f); 
        //cameraHolder.localEulerAngles = new Vector3(0f, 21.451f, 0f);
        cameraHolder.localEulerAngles = new Vector3(1.7f, 35.27f, 0f);
        //cameraHolder.localEulerAngles = new Vector3(0f, 0f, 0f);

        // new world
        Vector3 newPos = cameraHolder.position;
        Quaternion newRot = cameraHolder.rotation;

        float dist = Vector3.Distance(oldPos, newPos);
        float angleDiff = Quaternion.Angle(oldRot, newRot);

        Debug.Log($"[ParentCameraHolderWithFixedLocal] " +
                  $"OldPos={oldPos}, NewPos={newPos}, Dist={dist:F4}, " +
                  $"OldRot={oldRot.eulerAngles}, NewRot={newRot.eulerAngles}, " +
                  $"AngleDiff={angleDiff:F2}");
    }

    // ================== DEBUG DISTANCES ==================
    private void PrintDebugInfo(string phaseLabel)
    {
        if (!cameraHolder || !realTracker || !carTrackerPivot)
            return;

        float distCamToReal = Vector3.Distance(cameraHolder.position, realTracker.position);
        float distCamToPivot = Vector3.Distance(cameraHolder.position, carTrackerPivot.position);
        float distRealToPivot = Vector3.Distance(realTracker.position, carTrackerPivot.position);

        float maxDist = 1f;
        float alignValue = Mathf.Clamp01(1f - distRealToPivot / maxDist);
        float alignPercent = alignValue * 100f;

        Debug.Log(
            $"[{phaseLabel} Debug] " +
            $"Cam->Real:{distCamToReal:F3}, " +
            $"Cam->VirtPivot:{distCamToPivot:F3}, " +
            $"Real->VirtPivot:{distRealToPivot:F3}, " +
            $"Align~{alignPercent:F1}%"
        );
    }
}