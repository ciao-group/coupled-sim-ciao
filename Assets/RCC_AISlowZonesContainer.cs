using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/RCC AI Slow Zones Container")]
public class RCC_AISlowZonesContainer : MonoBehaviour
{
    public List<Transform> slowZones = new List<Transform>();
    public float cylinderHeight = 10f;
    public int segments = 32;

    private void OnDrawGizmos()
    {
        foreach (Transform zoneTransform in slowZones)
        {
            if (!zoneTransform)
                continue;

            RCC_AISlowZone zone = zoneTransform.GetComponent<RCC_AISlowZone>();
            if (!zone)
                continue;

            DrawCylinder(zoneTransform.position, zone.distance, cylinderHeight);
        }
    }

    void DrawCylinder(Vector3 center, float radius, float height)
    {
        Gizmos.color = new Color(1f, .5f, 0f, 0.25f);

        Vector3 top = center + Vector3.up * (height * 0.5f);
        Vector3 bottom = center - Vector3.up * (height * 0.5f);

        float angleStep = 360f / segments;

        Vector3 prevTop = top + new Vector3(radius, 0f, 0f);
        Vector3 prevBottom = bottom + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            Vector3 curTop = top + offset;
            Vector3 curBottom = bottom + offset;

            // vertical edge
            Gizmos.DrawLine(curTop, curBottom);

            // top circle
            Gizmos.DrawLine(prevTop, curTop);

            // bottom circle
            Gizmos.DrawLine(prevBottom, curBottom);

            prevTop = curTop;
            prevBottom = curBottom;
        }
    }
}
