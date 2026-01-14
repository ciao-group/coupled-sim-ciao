using UnityEngine;
using System.Collections;

public class Event_Bench : MonoBehaviour
{

    [Header("Brakezones to simulate stopping behaviour (will deactivate)")]
    public GameObject BrakeZoneForStopping1;
    public GameObject BrakeZoneForStopping2;

    [Header("Waypoints to simulate recalculation of route")]
    public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Waypoint4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(RecalculateRoute(8f));
            StartCoroutine(DespawnBrakeZone(11f));
        }
    }
    private IEnumerator RecalculateRoute(float time)
    {
        yield return new WaitForSeconds(time);

        Waypoint1.transform.position = new Vector3(-195f, 0.7f, -49.2f);
        Waypoint2.transform.position = new Vector3(-195f, 0.7f, -43.5f);
        Waypoint3.transform.position = new Vector3(-195f, 0.7f, -37.2f);
        Waypoint4.transform.position = new Vector3(-195f, 0.7f, -30.8f);

        Debug.Log("Moved Waypoints");
    }
    private IEnumerator DespawnBrakeZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BrakeZoneForStopping1 != null && BrakeZoneForStopping2 != null)
        {
            BrakeZoneForStopping1.SetActive(false);
            BrakeZoneForStopping2.SetActive(false);
        }
        Debug.Log("Event B brake zones despawned");
    }
}
