using UnityEngine;

public class SmoothDroneFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform car;

    public Vector3 localOffset = new Vector3(0f, 10f, -20f);

    [Header("Smoothing")]
    [Tooltip("Higher = tighter follow, Lower = more cinematic lag")]
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (car == null) return;

        Quaternion yawOnly = Quaternion.Euler(0f, car.eulerAngles.y, 0f);
        Vector3 startPos = car.position + yawOnly * localOffset;

        transform.position = startPos;
    }

    void LateUpdate()
    {
        if (car == null) return;


        Quaternion yawOnly = Quaternion.Euler(0f, car.eulerAngles.y, 0f);


        Vector3 targetPos = car.position + yawOnly * localOffset;


        // Smoothly catch up (low-pass filter)
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);


    }
}
