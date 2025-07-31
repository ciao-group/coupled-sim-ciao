using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

[RequireComponent(typeof(RCC_CarControllerV3))]
public class RCC_SteerAngleSender : MonoBehaviour
{
    [Header("TCP target (must match UpperComputer)")]
    public string host = "127.0.0.1";
    public int port = 6000;

    [Header("Send interval (s) — 0.02 ≈ every FixedUpdate at 50 Hz")]
    [Range(0.01f, 0.2f)]
    public float sendInterval = 0.01f;

    private TcpClient client;
    private StreamWriter writer;
    private float sendTimer;
    private float reconnectTimer;

    private float connectionAttemptTime = 0f; // ⬅️ für das 10-Sekunden-Timeout

    private RCC_CarControllerV3 car;

    void Awake() => car = GetComponent<RCC_CarControllerV3>();

    void Start() => TryConnect();

    void FixedUpdate()
    {
        if (client == null || !client.Connected)
        {
            reconnectTimer += Time.fixedDeltaTime;
            connectionAttemptTime += Time.fixedDeltaTime;

            if (connectionAttemptTime >= 10f)
            {
                Debug.LogWarning("[SteerSender] No connection after 10 seconds. Disabling script.");
                this.enabled = false; // ⬅️ Script deaktivieren
                return;
            }

            if (reconnectTimer >= 2f)
            {
                reconnectTimer = 0f;
                TryConnect();
            }
            return;
        }

        // Reset the connectionAttemptTime once connected
        connectionAttemptTime = 0f;

        sendTimer += Time.fixedDeltaTime;
        if (sendTimer < sendInterval) return;
        sendTimer = 0f;

        float maxAtThisSpeed = car.steerAngleCurve.Evaluate(car.speed);
        float steerDeg = car.steerInput * maxAtThisSpeed;

        try
        {
            writer.WriteLine(steerDeg.ToString("F3"));
            writer.Flush();
        }
        catch (Exception)
        {
            CloseSocket();
        }
    }

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
