using System.Collections;
using TMPro;
using UnityEngine;

public class DestinationReached : MonoBehaviour
{

    public IVISLogic IvisLogic;
    public ExperimentConfigs Configs;

    public AudioSource DestinationReachedSound;
    [Header("Assistant Voice Output")]
    public AudioClip lumoClip;
    public AudioClip codaClip;
    public AudioClip neloClip;

    public AudioClip DestinationClip =>
        Configs.condition == ConditionType.Lumo ? lumoClip :
        Configs.condition == ConditionType.Coda ? codaClip :
        Configs.condition == ConditionType.Nevo ? neloClip :
        null;

    [SerializeField] public AudioSource voiceSource;

    [TextArea] public string explanationText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Change sprite to ACTIVE
            IvisLogic.agentImage.sprite = IvisLogic.activeSprite;

            // Update bubble text
            IvisLogic.explanationText.text = explanationText;

            IvisLogic.FadeInBubble();
            StartCoroutine(DestinationReachedClip());
            GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator DestinationReachedClip()
    {
        if (DestinationReachedSound != null)
        {
            DestinationReachedSound.Play();
            yield return new WaitForSeconds(2);
            voiceSource.clip = DestinationClip;
            voiceSource.Play();
        }
    }
}
