using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

[RequireComponent(typeof(RCC_CarControllerV3))]
public class RCCSteerAngleSender : MonoBehaviour
{
    [Header("TCP target (must match UpperComputer)")]
    public string host = "127.0.0.1";
    public int port = 6000;
    [Header("Send interval (s) — 0.02 ≈ 50 Hz")]
    [Range(0.01f, 0.2f)]
    public float sendInterval = 0.05f;
    /* ---------------------------------------------------------- */
    private RCC_CarControllerV3 car;
    private TcpClient client;
    private StreamWriter writer;
    private float timer;
    /* ---------------------------------------------------------- */
    void Awake() => car = GetComponent<RCC_CarControllerV3>();
    void Start() => TryConnect();
    void Update()
    {
        /* 1) Keep socket alive / reconnect */
        if (client == null || !client.Connected)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= 2f)           // retry every 2 s
            {
                timer = 0f;
                TryConnect();
            }
            return;
        }
        /* 2) Throttle send rate */
        timer += Time.deltaTime;
        if (timer < sendInterval) return;
        timer = 0f;
        /* 3) Grab true wheel-collider angles (degrees) */
        float fl = car.FrontLeftWheelCollider.WheelCollider.steerAngle;
        float fr = car.FrontRightWheelCollider.WheelCollider.steerAngle;
        float steerDeg = 0.5f * (fl + fr);
        /* 4) Send “<angle>\n” */
        try
        {
            writer.WriteLine(steerDeg.ToString("F3"));
            writer.Flush();            // push immediately
        }
        catch (Exception)
        {
            CloseSocket();             // will reconnect next frame
        }
    }
    /* ---------------------------------------------------------- */
    void TryConnect()
    {
        try
        {
            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(host, port);
            writer = new StreamWriter(client.GetStream());
            Debug.Log($"[RCCSteerAngleSender] Connected to {host}:{port}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[RCCSteerAngleSender] Connect failed: {ex.Message}");
            CloseSocket();
        }
    }
    void CloseSocket()
    {
        writer?.Close();
        client?.Close();
        writer = null;
        client = null;
    }
    void OnApplicationQuit() => CloseSocket();
}
