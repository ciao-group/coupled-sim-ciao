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
    public ExperimentConfigs Configs;

    public ZoneType zoneType;
    public AudioSource zoneEnterSound;
    public AudioClip lumoClip;
    public AudioClip codaClip;

    public AudioClip voiceClip =>
        Configs.condition == ConditionType.Lumo ? lumoClip :
        Configs.condition == ConditionType.Coda ? codaClip :
        null;

    [TextArea] public string explanationText;

    public void PlayEnterSound()
    {
        if (zoneEnterSound != null)
        {
            zoneEnterSound.Play();
        }
    }
}