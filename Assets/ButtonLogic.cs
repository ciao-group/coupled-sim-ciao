using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonLogic : MonoBehaviour
{


    [Header("Menu Bar Button Settings")]

    [Tooltip("Icon when audio is not muted.")]
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
        if (volumeSlider != null && audioSource != null)
        {
            // Set slider to match current volume
            volumeSlider.value = audioSource.volume;

            // Add listener for changes
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Set total duration once at start
        if (audioSource.clip != null)
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

        // Update slider value and current time display
        progressSlider.value = audioSource.time;
        currentTimeText.text = FormatTime(audioSource.time);
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

    public void ToggleMusicMenu()
    {
        if (musicPanel != null)
        {
            musicPanel.SetActive(!musicPanel.activeSelf);
        }
    }

    public void SetVolume(float value)
    {
        if (audioSource != null)
            audioSource.volume = value;
    }

    public void ToggleVolumeMenu()
    {
        if (volumePanel != null)
        {
            volumePanel.SetActive(!volumePanel.activeSelf);
        }
    }

    public void OnSliderValueChanged(float value)
    {
        if (isDragging)
        {
            currentTimeText.text = FormatTime(value);
        }
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }
}







