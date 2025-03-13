using UnityEngine;
using System.Collections;

public class ActivateAllDisplays : MonoBehaviour
{
    public Camera display3Camera; // Kamera für Display 3 (cockpit)
    public Camera display4Camera; // Kamera für Display 4 (tablet)
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        Display.displays[2].Activate();
        Display.displays[3].Activate();

        if (display3Camera != null)
        {
            display3Camera.targetDisplay = 2;
        }
        if (display4Camera != null)
        {
            display4Camera.targetDisplay = 3;
        }

    }

    void Update()
    {

    }
}