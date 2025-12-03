using UnityEngine;

public class ZoneDetector : MonoBehaviour
{
    public IVISLogic LogicScript;
    public ExperimentConfigs Configs;
    public ZoneType currentZone = ZoneType.N;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZoneTrigger zone) && Configs.condition != ConditionType.Nevo)
        {
            LogicScript.EnterZone(zone);
            zone.PlayEnterSound();
            currentZone = zone.zoneType;
            Debug.Log("Zone " + zone.zoneType + " entered.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ZoneTrigger zone) && Configs.condition != ConditionType.Nevo)
        {
            LogicScript.ExitZone();
            currentZone = ZoneType.N;
            Debug.Log("Zone " + zone.zoneType + " left.");
        }
    }
}