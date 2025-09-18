using UnityEngine;
using System.Collections.Generic;

public class AITrajectoryVisualizer : MonoBehaviour
{
    public RCC_CarControllerV3 AIController;
    public RCC_AIWaypointsContainer waypointsContainer;
    public LineRenderer lineRenderer;
    public int lookAheadWaypoints = 20;
    public int samplesPerSegment = 10;
    public float lineWidth = 0.3f;
    public float lineHeightOffset = 0.1f; // slightly above ground

    public int currentWaypointIndex = 0;

    void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRenderer.receiveShadows = false;
            lineRenderer.widthMultiplier = lineWidth;
            lineRenderer.positionCount = 0;
            lineRenderer.alignment = LineAlignment.TransformZ;
        }
    }

    void Update()
    {
        if (AIController == null || waypointsContainer == null || waypointsContainer.waypoints.Count == 0)
            return;

        List<Vector3> points = new List<Vector3>();
        RCC_Waypoint currentWaypoint = waypointsContainer.waypoints[currentWaypointIndex];

        for (int i = 0; i < lookAheadWaypoints; i++)
        {
            int wpIndex = (currentWaypointIndex + i) % waypointsContainer.waypoints.Count;
            Vector3 wp = currentWaypoint.transform.position;
            wp.y += lineHeightOffset;
            points.Add(wp);
        }

        List<Vector3> smoothPoints = TrajectoryUtils.GetCatmullRomSpline(points, samplesPerSegment);

        lineRenderer.positionCount = smoothPoints.Count;
        lineRenderer.SetPositions(smoothPoints.ToArray());
    }
}
