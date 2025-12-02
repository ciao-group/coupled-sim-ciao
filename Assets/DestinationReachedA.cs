using System.Collections;
using TMPro;
using UnityEngine;

public class DestinationReachedA : MonoBehaviour
{

    public IVISLogic IvisLogic;

    public AudioSource DestinationReachedSound;

    [TextArea] public string whyText;
    [TextArea] public string whatText;

    private void OnTriggerEnter(Collider other)
    {
        // Change sprite to ACTIVE
        IvisLogic.agentImage.sprite = IvisLogic.activeSprite;

        // Update bubble text
        IvisLogic.WhyText.text = whyText;
        IvisLogic.WhatText.text = whatText;

        // Show bubble and play sound

        IvisLogic.FadeInBubble();

        if (DestinationReachedSound != null)
        {
            DestinationReachedSound.Play();
        }
    }

}
