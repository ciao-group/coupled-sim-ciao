using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CockpitGUI : MonoBehaviour
{

    public RCC_CarControllerV3 car;
    public TextMeshProUGUI speedText;

    public TextMeshProUGUI gearText;
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

        if (car != null && speedText != null)
        {
            speedText.text = Mathf.Round(car.speed).ToString() + " km/h";

            gearText.text = (RCC_InputManager.Instance.GearMode).ToString().Substring(0, 1);
            steerText.text = (steeringAngle).ToString() + "°";

            throttleBar.fillAmount = Mathf.Lerp(throttleBar.fillAmount, throttleValue, Time.deltaTime * transitionSpeed);
            brakeBar.fillAmount = Mathf.Lerp(brakeBar.fillAmount, brakeValue, Time.deltaTime * transitionSpeed);

        }


        DateTime currentTime = DateTime.Now;

        string formattedTime = currentTime.ToString("h:mm tt");
        string formattedDate = currentTime.ToString("ddd MMM d");

        timeText.text = formattedTime;
        dateText.text = formattedDate;
    }
}
