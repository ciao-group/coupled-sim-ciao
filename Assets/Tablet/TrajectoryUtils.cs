using UnityEngine;
using System.Collections.Generic;

public static class TrajectoryUtils
{
    // Catmull-Rom spline interpolation
    public static List<Vector3> GetCatmullRomSpline(List<Vector3> waypoints, int samplesPerSegment)
    {
        List<Vector3> smoothPoints = new List<Vector3>();

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? waypoints[i] : waypoints[i - 1];
            Vector3 p1 = waypoints[i];
            Vector3 p2 = waypoints[i + 1];
            Vector3 p3 = i + 2 < waypoints.Count ? waypoints[i + 2] : waypoints[i + 1];

            for (int j = 0; j < samplesPerSegment; j++)
            {
                float t = j / (float)samplesPerSegment;
                Vector3 point = 0.5f * (
                    (2 * p1) +
                    (-p0 + p2) * t +
                    (2 * p0 - 5 * p1 + 4 * p2 - p3) * (t * t) +
                    (-p0 + 3 * p1 - 3 * p2 + p3) * (t * t * t)
                );
                smoothPoints.Add(point);
            }
        }
        return smoothPoints;
    }
}
