using UnityEngine;
using System.Collections;

public class TriggerB : MonoBehaviour
{

    public GameObject BreakZoneforStopping1;
    public GameObject BreakZoneforStopping2;

    [Header("Despawn Settings")]
    [Tooltip("Time in seconds before the pedestrian despawns")]
    public float despawnTime = 30f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RCC"))
        {

            StartCoroutine(DespawnBreakZone(10f));

            // Optional: Disable trigger so it doesn't activate again
            GetComponent<Collider>().enabled = false;

            Debug.Log("RCC Car Activated!");
        }
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
