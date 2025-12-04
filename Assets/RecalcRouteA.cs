using UnityEngine;
using System.Collections;

public class RecalcRouteA : MonoBehaviour
{

    public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Waypoint4;

    private void OnTriggerEnter(Collider other)
    {
        Waypoint1.transform.position = new Vector3(-157.9356f, 0f, 177.0451f);
        Waypoint2.transform.position = new Vector3(-170.708f, 0f, 177.2885f);
        Waypoint3.transform.position = new Vector3(-190.4859f, 0f, 176.992f);
        Waypoint4.transform.position = new Vector3(-210.7999f, 0f, 177.4917f);
    }

}
