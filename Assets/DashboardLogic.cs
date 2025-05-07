using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Globalization;

public class DashboardGUI : MonoBehaviour
{
    public IconFader[] iconFaders;

    public RCC_CarControllerV3 car;

    [Header("Leftside Elements")]
    public Image throttleBar;
    public Image brakeBar;
    [Tooltip("Big letter showing current gear.")]
    public TextMeshProUGUI gearText;
    [Tooltip("Three small letters showing the other gears.")]
    public TextMeshProUGUI gearTextSmall;
    [Space(10)]
    [Tooltip("Adapts smoothness of bars rising/ decreasing.")]
    [Range(0f, 10f)]
    public float barSmoothness = 5f;

    [Header("Center Elements")]
    // shows current steering angle of wheel
    public TextMeshProUGUI steerText;
    public TextMeshProUGUI speedTextBig;

    [Header("Top Elements")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI tempText;

    [Header("Rightside Elements")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI timeElapsedText;
    public TextMeshProUGUI DistanceText;
    public TextMeshProUGUI ConsText;


    private float elapsedTime = 0f;
    private float distanceTravelled = 0f;
    private Vector3 lastPosition;
    private bool hasStarted = false;


    void Start()
    {
        if (car != null)
        {
            lastPosition = car.transform.position;
        }

    }

    void Update()
    {


        if (!hasStarted && car.speed > 1f)
        {
            hasStarted = true;
            elapsedTime = 0f;
            distanceTravelled = 0f;
            lastPosition = car.transform.position;
        }

        if (true) // hasStarted
        {

            elapsedTime += Time.deltaTime;
            // calc positional difference
            float deltaDistance = Vector3.Distance(car.transform.position, lastPosition);
            distanceTravelled += deltaDistance;
            lastPosition = car.transform.position;

            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;

            if (hours > 0)
                timeElapsedText.text = $"{hours} <font=\"DinLight_Glow\"><size=60%>hour{(hours > 1 ? "s" : "")}</size></font> {minutes} <font=\"DinLight_Glow\"><size=60%>min</size></font>";
            else
                timeElapsedText.text = $"{minutes} <font=\"DinLight_Glow\"><size=60%>min</size></font>";

            DistanceText.text = (distanceTravelled / 1000f).ToString("F2") + " <font=\"DinLight_Glow\"><size=60%>km/h</size></font>"; // 2 decimal points
        }



        float throttleValue = car.throttleInput;
        float brakeValue = car.brakeInput;

        float steeringAngle = Mathf.RoundToInt(car.steerInput * 360);

        string[] allGears = { "P", "R", "N", "D" };

        if (car != null && speedText != null && gearText.text != null && gearTextSmall.text != null)
        {
            speedText.text = Mathf.Round(car.speed).ToString() + " <font=\"DinLight_Glow\"><size=60%>km/h</size></font>";
            speedTextBig.text = Mathf.Round(car.speed).ToString();

            string currentGear = RCC_InputManager.Instance.GearMode.ToString().Substring(0, 1);
            gearText.text = currentGear;
            string smallGears = "";

            foreach (string gear in allGears)
            {
                if (gear != currentGear)
                {
                    smallGears += gear;
                }
            }

            gearTextSmall.text = smallGears.Trim();

            steerText.text = (steeringAngle).ToString() + "°";

            throttleBar.fillAmount = Mathf.Lerp(throttleBar.fillAmount, throttleValue, Time.deltaTime * barSmoothness);
            brakeBar.fillAmount = Mathf.Lerp(brakeBar.fillAmount, brakeValue, Time.deltaTime * barSmoothness);

        }


        DateTime currentTime = DateTime.Now;
        // defaults to German time layout, so converting to US
        CultureInfo english = new CultureInfo("en-US");

        string dayAbbrev = currentTime.ToString("ddd", english); // fe. Mon
        string monthDay = currentTime.ToString("MMM d", english); // fe. Feb 21

        string hourMinute = currentTime.ToString("h:mm", english); // fe. 3:23
        string ampm = currentTime.ToString("tt", english).ToLower(); // fe. am
        // adding colour and size stylisations
        string styledDate = $"{dayAbbrev} <#ffffff99>{monthDay}</color>";
        string styledTime = $"{hourMinute}<size=60%><#ffffff99>{ampm}</color></size>";

        dateText.text = styledDate;
        timeText.text = styledTime;

    }
}
