using UnityEngine;


public enum ZoneType
{
    A,
    B,
    C,
    D,
    N
}

public class ZoneTrigger : MonoBehaviour
{
    public ZoneType zoneType;
    public AudioSource zoneEnterSound;
    public AudioClip voiceClip;

    [TextArea] public string explanationText;

    public void PlayEnterSound()
    {
        if (zoneEnterSound != null)
        {
            zoneEnterSound.Play();
        }
    }
}