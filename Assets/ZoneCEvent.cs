using UnityEngine;
using System.Collections;

public class ZoneCEvent : MonoBehaviour
{
    [Header("Pedestrian that crosses the street")]
    public GameObject targetpedestrian;
    public GameObject BreakZoneforStopping1;
    public GameObject BreakZoneforStopping2;

    private Animator animator;
    private AIPedestrian aipedestrian;

    [Header("Despawn Settings")]
    [Tooltip("Time in seconds before the pedestrian despawns")]
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



            StartCoroutine(DespawnAfterTime(despawnTime));

            StartCoroutine(DespawnBreakZone(10f));

            // Optional: Disable trigger so it doesn't activate again
            GetComponent<Collider>().enabled = false;

            Debug.Log("RCC Car Activated!");
        }
    }

    private IEnumerator DespawnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (targetpedestrian != null)
            Destroy(targetpedestrian);

        Debug.Log("targetpedestrian despawned");
    }

    private IEnumerator DespawnBreakZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BreakZoneforStopping1 != null && BreakZoneforStopping2 != null)
            BreakZoneforStopping1.SetActive(false);
            BreakZoneforStopping2.SetActive(false);

        Debug.Log("No more break zone!");
    }

}
