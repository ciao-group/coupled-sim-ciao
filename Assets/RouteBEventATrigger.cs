using UnityEngine;
using System.Collections;

public class RouteBEventATrigger : MonoBehaviour
{

    [Header("Brakezones to simulate stopping behaviour (will deactivate)")]
    public GameObject BrakeZoneforStopping1;
    public GameObject BrakeZoneforStopping2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RCC"))
        {
            // despawn the brake zones after some hesitation
            StartCoroutine(DespawnBrakeZone(25f));
            // disable trigger 
            GetComponent<Collider>().enabled = false;

            Debug.Log("Triggered Event A");
        }
    }

    private IEnumerator DespawnBrakeZone(float time)
    {
        yield return new WaitForSeconds(time);

        if (BrakeZoneforStopping1 != null && BrakeZoneforStopping2 != null)
            BrakeZoneforStopping1.SetActive(false);
            BrakeZoneforStopping2.SetActive(false);

        Debug.Log("Event A brake zones despawned");
    }

}
