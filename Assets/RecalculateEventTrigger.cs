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
        if (other.gameObject.layer == LayerMask.NameToLayer("RCC"))
        {
            StartCoroutine(RecalculateRoute(timeWaypoints));
            StartCoroutine(DespawnBrakeZones(timeBrakeZone));
        }
    }

    private IEnumerator RecalculateRoute(float time)
    {
        yield return new WaitForSeconds(time);

        waypoint1.transform.position = new Vector3(-157.9356f, 0f, 177.0451f);
        waypoint2.transform.position = new Vector3(-170.708f, 0f, 177.2885f);
        waypoint3.transform.position = new Vector3(-190.4859f, 0f, 176.992f);
        waypoint4.transform.position = new Vector3(-210.7999f, 0f, 177.4917f);
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
