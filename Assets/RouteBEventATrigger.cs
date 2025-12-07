using UnityEngine;
using System.Collections;

public class RouteBEventATrigger : MonoBehaviour
{
    [Header("Pedestrian that crosses the street")]
    public GameObject targetpedestrian;
    [Header("Brakezones to simulate stopping behaviour (will deactivate)")]
    public GameObject BrakeZoneforStopping1;
    public GameObject BrakeZoneforStopping2;

    private Animator animator;
    private AIPedestrian aipedestrian;

    [Header("Time in seconds before the pedestrian despawns")]
    public float despawnTime = 30f;

    private void Start()
    {
        if (targetpedestrian != null)
        {
            // Get RCC components from the car
            animator = targetpedestrian.GetComponent<Animator>();
            aipedestrian = targetpedestrian.GetComponent<AIPedestrian>();

            if (aipedestrian == null)
                Debug.LogWarning("No aipedestrian found on targetpedestrian");
            else
                aipedestrian.enabled = false;

            if (animator == null)
                Debug.LogWarning("No animator found on pedestrian");
            else
                animator.enabled = false;
        }
        else
        {
            Debug.LogWarning("no target pedestrian assigned");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetpedestrian != null && other.gameObject.layer == LayerMask.NameToLayer("RCC"))
        {
            // Activate
            if (aipedestrian != null)
                aipedestrian.enabled = true;
            if (animator != null)
                animator.enabled = true;

            // despawn the pedestrian after the event
            StartCoroutine(DespawnAfterTime(despawnTime));
            // despawn the brakezones after the pedestrian crossed
            StartCoroutine(DespawnBrakeZone(10f));
            // disable trigger 
            GetComponent<Collider>().enabled = false;

            Debug.Log("Triggered Event A");
        }
    }

    private IEnumerator DespawnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (targetpedestrian != null)
            Destroy(targetpedestrian);

        Debug.Log("Event A pedestrian despawned");
    }

    private IEnumerator DespawnBrakeZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BrakeZoneforStopping1 != null && BrakeZoneforStopping2 != null)
            BrakeZoneforStopping1.SetActive(false);
            BrakeZoneforStopping2.SetActive(false);

        Debug.Log("Event A brakezones despawned");
    }

}
