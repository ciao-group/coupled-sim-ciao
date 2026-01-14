using UnityEngine;
using System.Collections;

public class RecalculateEventTrigger : MonoBehaviour
{

    [Header("Brake Zones to simulate stopping behaviour (will deactivate)")]
    public GameObject brakeZoneForStopping1;
    public GameObject brakeZoneForStopping2;
    [Header("Waypoints to be moved")]
    public Transform waypoint1;
    public Transform waypoint2;
    public Transform waypoint3;
    public Transform waypoint4;
    [Header("Vector3 positions to move waypoints to")]
    public Vector3 newWaypoint1;
    public Vector3 newWaypoint2;
    public Vector3 newWaypoint3;
    public Vector3 newWaypoint4;
    [Header("When to move Waypoints")]
    public float timeWaypoints;
    [Header("When to deactivate Brake Zones")]
    public float timeBrakeZone;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RecalculateRoute(timeWaypoints));
            StartCoroutine(DespawnBrakeZones(timeBrakeZone));
        }
    }

    private IEnumerator RecalculateRoute(float time)
    {
        yield return new WaitForSeconds(time);

        waypoint1.transform.position = newWaypoint1;
        waypoint2.transform.position = newWaypoint2;
        waypoint3.transform.position = newWaypoint3;
        waypoint4.transform.position = newWaypoint4;
        Debug.Log("Moved Waypoints");
    }

    private IEnumerator DespawnBrakeZones(float time)
    {
        yield return new WaitForSeconds(time);

        if (brakeZoneForStopping1 != null)
            brakeZoneForStopping1.SetActive(false);
        if (brakeZoneForStopping2 != null)
            brakeZoneForStopping2.SetActive(false);
        Debug.Log("Event Brake Zones despawned");
    }
}
