using System.Collections;
using TMPro;
using UnityEngine;

public class DestinationReached : MonoBehaviour
{

    public IVISLogic IvisLogic;

    public AudioSource DestinationReachedSound;
    [Header("Assistant Voice Output")]
    [SerializeField] public AudioSource voiceSource;
    [SerializeField] public AudioClip destinationClip;

    [TextArea] public string explanationText;

    private void OnTriggerEnter(Collider other)
    {
        // Change sprite to ACTIVE
        IvisLogic.agentImage.sprite = IvisLogic.activeSprite;

        // Update bubble text
        IvisLogic.explanationText.text = explanationText;

        // Show bubble and play sound and voice clip

        IvisLogic.FadeInBubble();

        StartCoroutine(destinationReached());
    }

    IEnumerator destinationReached()
    {
        if (DestinationReachedSound != null)
        {
            DestinationReachedSound.Play();
            yield return new WaitForSeconds(2);
            voiceSource.clip = destinationClip;
            voiceSource.Play();
        }
    }
}
