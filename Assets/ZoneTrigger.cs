using UnityEngine;


public enum ZoneType
{
    ZoneA,
    ZoneB,
    ZoneC,
    ZoneD
}

public class ZoneTrigger : MonoBehaviour
{
    public ZoneType zoneType;
    public AudioSource zoneEnterSound;

    [TextArea] public string howText;
    [TextArea] public string whatText;

    public void PlayEnterSound()
    {
        if (zoneEnterSound != null)
        {
            zoneEnterSound.Play();
        }
    }
}