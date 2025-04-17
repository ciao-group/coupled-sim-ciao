using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Globalization;

public class DashboardGUI : MonoBehaviour
{

    public RCC_CarControllerV3 car;
    public TextMeshProUGUI speedText;

    public TextMeshProUGUI gearText;
    public TextMeshProUGUI gearTextSmall;

    public TextMeshProUGUI steerText;

    public Image throttleBar;
    public Image brakeBar;
    public float transitionSpeed = 5f;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float throttleValue = car.throttleInput;
        float brakeValue = car.brakeInput;

        float steeringAngle = Mathf.RoundToInt(car.steerInput * 360);

        string[] allGears = { "P", "R", "N", "D" };

        if (car != null && speedText != null && gearText.text != null && gearTextSmall.text != null)
        {
            speedText.text = Mathf.Round(car.speed).ToString() + " km/h";

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

            throttleBar.fillAmount = Mathf.Lerp(throttleBar.fillAmount, throttleValue, Time.deltaTime * transitionSpeed);
            brakeBar.fillAmount = Mathf.Lerp(brakeBar.fillAmount, brakeValue, Time.deltaTime * transitionSpeed);


        }


        DateTime currentTime = DateTime.Now;
        CultureInfo english = new CultureInfo("en-US");

        string dayAbbrev = currentTime.ToString("ddd", english);
        string monthDay = currentTime.ToString("MMM d", english);

        string hourMinute = currentTime.ToString("h:mm", english);
        string ampm = currentTime.ToString("tt", english).ToLower();

        string styledDate = $"{dayAbbrev} <#ffffff99>{monthDay}</color>";
        string styledTime = $"{hourMinute}<size=60%><#ffffff99>{ampm}</color></size>";

        dateText.text = styledDate;
        timeText.text = styledTime;

    }
}
