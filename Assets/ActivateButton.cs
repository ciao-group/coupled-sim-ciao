using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateButton : MonoBehaviour
{
    [SerializeField] Button myButton;

    void Start()
    {
        myButton.interactable = false;
        StartCoroutine(EnableButtonAfterDelay(5f));
    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        myButton.interactable = true;
    }
}
