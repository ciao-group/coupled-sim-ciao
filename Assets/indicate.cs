using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Indicate : MonoBehaviour
{
    public enum IndicatorDirection { Left, Right, Off, None }

    public IndicatorDirection inDirection = IndicatorDirection.None;
}
