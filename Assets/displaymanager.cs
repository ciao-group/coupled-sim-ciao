using UnityEngine;

public class DisplaySetup : MonoBehaviour
{
    void Start()
    {
        // Log the number of displays detected
        Debug.Log("Number of Displays: " + Display.displays.Length);

        // Check if Display 2 exists and activate it
        if (Display.displays.Length > 1)
        {
            // Activate Display 2
            Display.displays[1].Activate();
            Debug.Log("Display 2 Activated");  // Log message to confirm activation
        }
        else
        {
            Debug.Log("Display 2 is not available");  // Log message if Display 2 is not found
        }


        Debug.Log("Camera setup: Display 1 (Main) and Display 2 (Second screen)");
    }
}
