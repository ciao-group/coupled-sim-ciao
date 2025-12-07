using UnityEngine;
using System.Collections;

public class RouteBEventBTrigger : MonoBehaviour
{
    [Header("Waypoints to simulate recalculation of route")]
    public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Waypoint4;

    private void OnTriggerEnter(Collider other)
    {
        Waypoint1.transform.position = new Vector3(-68.3f, 0f, 10f);
        Waypoint2.transform.position = new Vector3(-62.97f, 0f, 10f);
        Waypoint3.transform.position = new Vector3(-57.72731f, 0f, 10f);
        Waypoint4.transform.position = new Vector3(-52.39731f, 0f, 10f);
    }

}
