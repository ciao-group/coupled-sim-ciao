using System.Collections;
using System.Media;
using Barmetler;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using Varjo.XR;

public class IVISLogic : MonoBehaviour
{

    public ExperimentConfigs Configs;

    [Header("Assignments")]
    [SerializeField] private RCC_AICarController aiCar;
    [SerializeField] private RectTransform homeScreen;
    [SerializeField] private RectTransform toggleButton;
    [SerializeField] private Vector2 homeHiddenPos; // Offscreen position
    [SerializeField] private Vector2 homeVisiblePos; // Target onscreen position
    [SerializeField] private Vector2 buttonHiddenPos; // Start button position
    [SerializeField] private Vector2 buttonVisiblePos;

    [Header("Assistant")]
    [SerializeField] public Button agentButton;
    [SerializeField] public Image agentImage;
    [SerializeField] public Sprite idleSprite;
    [SerializeField] public Sprite alertSprite;
    [SerializeField] public Sprite activeSprite;
    [SerializeField] public AudioSource VoiceSource;
    [SerializeField] public AudioClip lumo2;
    [SerializeField] public AudioClip lumo3;


    [Header("Explanation")]
    [SerializeField] public CanvasGroup agentSpeechBubble;
    [SerializeField] public TextMeshProUGUI WhyText;
    [SerializeField] public TextMeshProUGUI WhatText;

    private ZoneTrigger currentZone;
    private Coroutine fadeRoutine;

    private bool isOpen = false;

    [Header("Music Player")]

    [Tooltip("AudioSource to mute/unmute and volume control (speaker).")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Button Icon to play audio source.")]
    [SerializeField] private Sprite playIcon;

    [Tooltip("Button Icon to pause audio source.")]
    [SerializeField] private Sprite pauseIcon;

    [Tooltip("UI Image that displays the play/pause icon.")]
    [SerializeField] private SVGImage playImage;

    [Tooltip("Button Icon to mute audio source.")]
    [SerializeField] private Sprite muteIcon;

    [Tooltip("Button Icon to unmute audio source.")]
    [SerializeField] private Sprite volumeIcon;

    [Tooltip("UI Image that displays the volume icon.")]
    [SerializeField] private SVGImage volumeImage;


    private bool isPlaying = false;
    private bool isMuted = false;
    private float previousVolume = 1f;


    private float duration = 0.4f;
    private bool isVisible = false;
    private Coroutine currentAnim;


    public void ToggleHomeScreen()
    {
        isOpen = !isOpen;

        // Activate Home Screen if opening
        if (isOpen) homeScreen.gameObject.SetActive(true);

        if (currentAnim != null) StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(SlideUI());
    }

    private IEnumerator SlideUI()
    {
        Vector2 startHomePos = homeScreen.anchoredPosition;
        Vector2 targetHomePos = isOpen ? homeVisiblePos : homeHiddenPos;

        Vector2 startButtonPos = toggleButton.anchoredPosition;
        Vector2 targetButtonPos = isOpen ? buttonVisiblePos : buttonHiddenPos;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            homeScreen.anchoredPosition = Vector2.Lerp(startHomePos, targetHomePos, t);
            toggleButton.anchoredPosition = Vector2.Lerp(startButtonPos, targetButtonPos, t);

            yield return null;
        }

        homeScreen.anchoredPosition = targetHomePos;
        toggleButton.anchoredPosition = targetButtonPos;

        // Deactivate Home Screen if closing
        if (!isOpen) homeScreen.gameObject.SetActive(false);

        currentAnim = null;
    }


    // turn RCC waypoint following on / off
    public void ActivateAI()
    {
        if (aiCar != null)
        {
            aiCar.enabled = true;
            aiCar.CarController.enabled = true;
            aiCar.CarController.externalController = true;
        }

        GameObject[] aiCars = GameObject.FindGameObjectsWithTag("AICar");

        foreach (GameObject car in aiCars)
        {
            car.GetComponent<RCC_AICarController>().enabled = true;
        }

        GameObject[] trafficLights = GameObject.FindGameObjectsWithTag("TrafficLight");

        foreach (GameObject light in trafficLights)
        {
            light.GetComponent<HealthbarGames.TrafficLightManager>().enabled = true;
        }
    }

    public void DeactivateAI()
    {
        if (aiCar != null)
        {
            aiCar.enabled = false;
            aiCar.CarController.externalController = false;
        }
    }

    public void startDialogue()
    {
        if (Configs.isAutomated)
        {
            StartCoroutine(startAgent());
        }
    }

    IEnumerator startAgent()
    {
        yield return new WaitForSeconds(2);

        agentImage.sprite = activeSprite;

        FadeInBubble();

        VoiceSource.Play();

        yield return new WaitForSeconds(4);

        VoiceSource.clip = lumo3;
        VoiceSource.Play();
        WhyText.text = "If I notice something unusual and decide to act on it, I will notify you with a <b><color=red>red exclamation mark</b></color>.";

        yield return new WaitForSeconds(7);
        VoiceSource.clip = lumo2;
        VoiceSource.Play();
        WhyText.text = "Please <b>click on me</b> if you would like me to explain myself. \r\nI will tell you what my sensors detected and how I adjusted my behaviour in accordance to it.";

        yield return new WaitForSeconds(9);

        agentImage.sprite = idleSprite;
        fadeRoutine = StartCoroutine(FadeOutBubbleAfterDelay(1f));



        yield return new WaitForSeconds(3);

        ActivateAI();

    }

    public void ToggleUIBehaviour(Behaviour uiElement)
    {
        if (uiElement == null) return;

        if (currentAnim == null)
        {
            if (uiElement is CanvasGroup cg)
                isVisible = cg.alpha > 0.9f;
            else if (uiElement is Image img)
                isVisible = img.color.a > 0.9f;
        }

        isVisible = !isVisible;

        if (currentAnim != null) StopCoroutine(currentAnim);

        if (uiElement is CanvasGroup panel)
        {
            currentAnim = StartCoroutine(FadeFloat(panel.alpha, isVisible ? 1f : 0f, v => panel.alpha = v, () =>
            {
                panel.interactable = isVisible;
                panel.blocksRaycasts = isVisible;
            }));
        }
        else if (uiElement is Image image)
        {
            currentAnim = StartCoroutine(FadeFloat(image.color.a, isVisible ? 1f : 0f, v =>
            {
                var c = image.color;
                c.a = v;
                image.color = c;
            }));
        }
    }
    private IEnumerator FadeFloat(float start, float end, System.Action<float> Apply, System.Action OnDone = null)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float value = Mathf.Lerp(start, end, time / duration);
            Apply(value);
            yield return null;
        }

        Apply(end);
        OnDone?.Invoke();
        currentAnim = null;
    }

    // Music player scripts
    public void TogglePlay()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            playImage.sprite = pauseIcon;  // show pause icon
            audioSource.Play();
        }
        else
        {
            playImage.sprite = playIcon;   // show play icon
            audioSource.Pause();
        }
    }

    public void ToggleVolume()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            previousVolume = audioSource.volume;
            audioSource.volume = 0f;
            if (volumeImage != null) volumeImage.sprite = muteIcon;
        }
        else
        {

            audioSource.volume = previousVolume;
            if (volumeImage != null) volumeImage.sprite = volumeIcon;
        }
    }

    // --------------------------
    public void EnterZone(ZoneTrigger zone)
    {
        currentZone = zone;

        if (Configs.condition == ConditionType.Lumo)
        {
            agentButton.interactable = true;
            agentImage.sprite = alertSprite;
        }
        else
        {
            // Change sprite to ACTIVE
            agentImage.sprite = activeSprite;

            // Update bubble text
            WhyText.text = currentZone.whyText;
            WhatText.text = currentZone.whatText;

            // update audio clip
            VoiceSource.clip = currentZone.voiceClip;
            VoiceSource.Play();
            // Show bubble
            FadeInBubble();
        }
    }

    public void ExitZone()
    {
        currentZone = null;
        SetIdleState();
        FadeOutBubbleInstant();
    }
    public void OnAgentButtonClicked()
    {
        if (currentZone == null)
            return;

        // Change sprite to ACTIVE
        agentImage.sprite = activeSprite;

        // Update bubble text
        WhyText.text = currentZone.whyText;
        WhatText.text = currentZone.whatText;

        VoiceSource.clip = currentZone.voiceClip;
        VoiceSource.Play();

        // Show bubble
        FadeInBubble();

        // Hide after 5 seconds
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutBubbleAfterDelay(5f));
    }


    private void SetIdleState()
    {
        agentButton.interactable = false;
        agentImage.sprite = idleSprite;
    }

    public void FadeInBubble()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        agentSpeechBubble.alpha = 1f;
    }

    private void FadeOutBubbleInstant()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        agentSpeechBubble.alpha = 0f;
    }

    private IEnumerator FadeOutBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float t = 0f;
        float start = agentSpeechBubble.alpha;

        while (t < 1f)
        {
            t += Time.deltaTime / 1f; // 1 second fade
            agentSpeechBubble.alpha = Mathf.Lerp(start, 0f, t);
            yield return null;
        }
    }
}

