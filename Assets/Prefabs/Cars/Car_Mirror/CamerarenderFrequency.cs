using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerarenderFrequency : MonoBehaviour
{
    public Camera mirrorCamera;
    public int renderInterval = 5;
    private void Start()
    {
        mirrorCamera.enabled = false;
    }
    void Update()
    {
        if (Time.frameCount % renderInterval == 0)
        {
            mirrorCamera.Render();
        }
    }
}
