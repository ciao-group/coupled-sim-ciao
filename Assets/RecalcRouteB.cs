using UnityEngine;
using System.Collections;

public class RecalcRouteB : MonoBehaviour
{

    public Transform Waypoint1;
    public Transform Waypoint2;

    private void OnTriggerEnter(Collider other)
    {
        Waypoint1.transform.position = new Vector3(-68.3f, 0f, 10f);
        Waypoint2.transform.position = new Vector3(-68.3f, 0f, 10f);
    }

}
