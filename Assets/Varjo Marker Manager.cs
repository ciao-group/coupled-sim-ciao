using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

/// <summary>
/// Marker-based alignment for an RCC car
/// 
/// Usage:
/// 1) Attach this script to the same GameObject that has RCC_CarControllerV3 + Rigidbody.
/// 2) Provide marker IDs & offsets in 'trackedObjects'.
/// </summary>
public class VarjoMarkerManager : MonoBehaviour
{
    [System.Serializable]
    public struct TrackedObject
    {
        public long[] ids;                  // Marker IDs
        public Vector3[] positionOffsets;   // Position offset per marker ID
        public Vector3[] rotationOffsets;   // Euler offset per marker ID
        public Vector3[] scaleOffsets;      // Scale offset per marker ID
    }

    [Header("RCC & Markers")]
    [Tooltip("The RCC Car Controller for your vehicle.")]
    public RCC_CarControllerV3 rccCar;

    [Tooltip("Marker configurations for alignment during calibration.")]
    public TrackedObject[] trackedObjects;

    [Header("Calibration Settings")]
    [Tooltip("Seconds to keep the car kinematic, aligning to markers.")]
    public float calibrationDuration = 10f;

    [Tooltip("Lerp/slerp speed while aligning to the marker.")]
    public float alignmentLerpSpeed = 5f;

    // Internal calibration state
    private float calibrationTimer = 0f;
    private bool isCalibrating = false;

    // For marker detection
    private List<VarjoMarker> currentMarkers = new List<VarjoMarker>();
    private List<long> removedMarkers = new List<long>();

    private void OnEnable()
    {
        // Enable Varjo marker tracking
        VarjoMarkers.EnableVarjoMarkers(true);

        // (Optional) Start calibration automatically
       StartCalibration();
    }

    private void OnDisable()
    {
        // Disable Varjo marker tracking
        VarjoMarkers.EnableVarjoMarkers(false);
    }

    /// <summary>
    /// Begins the marker-based calibration by setting the car rigidbody to kinematic
    /// and resetting the calibration timer.
    /// </summary>

    public void StartCalibration()
    {
        if (rccCar && rccCar.Rigid)
            rccCar.Rigid.isKinematic = true;

        calibrationTimer = calibrationDuration;
        isCalibrating = true;
    }

    /// <summary>
    /// Ends the calibration:
    ///   - Re-enables RCC physics,
    ///   - Re-parents the camera so it doesn't teleport in world space
    ///     (stays exactly where it is if the car hasn't moved).
    /// </summary>
    public void EndCalibration()
    {
        isCalibrating = false;

        // Re-enable normal RCC physics
        if (rccCar && rccCar.Rigid)
            rccCar.Rigid.isKinematic = false;
        
        // (Optional) If no more marker alignment is needed:
        // this.enabled = false;
    }

    private void Update()
    {
        
        if (!isCalibrating)
            return;

        calibrationTimer -= Time.deltaTime;
        if (calibrationTimer <= 0f)
        {
            EndCalibration();
            return;
        }
        
        AlignCarToMarkers();
    }

    /// <summary>
    /// During calibration, smoothly align the car to the marker's pose.
    /// </summary>
    private void AlignCarToMarkers()
    {
        if (!VarjoMarkers.IsVarjoMarkersEnabled())
            return;

        VarjoMarkers.GetVarjoMarkers(out currentMarkers);
        VarjoMarkers.GetRemovedVarjoMarkerIds(out removedMarkers);

        // For each configured marker group
        foreach (TrackedObject tracked in trackedObjects)
        {
            // For each detected marker
            foreach (VarjoMarker marker in currentMarkers)
            {
                int idx = System.Array.IndexOf(tracked.ids, marker.id);
                if (idx < 0)
                    continue;  // not found

                // Found matching marker
                Vector3 realPos = marker.pose.position;
                Quaternion realRot = marker.pose.rotation;

                // Offsets
                Vector3 desiredPos = realPos + tracked.positionOffsets[idx];
                Quaternion desiredRot = realRot * Quaternion.Euler(tracked.rotationOffsets[idx]);
                Vector3 desiredScale = tracked.scaleOffsets[idx];

                // Lerp this transform (the RCC object)
                transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * alignmentLerpSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * alignmentLerpSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * alignmentLerpSpeed);

                // Only use first valid marker
                break;
            }
        }
    }
}
