using UnityEngine;


// we need this script because otherwise unity does not recognise the additional displays
public class ActivateAllDisplays : MonoBehaviour
{
    [Header("Display Cameras")]
    public Camera display3Camera; // Camera for Display 3 (cockpit)
    public Camera display4Camera; // Camera for Display 4 (tablet)

    void Start()
    {

        Debug.Log("Displays connected: " + Display.displays.Length);
        ActivateDisplays();
        AssignCamerasToDisplays();
    }

    private void ActivateDisplays()
    {

        for (int i = 1; i < Display.displays.Length; i++)
        {
            if (!Display.displays[i].active)
            {
                Display.displays[i].Activate();
                Debug.Log("Display " + i + " activated.");
            }
        }
    }

    private void AssignCamerasToDisplays()
    {
        if (display3Camera != null && display3Camera.targetDisplay != 2)
        {
            display3Camera.targetDisplay = 2;
            Debug.Log("Display 3 camera assigned to Display 2.");
        }

        if (display4Camera != null && display4Camera.targetDisplay != 3)
        {
            display4Camera.targetDisplay = 3;
            Debug.Log("Display 4 camera assigned to Display 3.");
        }
    }
}
