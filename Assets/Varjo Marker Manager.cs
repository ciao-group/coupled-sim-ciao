using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

public class VarjoMarkerManager : MonoBehaviour
{
    [System.Serializable]
    public struct TrackedObject
    {
        public long[] ids;
        public GameObject gameObject;
        public bool dynamicTracking;
        public Vector3[] positionOffsets;
        public Vector3[] rotationOffsets;
        public Vector3[] scaleOffsets;
    }

    public TrackedObject[] trackedObjects;
    private List<VarjoMarker> markers = new List<VarjoMarker>();
    private List<long> removedMarkerIds = new List<long>();
    public float transitionSpeed = 0.1f;
    public long adjustableMarkerId;

    private void OnEnable()
    {
        VarjoMarkers.EnableVarjoMarkers(true);
    }

    private void OnDisable()
    {
        VarjoMarkers.EnableVarjoMarkers(false);
    }

    void Update()
    {
        if (VarjoMarkers.IsVarjoMarkersEnabled())
        {
            VarjoMarkers.GetVarjoMarkers(out markers);
            VarjoMarkers.GetRemovedVarjoMarkerIds(out removedMarkerIds);
            foreach (var tracked in trackedObjects)
            {
                foreach (var marker in markers)
                {
                    int index = System.Array.IndexOf(tracked.ids, marker.id);
                    if (index != -1)
                    {
                        Vector3 adjustedPosition = marker.pose.position + tracked.positionOffsets[index];
                        Quaternion adjustedRotation = ApplyCustomRotation(marker.pose.rotation, tracked.rotationOffsets[index]);
                        Vector3 adjustedScale = tracked.scaleOffsets[index];

                        tracked.gameObject.SetActive(true);
                        tracked.gameObject.transform.localPosition = Vector3.Lerp(tracked.gameObject.transform.localPosition, adjustedPosition, Time.deltaTime * transitionSpeed);
                        tracked.gameObject.transform.localRotation = Quaternion.Slerp(tracked.gameObject.transform.localRotation, adjustedRotation, Time.deltaTime * transitionSpeed);
                        tracked.gameObject.transform.localScale = Vector3.Lerp(tracked.gameObject.transform.localScale, adjustedScale, Time.deltaTime * transitionSpeed);
                        break;
                    }
                }
            }
        }
    }

    Quaternion ApplyCustomRotation(Quaternion originalRotation, Vector3 eulerAngleOffset)
    {
        // Decompose the original rotation to its axes
        originalRotation.ToAngleAxis(out float angle, out Vector3 axis);
        Quaternion baseRotation = Quaternion.AngleAxis(angle, axis);

        // Apply Euler angle offsets correctly to avoid unintended axis interactions
        Quaternion rotationX = Quaternion.AngleAxis(eulerAngleOffset.x, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(eulerAngleOffset.y, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(eulerAngleOffset.z, Vector3.forward);

        // Compound the rotations, prioritizing the Z axis roll
        return baseRotation * rotationZ * rotationY * rotationX;
    }
}
