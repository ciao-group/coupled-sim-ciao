using HealthbarGames;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrosswalkZone : MonoBehaviour
{

    public Transform stopLine;
    public TrafficLightManager trafficLightManager;
    public string phaseName;

    private void OnDrawGizmos()
    {
        if (stopLine)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = stopLine.localToWorldMatrix;
            Gizmos.DrawSphere(Vector3.zero, 0.25f);
            Gizmos.DrawLine(Vector3.left * 2f, Vector3.right * 2f);
            Gizmos.matrix = Matrix4x4.identity;
        }

        BoxCollider col = GetComponent<BoxCollider>();
        if (col)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(col.center, col.size);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
    public TrafficLightPhase GetPhase()
    {
        if (trafficLightManager == null || string.IsNullOrEmpty(phaseName))
            return null;

        var phaseField = typeof(TrafficLightManager).GetField("PhaseList",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (phaseField == null)
            return null;

        var list = phaseField.GetValue(trafficLightManager) as List<TrafficLightPhase>;
        if (list == null || list.Count == 0)
            return null;

        foreach (var p in list)
        {
            if (p != null && p.Name == phaseName)
                return p;
        }

        return null;
    }

    void Start()
    {
        if (trafficLightManager == null)
        {
            Debug.LogWarning($"{name}: No TrafficLightManager assigned!");
            return;
        }

        var phaseField = typeof(TrafficLightManager).GetField("PhaseList",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (phaseField == null)
        {
            Debug.LogError("Could not find PhaseList field on TrafficLightManager");
            return;
        }

        var list = phaseField.GetValue(trafficLightManager) as List<TrafficLightPhase>;
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning($"{trafficLightManager.name}: Phase list is empty or null!");
            return;
        }

        Debug.Log($"{trafficLightManager.name} contains {list.Count} phases:");
        foreach (var phase in list)
        {
            if (phase == null)
            {
                Debug.Log("   - (null entry)");
                continue;
            }

            Debug.Log($"   {phase.Name} | State: {phase.GetState()} | Start:{phase.PhaseStartTime}s | Green:{phase.PhaseActiveTime}s | End:{phase.PhaseEndTime}s");
        }
    }
}
