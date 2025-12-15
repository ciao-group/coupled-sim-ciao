using HealthbarGames;
using UnityEngine;
using UnityEngine.Splines;



public class SplineFollower : MonoBehaviour
{
    public SplineContainer spline;
    public float maxSpeed = 6f;
    public float acceleration = 2f;
    public float braking = 4f;

    private float t = 0f;
    private float splineLength;
    private float currentSpeed = 0f;

    public Transform frontLeft;
    public Transform frontRight;
    public Transform rearLeft;
    public Transform rearRight;


    public float WheelDiameter = 0.65f;

    private float RotationSpeed;

    private CrosswalkZone currentCrosswalkZone;
    private Transform stopLineTarget;

    private bool inCrosswalkZone = false;

    private float distanceToStopLine = Mathf.Infinity;

    private bool mustStopForLight = false;

    private bool mustStopForCar = false;

    private bool shouldStop = false;

    void Start()
    {
        if (spline != null)
            splineLength = spline.CalculateLength();
    }

    void Update()
    {
        if (spline == null) return;

        // Apply stop/go logic
        if (shouldStop)
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, braking * Time.deltaTime);
        else
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        // Move along the spline
        t += (currentSpeed * Time.deltaTime) / splineLength;

        float normalized = t % 1f;

        Vector3 pos = spline.EvaluatePosition(normalized);
        Vector3 tan = spline.EvaluateTangent(normalized);

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(tan);

        WheelRotater(frontLeft, frontRight, rearLeft, rearRight);

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("CrosswalkZone"))
        {
            currentCrosswalkZone = other.GetComponent<CrosswalkZone>();
            if (currentCrosswalkZone != null)
            {
                stopLineTarget = currentCrosswalkZone.stopLine;
                inCrosswalkZone = true;
            }
        }
        
        if (inCrosswalkZone && currentCrosswalkZone != null)
        {
            var phaseState = currentCrosswalkZone.GetPhase().GetState();

            shouldStop = (phaseState == TrafficLightBase.State.Stop ||
            phaseState == TrafficLightBase.State.PrepareToStop);
        }
        else
        {
            shouldStop = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CrosswalkZone"))
            shouldStop = false;
            inCrosswalkZone = false;
    }


    private void WheelRotater(Transform frontLeft, Transform frontRight,Transform rearLeft, Transform rearRight)
    {
        RotationSpeed = 360f * currentSpeed / 3.6f / Mathf.PI / WheelDiameter;

        //Front Left
        frontLeft.transform.Rotate(RotationSpeed * Time.deltaTime, 0, 0);
        //Front Right
        frontRight.transform.Rotate(RotationSpeed * Time.deltaTime, 0, 0);
        //Rear Left
        rearLeft.transform.Rotate(RotationSpeed * Time.deltaTime, 0, 0);
        //Rear Right
        rearRight.transform.Rotate(RotationSpeed * Time.deltaTime, 0, 0);

    }
}
