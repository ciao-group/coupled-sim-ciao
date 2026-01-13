using System.Collections;
using System.Collections.Generic;
using HealthbarGames;
using UnityEngine;

public class Event_TrafficLight : MonoBehaviour
{
    [Header("Traffic Light Left")]
    public RealTrafficLight TrafficLightL;
    [Header("Traffic Light Right")]
    public RealTrafficLight TrafficLightR;

    [Header("Despawn Settings")]
    [Header("Brakezones to simulate stopping behaviour (will deactivate)")]
    public GameObject BrakeZoneForStopping1;
    public GameObject BrakeZoneForStopping2;
    void Start()
    {
        Color darkgray = new Color(0.66f, 0.66f, 0.66f, 1f);
        TrafficLightL.LightsOnMatRuntime.SetColor("_BaseColor", Color.gray);
        TrafficLightR.LightsOnMatRuntime.SetColor("_BaseColor", Color.gray);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BrakeZoneForStopping1.SetActive(true);
            BrakeZoneForStopping2.SetActive(true);

            StartCoroutine(DespawnBrakeZone(25f));
        }

    }

    private IEnumerator DespawnBrakeZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BrakeZoneForStopping1 != null && BrakeZoneForStopping2 != null)
        {
            BrakeZoneForStopping1.SetActive(false);
            BrakeZoneForStopping2.SetActive(false);
            Debug.Log("Event C brake zones despawned");
        }

    }
}
