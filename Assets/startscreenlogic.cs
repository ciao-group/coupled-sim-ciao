using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class startscreenlogic : MonoBehaviour
{

    public ExperimentConfigs configs;

    public TMP_Text arrivalText;
    public TMP_Text greetingText;
    public TMP_Text destinationText;

    // Start is called before the first frame update
    void Start()
    {
        DateTime now = DateTime.Now;
        greetingText.text = GetGreeting(now);
        DateTime timePlusFive = now.AddMinutes(5);
        arrivalText.text = timePlusFive.ToString("HH:mm");
        destinationText.text = GetDestination(configs.route);
    }
    string GetGreeting(DateTime time)
    {
        int hour = time.Hour;

        if (hour < 12)
            return "Good morning.";
        else if (hour < 18)
            return "Good afternoon.";
        else
            return "Good evening.";
    }

    string GetDestination(RouteType route)
    {
        switch (route)
        {
            case RouteType.A:
                return "Drop-Off Point A";
            case RouteType.B:
                return "Drop-Off Point B";
            case RouteType.C:
                return "Drop-Off Point C";
            case RouteType.T:
                return "Drop-Off Point T";
            default:
                return "Drop-Off Point";
        }
    }
}
