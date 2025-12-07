using System;
using System.IO;
using TMPro;
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
    C
}

public class ExperimentConfigs : MonoBehaviour
{

    [Header("Experiment")]
    [TextArea] public string participant_ID;

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
        ApplyRoute();
        ApplyCondition();

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logPath = $"C:\\Users\\ciaos\\Desktop\\Logs\\button_log_{timestamp}.csv";
        Debug.Log("Logging to: " + logPath);


        if (!File.Exists(logPath))
        {
            File.WriteAllText(logPath, "participant_id,condition,zone,timestamp,button\n");
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


    public void LogButtonPress(string buttonName)
    {
        string line = $"{participant_ID},{condition},{zoneDetector.currentZone},{Time.time:F4},{buttonName}\n";
        Debug.Log(line);
        File.AppendAllText(logPath, line);

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
                routeA.SetActive(true);
                startPosition = routeAStartPos;
                startRotation = Quaternion.Euler(routeAStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteA;
                break;
            case RouteType.B:
                routeA.SetActive(false);
                routeC.SetActive(false);
                routeB.SetActive(true);
                startPosition = routeBStartPos;
                startRotation = Quaternion.Euler(routeBStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteB;
                break;
            case RouteType.C:
                routeA.SetActive(false);
                routeB.SetActive(false);
                routeC.SetActive(true);
                startPosition = routeCStartPos;
                startRotation = Quaternion.Euler(routeCStartRot);
                PlayerCar.GetComponent<RCC_AICarController>().waypointsContainer = wpRouteC;
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

            ivisLogic.WhyText.text = $"My name is <b><color={hue}>{name}</color></b> and I will be your driver today.";
        }
        else
        {
            Debug.LogWarning("IvisLogic ref not assigned");
        }
    }
}
