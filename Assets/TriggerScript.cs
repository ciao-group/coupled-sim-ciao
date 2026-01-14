using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerScript : MonoBehaviour
{
    [Header("Truck that pulls into the street")]
    public GameObject targetCar;
    public GameObject BreakZoneforStopping;

    private RCC_AICarController aiController;
    private RCC_CarControllerV3 carController;

    [Header("Despawn Settings")]
    [Tooltip("Time in seconds before the car despawns")]
    public float despawnTime = 30f;
    [SerializeField] private InputAction indicatorRightAction;

    private void Start()
    {
        if (targetCar != null)
        {
            // Get RCC components from the car
            aiController = targetCar.GetComponent<RCC_AICarController>();
            carController = targetCar.GetComponent<RCC_CarControllerV3>();

            if (aiController == null)
                Debug.LogWarning("No RCC_AICarController found on targetCar!");
            else
                aiController.enabled = false; // Disable at start

            if (carController == null)
                Debug.LogWarning("No RCC_CarControllerV3 found on targetCar!");
            else
                carController.enabled = false; // Disable at start
        }
        else
        {
            Debug.LogWarning("Target car not assigned!");
        }

        indicatorRightAction.WasPressedThisFrame();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (targetCar != null && other.CompareTag("Player"))
        {
            // Activate RCC Car Controller
            if (carController != null)
                carController.enabled = true;
            // Activate RCC AI
            if (aiController != null)
                aiController.enabled = true;

            // Start despawn countdown
            StartCoroutine(DespawnCarAfterTime(despawnTime));
            // Start despawn countdown
            StartCoroutine(DespawnBreakZone(5f));

            // Optional: Disable trigger so it doesn't activate again
            GetComponent<Collider>().enabled = false;

            Debug.Log("RCC Car Activated!");
        }
    }


    private IEnumerator DespawnCarAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (targetCar != null)
            Destroy(targetCar);

        Debug.Log("RCC Car Despawned!");
    }

    private IEnumerator DespawnBreakZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BreakZoneforStopping != null)
            BreakZoneforStopping.SetActive(false);

        Debug.Log("No more break zone!");
    }

}
