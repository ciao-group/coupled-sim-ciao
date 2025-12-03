using UnityEngine;


public enum ZoneType
{
    A,
    B,
    C,
    N
}

public class ZoneTrigger : MonoBehaviour
{
    public ZoneType zoneType;
    public AudioSource zoneEnterSound;

    [TextArea] public string whyText;
    [TextArea] public string whatText;

    public void PlayEnterSound()
    {
        if (zoneEnterSound != null)
        {
            zoneEnterSound.Play();
        }
    }
}