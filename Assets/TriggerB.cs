using UnityEngine;
using System.Collections;

public class TriggerB : MonoBehaviour
{

    public GameObject BreakZoneforStopping1;
    public GameObject BreakZoneforStopping2;

    public GameObject targetpedestrian;
    [Header("Despawn Settings")]
    [Tooltip("Time in seconds before the pedestrian despawns")]
    public float despawnTime = 30f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RCC"))
        {
            StartCoroutine(DespawnAfterTime(11f));
            StartCoroutine(DespawnBreakZone(15f));

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

    private IEnumerator DespawnAfterTime(float time)
    {

        if (targetpedestrian != null)
            targetpedestrian.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (targetpedestrian != null)
            targetpedestrian.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        if (targetpedestrian != null)
            targetpedestrian.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        if (targetpedestrian != null)
            targetpedestrian.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        if (targetpedestrian != null)
            targetpedestrian.SetActive(true);

        yield return new WaitForSeconds(time);

        if (targetpedestrian != null)
            targetpedestrian.SetActive(false);


        Debug.Log("targetpedestrian despawned");
    }

}
