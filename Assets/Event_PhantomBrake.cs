using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_PhantomBrake : MonoBehaviour
{
    [Header("Object marker that will dissappear")]
    public GameObject phantomMarker;

    [Header("Brake zones to simulate stopping behaviour (will deactivate)")]
    public GameObject brakeZoneForStopping1;
    public GameObject brakeZoneForStopping2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            phantomMarker.SetActive(true);

            StartCoroutine(DespawnBrakeZone(17f));
            // despawn the brake zones after some hesitation
            StartCoroutine(DespawnPhantom(10f));
            // disable trigger 
            GetComponent<Collider>().enabled = false;

            Debug.Log("Triggered Event D");
        }
    }

    private IEnumerator DespawnBrakeZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (brakeZoneForStopping1 != null && brakeZoneForStopping2 != null)
        {
            brakeZoneForStopping1.SetActive(false);
            brakeZoneForStopping2.SetActive(false);
        }


        Debug.Log("Event D brake zones despawned");
    }

    private IEnumerator DespawnPhantom(float time)
    {
        yield return new WaitForSeconds(time);

        if (phantomMarker != null)
            phantomMarker.SetActive(false);

        Debug.Log("Event D phantom despawned");
    }

}
