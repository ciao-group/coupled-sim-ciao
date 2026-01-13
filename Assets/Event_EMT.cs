using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_EMT : MonoBehaviour
{
    [Header("EMT AI Car")]
    public GameObject emt;


    [Header("Waypoints to simulate recalculation of route")]
    public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Waypoint4;
    public Transform Waypoint5;

    private void OnTriggerEnter(Collider other)
    {
        if (emt != null && other.gameObject.tag == "Player")
        {
            SpawnEMT();

            StartCoroutine(RecalculateRoute(12f));

            GetComponent<Collider>().enabled = false;
        }
    }

    private void SpawnEMT()
    {
        if (emt != null)
        {
            emt.SetActive(true);
            //emt.GetComponent<RCC_AICarController>().enabled = true;
            //emt.GetComponent<RCC_CarControllerV3>().enabled = true;
            DespawnAfterTime(20f);
        }
    }

    private IEnumerator RecalculateRoute(float time)
    {
        yield return new WaitForSeconds(time);

        Waypoint1.transform.position = new Vector3(-263.5468f, 4f, -192.5f);
        Waypoint2.transform.position = new Vector3(-281.7859f, 4f, -192.5f);
        Waypoint3.transform.position = new Vector3(-304.321f, 5f, -192.5f);
        Waypoint4.transform.position = new Vector3(-319.7604f, 4.7f, -192.5f);
        Waypoint5.transform.position = new Vector3(-345.054f, 2.7f, -192.5f);

        Debug.Log("Moved Waypoints");
    }
    private IEnumerator DespawnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (emt != null)
            emt.SetActive(false);

        Debug.Log("emt despawned");
    }



}
