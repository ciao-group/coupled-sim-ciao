using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
[RequireComponent(typeof(RCC_CarControllerV3))]
public class RCC_SteerAngleSender : MonoBehaviour
{
    /* -------- inspector ------------------------------------------------- */
    [Header("TCP target (must match UpperComputer)")]
    public string host = "127.0.0.1";
    public int port = 6000;
    [Header("Send interval (s) — 0.02 ≈ every FixedUpdate at 50 Hz")]
    [Range(0.01f, 0.2f)]
    public float sendInterval = 0.01f;
    /* -------- internals -------------------------------------------------- */
    private TcpClient client;
    private StreamWriter writer;
    private float timer;
    private RCC_CarControllerV3 car;
    /* ===================================================================== */
    void Awake() => car = GetComponent<RCC_CarControllerV3>();
    void Start() => TryConnect();
    void FixedUpdate()
    {
        /* 1) keep socket alive / reconnect */
        if (client == null || !client.Connected)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= 2f)      // retry every 2 s
            {
                timer = 0f;
                TryConnect();
            }
            return;
        }
        /* 2) respect send interval */
        timer += Time.fixedDeltaTime;
        if (timer < sendInterval) return;
        timer = 0f;
        /* 3) pick the angle you want to transmit -------------------------- */
        // A) mathematical – steering input * high-speed limit (often enough)
        float maxAtThisSpeed = car.steerAngleCurve.Evaluate(car.speed);
        float steerDeg = car.steerInput * maxAtThisSpeed;
        /* 4) send “<angle>\n” -------------------------------------------- */
        try
        {
            writer.WriteLine(steerDeg.ToString("F3"));
            writer.Flush();
        }
        catch (Exception) // peer closed – drop socket and retry later
        {
            CloseSocket();
        }
    }
    /* ===================================================================== */
    void TryConnect()
    {
        try
        {
            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(host, port);
            writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
            Debug.Log($"[SteerSender] Connected to {host}:{port}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[SteerSender] Connect failed → {ex.Message}");
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