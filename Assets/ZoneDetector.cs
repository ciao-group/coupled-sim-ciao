using UnityEngine;

public class ZoneDetector : MonoBehaviour
{
    public IVISLogic LogicScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZoneTrigger zone))
        {
            LogicScript.EnterZone(zone);
            zone.PlayEnterSound();
            Debug.Log("Zone entered.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ZoneTrigger zone))
        {
            LogicScript.ExitZone();
            Debug.Log("Zone left.");
        }
    }
}