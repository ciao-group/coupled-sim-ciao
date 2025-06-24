using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonLogic : MonoBehaviour
{


    [Header("Menu Bar Button Settings")]

    [Tooltip("Volume menu Icon.")]
    [SerializeField] private Sprite volumeIcon;

    [Tooltip("UI Image that displays the mute icon.")]
    [SerializeField] private Image volumeImage;

    [Tooltip("Icon when audio is playing.")]
    [SerializeField] private Sprite playIcon;

    [Tooltip("Icon when audio is paused.")]
    [SerializeField] private Sprite pauseIcon;

    [Tooltip("UI Image that displays the play/pause icon.")]
    [SerializeField] private Image playImage;

    [Tooltip("Music Panel GameObject")]
    [SerializeField] private GameObject musicPanel;

    [Tooltip("Volume slider GameObject")]
    [SerializeField] private Slider volumeSlider;

    [Tooltip("Music Panel GameObject")]
    [SerializeField] private GameObject volumePanel;

    [Header("Music Player")]

    [Tooltip("AudioSource to mute/unmute and volume control.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Progress Slider of the song played.")]
    [SerializeField] private Slider progressSlider;

    [Tooltip("Time elapsed / progressed.")]
    [SerializeField] private TextMeshProUGUI currentTimeText;

    [Tooltip("Total duration of the song.")]
    [SerializeField] private TextMeshProUGUI totalTimeText;


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
            volumeSlider.onValueChanged.AddListener(SetVolume);

            AddSliderEventTriggers(volumeSlider);
        }

        if (progressSlider != null && audioSource.clip != null)
        {
            float totalSeconds = audioSource.clip.length;
            totalTimeText.text = FormatTime(totalSeconds);
            progressSlider.maxValue = totalSeconds;

            AddSliderEventTriggers(progressSlider);
        }
    }
    public void SetDragging(bool dragging)
    {
        if (!dragging && progressSlider != null && audioSource != null)
        {
            audioSource.time = progressSlider.value;
        }

        isDragging = dragging;
    }

    void Update()
    {
        if (audioSource.clip == null || isDragging)
            return;

        progressSlider.value = audioSource.time;
        currentTimeText.text = FormatTime(audioSource.time);
    }

    private void AddSliderEventTriggers(Slider slider)
    {
        EventTrigger trigger = slider.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slider.gameObject.AddComponent<EventTrigger>();
        }
        else
        {
            trigger.triggers.Clear();
        }

        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener((eventData) => SetDragging(true));

        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener((eventData) => SetDragging(false));

        trigger.triggers.Add(beginDrag);
        trigger.triggers.Add(endDrag);
    }

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

    // toggle menu visibility functions
    public void ToggleMusicMenu()
    {
        if (musicPanel != null)
        {
            musicPanel.SetActive(!musicPanel.activeSelf);
        }
    }
    public void ToggleVolumeMenu()
    {
        if (volumePanel != null)
        {
            volumePanel.SetActive(!volumePanel.activeSelf);
        }
    }

    //
    public void SetVolume(float value)
    {
        if (audioSource != null)
            audioSource.volume = value;
    }


    public void OnSliderValueChanged(float value)
    {
        if (currentTimeText != null)
            currentTimeText.text = FormatTime(value);

    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }
}







