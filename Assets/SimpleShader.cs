using UnityEngine;

public class MapCameraSetup : MonoBehaviour
{
    public Shader replacementShader;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (replacementShader != null && cam != null)
        {
            cam.SetReplacementShader(replacementShader, "");
        }
    }
}
