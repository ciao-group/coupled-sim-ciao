using UnityEngine;

/// <summary>
/// Lets you calibrate the offset between the real-world tracker and your in-game car,
/// by moving the car while the camera (HMD) stays fixed in the world.
/// 
/// 1) Car is kinematic during calibration, so we can forcibly move it in the scene.
/// 2) We measure the distance between the real tracker and the user's HMD (camera).
/// 3) We apply user-defined offsets (position + rotation) that represent how the tracker is physically mounted.
/// 4) We move the 'CarRoot' so that 'carTrackerPivot' lines up with the real tracker's position/rotation 
///    from the perspective of the camera.
/// 5) The camera is not moved; it stays in place as you tweak offsets. 
/// 6) After calibration, we re-enable RCC physics, so the car can drive normally.
/// 
/// Attach this script to any convenient object (often the same CarRoot).
/// Make sure to reference CarRoot, carTrackerPivot, hmdCamera, realTracker, and the RCC_CarControllerV3.
/// </summary>
public class CarTrackerOffsetCalibrator : MonoBehaviour
{
    [Header("Car & RCC")]
    [Tooltip("RCC Car Controller for your simulated vehicle.")]
    public RCC_CarControllerV3 rccCar;

    [Tooltip("Root transform of the car (the same object that has RCC_CarControllerV3 + Rigidbody).")]
    public Transform carRoot;

    [Tooltip("A child transform of the carRoot that corresponds to where the real tracker is physically mounted.")]
    public Transform carTrackerPivot;

    [Header("Camera & Real Tracker")]
    [Tooltip("The VR/AR HMD camera transform. (Not a child of carRoot during calibration.)")]
    public Transform hmdCamera;

    [Tooltip("The transform updated by the real HTC tracker on the car (hood, etc.).")]
    public Transform realTracker;

    [Header("User Adjustable Offsets")]
    [Tooltip("Position offset from the real tracker's anchor to your 3D car pivot, in local space. " +
             "You can tweak these in the Inspector until alignment is correct.")]
    public Vector3 offsetPos;

    [Tooltip("Rotation offset (Euler angles) for how the real tracker is oriented on the car. " +
             "X = pitch, Y = yaw, Z = roll. Tweak until alignment is correct.")]
    public Vector3 offsetRot;

    [Header("Calibration Settings")]
    [Tooltip("How many seconds to keep the car kinematic, letting you adjust offsets and forcibly align the car.")]
    public float calibrationDuration = 10f;

    [Tooltip("Speed for smoothing position/rotation updates during calibration.")]
    public float alignmentLerpSpeed = 5f;

    // Internal
    private bool isCalibrating = false;
    private float calibrationTimer = 0f;

    private void OnEnable()
    {
        // Optionally start calibration immediately
        StartCalibration();
    }

    /// <summary>
    /// Begins the calibration phase:
    ///  - Set the car to kinematic
    ///  - Reset the timer
    ///  
    /// During calibration, you can tweak offsetPos/offsetRot in the Inspector,
    /// and watch the car pivot line up with the real tracker from the user's stationary camera vantage.
    /// </summary>
    public void StartCalibration()
    {
        if (rccCar != null && rccCar.Rigid != null)
        {
            rccCar.Rigid.isKinematic = true;
        }

        calibrationTimer = calibrationDuration;
        isCalibrating = true;
    }

    /// <summary>
    /// Ends the calibration:
    ///  - Car is no longer forcibly moved
    ///  - Re-enable RCC physics so user can drive
    ///  - The final offset remains as you set it
    /// </summary>
    public void EndCalibration()
    {
        isCalibrating = false;

        if (rccCar != null && rccCar.Rigid != null)
        {
            rccCar.Rigid.isKinematic = false;
        }
    }

    private void Update()
    {
        if (!isCalibrating)
            return;

        // Count down
        calibrationTimer -= Time.deltaTime;
        if (calibrationTimer <= 0f)
        {
            EndCalibration();
            return;
        }

        // Each frame, forcibly move carRoot so carTrackerPivot lines up with real tracker from the HMD perspective.
        AlignCarToTrackerOffset();
    }

    /// <summary>
    /// Called each frame during calibration:
    /// 1) Calculate the real offset between hmdCamera and realTracker
    /// 2) Apply user offsets (offsetPos, offsetRot)
    /// 3) Move carRoot so carTrackerPivot in the game ends up exactly at that position/rotation in world space.
    /// 4) Camera remains stationary in the scene, so you can see the alignment.
    /// </summary>
    private void AlignCarToTrackerOffset()
    {
        if (!carRoot || !carTrackerPivot || !hmdCamera || !realTracker)
            return;

        // A) Real offset in position:
        //    how far is realTracker from the camera in the real world?
        Vector3 realPosDiff = realTracker.position - hmdCamera.position;

        // B) Real offset in rotation:
        //    if you face the camera as 'reference', how do we rotate from camera to tracker?
        //    realRotDiff = realTracker.rotation * Inverse(hmdCamera.rotation).
        Quaternion realRotDiff = realTracker.rotation * Quaternion.Inverse(hmdCamera.rotation);

        // C) Now apply the user offsets for how the tracker is physically oriented on the car:
        Quaternion offsetQ = Quaternion.Euler(offsetRot);
        Vector3 offsetPosGlobal = offsetQ * offsetPos;
        // By rotating offsetPos with offsetQ, we ensure position offset is oriented by that rotation.

        // So the final desired pivot position in world space:
        // pivot should be at camera's position + realPosDiff + offsetPosGlobal
        // (You can tune the sign if you want. If you notice you need negative, just invert offsetPos.)
        Vector3 desiredPivotPos = hmdCamera.position + realPosDiff + offsetPosGlobal;

        // The final pivot rotation is camera.rotation * realRotDiff * offsetQ 
        //   (the order can vary, but typically we do realRotDiff first, then offsetQ).
        // Let's do: desiredPivotRot = (camera.rotation * realRotDiff) * offsetQ
        Quaternion desiredPivotRot = (hmdCamera.rotation * realRotDiff) * offsetQ;

        // D) forcibly move/rotate carRoot so that "carTrackerPivot" is at (desiredPivotPos, desiredPivotRot).
        MoveRootSoChildMatches(carRoot, carTrackerPivot, desiredPivotPos, desiredPivotRot, alignmentLerpSpeed);
    }

    /// <summary>
    /// Moves/rotates 'root' so that 'childPivot' in world space becomes (desiredPos, desiredRot),
    /// using smooth interpolation (lerp/slerp). 
    /// The hmdCamera remains stationary in world, so visually the car slides around until it aligns.
    /// </summary>
    private void MoveRootSoChildMatches(
        Transform root, Transform childPivot,
        Vector3 desiredPos, Quaternion desiredRot,
        float lerpSpeed
    )
    {
        // 1) pivot's current world rotation
        Quaternion pivotRotNow = childPivot.rotation;

        // 2) we want pivotRotNow -> desiredRot
        // => root.rotation -> desiredRot * Inverse(pivotRotNow)
        Quaternion rootRotNow = root.rotation;
        Quaternion targetRootRot = desiredRot * Quaternion.Inverse(pivotRotNow);

        // Slerp rotation
        Quaternion finalRootRot = Quaternion.Slerp(
            rootRotNow,
            targetRootRot,
            Time.deltaTime * lerpSpeed
        );
        root.rotation = finalRootRot;

        // After rotating, pivot pos changes
        Vector3 pivotPosAfterRot = childPivot.position;

        // 3) we want pivotPosAfterRot -> desiredPos
        Vector3 moveDelta = desiredPos - pivotPosAfterRot;

        // Lerp position
        Vector3 rootPosNow = root.position;
        Vector3 targetRootPos = rootPosNow + moveDelta;
        Vector3 finalRootPos = Vector3.Lerp(
            rootPosNow,
            targetRootPos,
            Time.deltaTime * lerpSpeed
        );

        root.position = finalRootPos;
    }
}