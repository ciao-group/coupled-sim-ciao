using System;
using System.IO;
using TMPro;
using Unity.Properties;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.UI;
public enum ConditionType
{
    Nevo,
    Coda,
    Lumo
}

public enum RouteType
{
    A,
    B,
    C,
    T
}

public class ExperimentConfigs : MonoBehaviour
{

    [Header("Experiment")]
    [TextArea] public string participant_ID;

    public bool isAutomated = true;
    public bool isLogging = true;

    public Vector3 startPosition;
    public Quaternion startRotation;

    [Header("Choose Condition")]
    public ConditionType condition;

    [Header("Choose Route")]
    public RouteType route;


    [Header("Route A")]
    public GameObject routeA;
    public RCC_AIWaypointsContainer wpRouteA;
    public Vector3 routeAStartPos;
    public Vector3 routeAStartRot;

    [Header("Route B")]
    public GameObject routeB;
    public RCC_AIWaypointsContainer wpRouteB;
    public Vector3 routeBStartPos;
    public Vector3 routeBStartRot;

    [Header("Route C")]
    public GameObject routeC;
    public RCC_AIWaypointsContainer wpRouteC;
    public Vector3 routeCStartPos;
    public Vector3 routeCStartRot;

    [Header("Route T")]
    public GameObject routeT;
    public RCC_AIWaypointsContainer wpRouteT;
    public Vector3 routeTStartPos;
    public Vector3 routeTStartRot;


    [Header("Lumo")]
    public Sprite lumoIdleSprite;
    public Sprite lumoActiveSprite;
    public Sprite lumoAlertSprite;

    [Header("Coda")]
    public Sprite codaIdleSprite;
    public Sprite codaActiveSprite;
    public Sprite codaAlertSprite;

    [Header("Nevo")]
    public Sprite nevoIdleSprite;
    public Sprite nevoActiveSprite;
    public Sprite nevoAlertSprite;

    public IVISLogic ivisLogic;
    public GameObject PlayerCar;


    public ZoneDetector zoneDetector;
    public GameObject TrajectoryLine;

    private string logPath;

    void Start()
    {

        if (isAutomated)
        {
            ApplyRoute();
            ApplyCondition();

        } else
        {
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

        if (isLogging)
        {

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logDirectory = "C:\\Users\\ciaos\\Desktop\\Logs\\";
            string fileName = $"button_log_{timestamp}.csv";
            logPath = $"{logDirectory}{timestamp}";
            Debug.Log("Logging to: " + logPath);


            if (!File.Exists(logPath) && AssetDatabase.IsValidFolder(logPath))
            {
                File.WriteAllText(logPath, "participant_id,route,condition,zone,timestamp,button\n");
                Debug.Log("=== Button Log Started ===");
            }
            else { Debug.Log("Filename already exists."); }

            if (TrajectoryLine != null)
            {
                int length = PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer.waypoints.Count;

                TrajectoryLine.GetComponent<TrajectoryPainter>().WPIndexDestination = length;
                //Debug.Log(length);
            }
        }
    }


    public void LogButtonPress(string buttonName)
    {
        if (isLogging)
        { 
        string line = $"{participant_ID},{route},{condition},{zoneDetector.currentZone},{Time.time:F4},{buttonName}\n";
        Debug.Log(line);
        File.AppendAllText(logPath, line);
        }
    }


    public void ApplyCondition()
    {
        switch (condition)
        {
            case ConditionType.Lumo:
                AssignSprites(lumoIdleSprite, lumoActiveSprite, lumoAlertSprite, "Lumo", "#31edae");
                break;
            case ConditionType.Coda:
                AssignSprites(codaIdleSprite, codaActiveSprite, codaAlertSprite, "Coda", "#5170ff");
                break;
            case ConditionType.Nevo:
                AssignSprites(nevoIdleSprite, nevoActiveSprite, nevoAlertSprite, "Nevo", "#ffbd59");
                break;
        }
    }

    public void ApplyRoute()
    {
        switch (route)
        {
            case RouteType.A:
                routeB.SetActive(false);
                routeC.SetActive(false);
                routeT.SetActive(false);
                routeA.SetActive(true);
                startPosition = routeAStartPos;
                startRotation = Quaternion.Euler(routeAStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteA;
                break;
            case RouteType.B:
                routeA.SetActive(false);
                routeT.SetActive(false);
                routeC.SetActive(false);
                routeB.SetActive(true);
                startPosition = routeBStartPos;
                startRotation = Quaternion.Euler(routeBStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteB;
                break;
            case RouteType.C:
                routeA.SetActive(false);
                routeB.SetActive(false);
                routeT.SetActive(false);
                routeC.SetActive(true);
                startPosition = routeCStartPos;
                startRotation = Quaternion.Euler(routeCStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteC;
                break;
            case RouteType.T:
                routeA.SetActive(false);
                routeB.SetActive(false);
                routeC.SetActive(false);
                routeT.SetActive(true);
                startPosition = routeTStartPos;
                startRotation = Quaternion.Euler(routeTStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteT;
                break;
        }
    }

    private void AssignSprites(Sprite idle, Sprite active, Sprite alert, String name, String hue)
    {
        if (ivisLogic != null)
        {
            ivisLogic.idleSprite = idle;
            ivisLogic.activeSprite = active;
            ivisLogic.alertSprite = alert;

            ivisLogic.WhyText.text = $"<b><size=28>Welcome!</b></size>\nMy name is <b><color={hue}>{name}</color></b> and I will be your driver today.";
        }
        else
        {
            Debug.LogWarning("IvisLogic ref not assigned");
        }
    }
}
