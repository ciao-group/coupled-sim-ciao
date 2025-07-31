using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonLogic : MonoBehaviour
{


    [Header("Menu Bar Button Settings")]

    [Tooltip("AudioSource to mute/unmute and volume control (speaker).")]
    [SerializeField] private AudioSource audioSource;


    [Header("Music Player")]

    [Tooltip("Progress Slider of the song played.")]
    [SerializeField] private Slider progressSlider;


    [Tooltip("Time elapsed / progressed.")]
    [SerializeField] private TextMeshProUGUI currentTimeText;

    [Tooltip("Total duration of the song.")]
    [SerializeField] private TextMeshProUGUI totalTimeText;

    [Tooltip("Button Icon to play audio source.")]
    [SerializeField] private Sprite playIcon;

    [Tooltip("Button Icon to pause audio source.")]
    [SerializeField] private Sprite pauseIcon;

    [Tooltip("UI Image that displays the play/pause icon.")]
    [SerializeField] private Image playImage;


    [Header("Volume Control Panel")]

    [Tooltip("Volume Slider GameObject")]
    [SerializeField] private Slider volumeSlider;


    private bool isDragging = false;
    private bool isPlaying = false;


    void Start()
    {

        // Set total duration once at start
        if (audioSource.clip != null)
        {
            float totalSeconds = audioSource.clip.length;
            totalTimeText.text = FormatTime(totalSeconds);
            progressSlider.maxValue = totalSeconds;

        }

        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume;

        }

        if (progressSlider != null && audioSource.clip != null)
        {
            float totalSeconds = audioSource.clip.length;
            totalTimeText.text = FormatTime(totalSeconds);
            progressSlider.maxValue = totalSeconds;

        }
    }

    void Update()
    {
        if (audioSource.clip == null || isDragging)
            return;

        progressSlider.value = audioSource.time;
        currentTimeText.text = FormatTime(audioSource.time);
    }

    /// <summary>
    /// MENU BAR
    /// </summary>
    public void TogglePanel(GameObject panel) // on click event for menu buttons
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    /// <summary>
    /// MUSIC PLAYER MENU
    /// </summary>
    public void TogglePlay() // attach this to play/pause button
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

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }

    /// <summary>
    /// VOLUME CONTROL MENU
    /// </summary>

    public void SetVolume(float value)
    {
        if (audioSource != null)
            audioSource.volume = value;
    }


    public void OnSliderValueChanged(float value)
    {
        if (currentTimeText != null)
            currentTimeText.text = FormatTime(value);

        audioSource.time = progressSlider.value;

    }


}







