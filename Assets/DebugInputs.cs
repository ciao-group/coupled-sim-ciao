using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public class ThrottleUI : MonoBehaviour
{
    public RCC_CarControllerV3 car;
    public TextMeshProUGUI throttleText;
    public TextMeshProUGUI brakeText;
    public TextMeshProUGUI speedText;

    void Update()
    {
        float throttleValue = car.throttleInput;
        float brakeValue = car.brakeInput;
        float currentSpeed = car.speed;


        throttleText.text = $"Throttle: {throttleValue:F2}";
        brakeText.text = $"Brake: {brakeValue:F2}";
        speedText.text = $"Speed: {currentSpeed:F2}";
    }
}
