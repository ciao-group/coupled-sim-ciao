using UnityEngine;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{


    [Header("Menu Bar Button Settings")]
    [Tooltip("AudioSource to mute/unmute.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Icon when audio is not muted.")]
    [SerializeField] private Sprite volumeIcon;

    [Tooltip("Icon when audio is muted.")]
    [SerializeField] private Sprite muteIcon;

    [Tooltip("UI Image that displays the mute/unmute icon.")]
    [SerializeField] private Image volumeImage;

    [Tooltip("Icon when audio is playing.")]
    [SerializeField] private Sprite playIcon;

    [Tooltip("Icon when audio is paused.")]
    [SerializeField] private Sprite pauseIcon;

    [Tooltip("UI Image that displays the play/pause icon.")]
    [SerializeField] private Image playImage;

    private bool isPlaying = false;

    public void TogglePlay()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            playImage.sprite = pauseIcon;
            audioSource.Play();
        }
        else
        {
            playImage.sprite = playIcon;
            audioSource.Pause();
        }
    }
    private bool isMuted = false;


    public void ToggleMute()
    {
        isMuted = !isMuted;

        audioSource.mute = isMuted;

        volumeImage.sprite = isMuted ? muteIcon : volumeIcon;
    }
}
