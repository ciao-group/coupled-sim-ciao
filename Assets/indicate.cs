using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Indicate : MonoBehaviour
{
    public enum IndicatorDirection { Left, Right, None }

    public IndicatorDirection direction = IndicatorDirection.Right;

    [Tooltip("Optional: how far the car must be to activate the indicator")]
    public float activationDistance = 5f;

    private bool indicatorActivated = false;

    private void Update()
    {

        RCC_CarControllerV3 car = RCC_SceneManager.Instance.activePlayerVehicle;
        if (car == null)
            return;

        float distance = Vector3.Distance(car.transform.position, transform.position);

        if (!indicatorActivated && distance < activationDistance)
        {
            // Activate indicator
            switch (direction)
            {
                case IndicatorDirection.Left:
                    car.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Left;
                    Debug.Log("Left");
                    break;
                case IndicatorDirection.Right:
                    car.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Right;
                    Debug.Log("Right");
                    break;
                case IndicatorDirection.None:
                    car.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
                    Debug.Log("Off");
                    break;
            }

            indicatorActivated = true;
        }

        if (indicatorActivated && distance > activationDistance)
        {
            //car.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
            //indicatorActivated = false;
        }
    }
}
