/* using UnityEngine;
using Varjo.XR;
// Make sure to include the Varjo plugin namespace(s) used for MR access
using Varjo;         // For Varjo-specific classes and enums
using Varjo.MR;      // For MR-specific methods

public class EyeReprojectionController : MonoBehaviour
{
    // Range attribute makes the slider appear in the Inspector
    [Range(0f, 2f)]
    public float reprojectionDistance = 1.0f;

    // This example assumes you're manually controlling the locking/unlocking
    // and property-setting in Update(). You could do this elsewhere too.
    void Update()
    {
        // Lock the camera configuration so we can make changes
        bool locked = VarjoPluginMR.LockCameraConfig();
        if (!locked)
        {
            // If we can't lock, another process might be holding it,
            // so we just return.
            return;
        }

        // Set eye reprojection to Manual mode
        // (the plugin calls mirror varjo_MRSetCameraPropertyMode)
        VarjoPluginMR.SetCameraPropertyMode(
            VarjoCameraPropertyType.EyeReprojection,
            VarjoCameraPropertyMode.Manual
        );

        // Now set the reprojection distance in meters
        // (mirrors varjo_MRSetCameraPropertyValue)
        VarjoPluginMR.SetCameraPropertyValue(
            VarjoCameraPropertyType.EyeReprojection,
            reprojectionDistance
        );

        // Unlock when done so that other applications can modify config
        VarjoPluginMR.UnlockCameraConfig();
    }
}

*/