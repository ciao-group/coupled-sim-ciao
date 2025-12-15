using UnityEngine;
using System.Collections;

public class RouteBEventBTrigger : MonoBehaviour
{

    [Header("Brakezones to simulate stopping behaviour (will deactivate)")]
    public GameObject BrakeZoneforStopping1;
    public GameObject BrakeZoneforStopping2;

    [Header("Waypoints to simulate recalculation of route")]
    public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Waypoint4;

    private void OnTriggerEnter(Collider other)
    {
        
        StartCoroutine(RecalculateRoute(6f));
        StartCoroutine(DespawnBrakeZone(7f));

    }
    private IEnumerator RecalculateRoute(float time)
    {
        yield return new WaitForSeconds(time);

        Waypoint1.transform.position = new Vector3(-68.3f, 0f, 10f);
        Waypoint2.transform.position = new Vector3(-62.97f, 0f, 10f);
        Waypoint3.transform.position = new Vector3(-57.72731f, 0f, 10f);
        Waypoint4.transform.position = new Vector3(-52.39731f, 0f, 10f);

        Debug.Log("Moved Waypoints");
    }
    private IEnumerator DespawnBrakeZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BrakeZoneforStopping1 != null && BrakeZoneforStopping2 != null)
            BrakeZoneforStopping1.SetActive(false);
        BrakeZoneforStopping2.SetActive(false);

        Debug.Log("Event B brake zones despawned");
    }
}
