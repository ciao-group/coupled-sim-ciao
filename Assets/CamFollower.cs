using UnityEngine;

public class CanvasFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Assign this in the Inspector
    public Vector3 offset = new Vector3(0, 0, -0.1f); // Default offset is 10cm forward

    void Update()
    {
        if (mainCamera != null)
        {
            // Apply the offset to position the canvas correctly
            transform.position = mainCamera.transform.position + mainCamera.transform.TransformDirection(offset);
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
