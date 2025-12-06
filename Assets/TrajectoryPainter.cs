using UnityEngine;
using System.Collections.Generic;


public class TrajectoryPainter : MonoBehaviour
{
    [Header("Car")]
    public RCC_AICarController aiController;

    [Header("Trajectory")]
    public LineRenderer lineRenderer;

    public float heightOffset = 1.5f;                 
    public int previewWaypointCount = 10;     
    public int smoothness = 10;               
    public Color baseColor = Color.cyan;
    public float lineWidth = 0.12f;

    [Header("Waypoints")]
    public bool isCycle = false;
    public float WPIndexDestination;

    private Material lineMaterial;

    void Awake()
    {

        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
            shader = Shader.Find("Unlit/Transparent"); // fallback

        lineMaterial = new Material(shader);

        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        lineMaterial.DisableKeyword("_ALPHATEST_ON");
        lineMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        lineMaterial.SetInt("_ZWrite", 0);
        lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        lineMaterial.renderQueue = 1000;

        lineRenderer.material = lineMaterial;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.numCapVertices = 6;
        lineRenderer.numCornerVertices = 6;
        lineRenderer.textureMode = LineTextureMode.Stretch;

        // initial gradient (will be updated every frame)
        ApplyGradient(1.0f, 0.0f);
    }

    void LateUpdate()
    {
        if (aiController == null || aiController.waypointsContainer == null)
            return;

        var waypoints = aiController.waypointsContainer.waypoints;

        // draw no line if there is no waypoint container attached to car
        if (waypoints == null || waypoints.Count < 1)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        int current = aiController.currentWaypointIndex;
        int count = Mathf.Min(previewWaypointCount, waypoints.Count);


        List<Vector3> controlPoints = new List<Vector3>();
        Vector3 under = new Vector3(0f, heightOffset, 0f);
        controlPoints.Add(aiController.transform.position + under);

        for (int i = 0; i < count; i++)
        {
            int idx = (current + i) % waypoints.Count;

  
            // simulate star-to-finish line by breaking once destination index is reached
            if (!isCycle && idx >= (WPIndexDestination-1))
            {
                Debug.Log(idx);
                break;
            }

            controlPoints.Add(waypoints[idx].transform.position + under);
        }

        if (controlPoints.Count == 1)
        {
            Vector3 p0 = controlPoints[0];
            Vector3 p1 = p0 + aiController.transform.forward * 1.0f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, p0);
            lineRenderer.SetPosition(1, p1);

            // gradient goes solid to faded out
            ApplyGradient(1f, 0f);
            return;
        }

        //smooth points using Catmull-Rom with endpoints duplicated for stability
        List<Vector3> smoothPoints = GenerateSmoothCurve(controlPoints, smoothness);

        if (smoothPoints == null || smoothPoints.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = smoothPoints.Count;
        lineRenderer.SetPositions(smoothPoints.ToArray());

        ApplyGradient(1.0f, 0.05f);
    }

    // gradient from baseColor with startAlpha -> endAlpha
    private void ApplyGradient(float startAlpha, float endAlpha)
    {
        Gradient g = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(baseColor, 0f);
        colorKeys[1] = new GradientColorKey(baseColor, 1f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(Mathf.Clamp01(startAlpha), 0f);
        alphaKeys[1] = new GradientAlphaKey(Mathf.Clamp01(endAlpha), 1f);

        g.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = g;

    }

    // Catmull-Rom spline generator
    private List<Vector3> GenerateSmoothCurve(List<Vector3> points, int subdivisions)
    {
        List<Vector3> result = new List<Vector3>();

        if (points == null || points.Count < 2)
            return result;

        // For Catmull-Rom we want P-1..Pn+1; duplicate endpoints for natural endpoints
        List<Vector3> pts = new List<Vector3>();
        pts.Add(points[0]); // duplicate first
        pts.AddRange(points);
        pts.Add(points[points.Count - 1]); // duplicate last

        for (int i = 0; i < pts.Count - 3; i++)
        {
            Vector3 p0 = pts[i];
            Vector3 p1 = pts[i + 1];
            Vector3 p2 = pts[i + 2];
            Vector3 p3 = pts[i + 3];

            for (int j = 0; j <= subdivisions; j++)
            {
                float t = j / (float)subdivisions;
                float t2 = t * t;
                float t3 = t2 * t;

                Vector3 pos = 0.5f * (
                    (2f * p1) +
                    (-p0 + p2) * t +
                    (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                    (-p0 + 3f * p1 - 3f * p2 + p3) * t3
                );
                result.Add(pos);
            }
        }

        return result;
    }
}
