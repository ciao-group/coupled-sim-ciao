using TMPro;
using UnityEngine;

public class MarkerAdjustmentUI : MonoBehaviour
{
    public VarjoMarkerManager varjoMarkerManager;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI rotationText;
    public TextMeshProUGUI scaleText;

    public float positionStep = 0.01f;
    public float rotationStep = 0.01f; 
    public float scaleStep = 0.01f;

    private int activeMarkerIndex = 0;

    void Update()
    {
        // Position adjustments
        if (Input.GetKeyDown(KeyCode.Q)) AdjustVectorValue("position", 0, -positionStep);
        if (Input.GetKeyDown(KeyCode.E)) AdjustVectorValue("position", 0, positionStep);
        if (Input.GetKeyDown(KeyCode.F)) AdjustVectorValue("position", 1, -positionStep);
        if (Input.GetKeyDown(KeyCode.G)) AdjustVectorValue("position", 1, positionStep);
        if (Input.GetKeyDown(KeyCode.Z)) AdjustVectorValue("position", 2, -positionStep);
        if (Input.GetKeyDown(KeyCode.X)) AdjustVectorValue("position", 2, positionStep);

        // Rotation adjustments
        if (Input.GetKeyDown(KeyCode.T)) AdjustVectorValue("rotation", 0, -rotationStep);
        if (Input.GetKeyDown(KeyCode.Y)) AdjustVectorValue("rotation", 0, rotationStep);
        if (Input.GetKeyDown(KeyCode.H)) AdjustVectorValue("rotation", 1, -rotationStep);
        if (Input.GetKeyDown(KeyCode.J)) AdjustVectorValue("rotation", 1, rotationStep);
        if (Input.GetKeyDown(KeyCode.V)) AdjustVectorValue("rotation", 2, -rotationStep);
        if (Input.GetKeyDown(KeyCode.B)) AdjustVectorValue("rotation", 2, rotationStep);

        // Scale adjustments
        if (Input.GetKeyDown(KeyCode.U)) AdjustVectorValue("scale", 0, -scaleStep);
        if (Input.GetKeyDown(KeyCode.I)) AdjustVectorValue("scale", 0, scaleStep);
        if (Input.GetKeyDown(KeyCode.K)) AdjustVectorValue("scale", 1, -scaleStep);
        if (Input.GetKeyDown(KeyCode.L)) AdjustVectorValue("scale", 1, scaleStep);
        if (Input.GetKeyDown(KeyCode.N)) AdjustVectorValue("scale", 2, -scaleStep);
        if (Input.GetKeyDown(KeyCode.M)) AdjustVectorValue("scale", 2, scaleStep);
    }

    private void AdjustVectorValue(string property, int axis, float increment)
    {
        if (property == "position")
            varjoMarkerManager.trackedObjects[activeMarkerIndex].positionOffsets[0][axis] += increment;
        else if (property == "rotation")
            varjoMarkerManager.trackedObjects[activeMarkerIndex].rotationOffsets[0][axis] += increment;  // Rotation in degrees, convert if necessary
        else if (property == "scale")
            varjoMarkerManager.trackedObjects[activeMarkerIndex].scaleOffsets[0][axis] += increment;

        UpdateUI();
    }

    private void UpdateUI()
    {
        positionText.text = "Position: " + varjoMarkerManager.trackedObjects[activeMarkerIndex].positionOffsets[0].ToString();
        rotationText.text = "Rotation: " + varjoMarkerManager.trackedObjects[activeMarkerIndex].rotationOffsets[0].ToString();
        scaleText.text = "Scale: " + varjoMarkerManager.trackedObjects[activeMarkerIndex].scaleOffsets[0].ToString();
    }
}
