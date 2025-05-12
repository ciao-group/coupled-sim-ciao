using UnityEngine;

public class IndicatorIconController : MonoBehaviour
{
    public Animator iconAnimator;
    public RCC_CarControllerV3 rccCar;
    public enum BlinkerSide { Left, Right }
    public BlinkerSide side;

    void Update()
    {

        if (side == BlinkerSide.Left)
        {
            bool isBlinkingLeft = rccCar.indicatorsOn == RCC_CarControllerV3.IndicatorsOn.Left;
            iconAnimator.SetBool("IsBlinkingLeft", isBlinkingLeft);
            
        }
        else if (side == BlinkerSide.Right)
        {
            bool isBlinkingRight = rccCar.indicatorsOn == RCC_CarControllerV3.IndicatorsOn.Right;
            iconAnimator.SetBool("IsBlinkingRight", isBlinkingRight);
        }
    }
}
