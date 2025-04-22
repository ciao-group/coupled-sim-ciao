using UnityEngine;


public class ClusterIndicator : MonoBehaviour
{

    /// <summary>
    /// Animator for this indicator light
    /// </summary>
    private Animator Animator;

    /// <summary>
    /// Is the indicator currently turned on?
    /// </summary>
    public bool IsTurnedOn { get; private set; }

    /// <summary>
    /// Unity Awake callback
    /// </summary>
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Listener to hook up the indicator to a boolean value change event
    /// </summary>
    /// <param name="value">value the indicator used to determine to turn on or off</param>
    public virtual void OnValueChangeListener(bool value)
    {
        if (value && !IsTurnedOn)
        {
            TurnOn();
        }
        else if (!value && IsTurnedOn)
        {
            TurnOff();
        }
    }

    /// <summary>
    /// Turn the cluster indicator on
    /// </summary>
    public void TurnOn()
    {
        IsTurnedOn = true;


        Animator.SetBool("Activate", true);

    }

    /// <summary>
    /// Turn the cluster indicator off
    /// </summary>
    public void TurnOff()
    {
        IsTurnedOn = false;
        Animator.SetBool("Deactivate", true);
    }
}