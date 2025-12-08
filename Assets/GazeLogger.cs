/*using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using Varjo.XR;

public class GazeLogger : MonoBehaviour
{

    // Varjo native API call for requesting calibration
    [DllImport("varjo")]
    private static extern void varjo_RequestGazeCalibration(IntPtr session);
    private System.Collections.Generic.List<InputDevice> devices = new System.Collections.Generic.List<InputDevice>();
    private InputDevice device;
    private System.Collections.Generic.List<VarjoEyeTracking.GazeData> dataSinceLastUpdate;
    private System.Collections.Generic.List<VarjoEyeTracking.EyeMeasurements> eyeMeasurementsSinceLastUpdate;
    // Variables to store the latest gaze positions
    private Vector3 latestIntersectionPoint = Vector3.zero;
    private bool hasValidGaze = false;

    // Variables to store the last known pixel coordinates
    private int lastPixelX = 0;
    private int lastPixelY = 0;
    public string fileName = "Log_Gaze";
    public bool logtoconsole = true;

    private string filePath;

    private StreamWriter writer;

    // Start is called before the first frame update

    private void Awake()
    {
        GetDevice();
    }
    void Start()
    {
        filePath = Path.Combine(Application.dataPath, fileName);

        writer = new StreamWriter(filePath);
        writer.AutoFlush = true;

        writer.WriteLine(unityTime, SystemTime, gazeDirX, gazeDirY, gazeDirZ, focusDistance, leftStatus, rightStatus);

        Debug.Log("Started");


    }

    void OnEnable()
    {
        // Subscribe to data received event for both Eye Tracking and Arduino
        if (_inputManager != null)
        {
            _inputManager.OnDataReceived += LogData;
        }
        else
        {
            Debug.LogError("InputManager not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 dir = Vector3.zero;
        double sysTime = 0.0;
        float focusDist = 0f;
        string leftStatus = "Unknown";
        string rightStatus = "Unknown";

        string line = string.Format("{0:F4},{1:F6},{2:F6},{3:F6},{4:F6},{5:F4},{6},{7}",
            Time.time,
            sysTime,
            dir.x,dir.y,dir.z,
            focusDist,
            leftStatus,
            rightStatus);


        writer.WriteLine(line);

        if (logtoconsole)
        {
            Debug.Log(line);
        }

    }
    void GetDevice()
    {
        // Get the center eye device
        InputDevices.GetDevicesAtXRNode(XRNode.CenterEye, devices);
        device = devices.FirstOrDefault();
    }

    private void OnApplicationQuit()
    {
        if (writer != null) {
            writer.Flush();
            writer.Close();
            writer = null;
        }
    }
}
*/