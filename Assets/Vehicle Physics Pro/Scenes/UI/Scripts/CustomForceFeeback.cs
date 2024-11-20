using UnityEngine;

/// <summary>
/// This script calculates force feedback values from the RCC simulation and logs them to the console.
/// </summary>
public class CustomForceFeedbackLogger : MonoBehaviour
{
    // Enable or disable force feedback calculations
    public bool useForceFeedback = true;

    // Roughness factor for road resistance
    public float roughness = 70f;

    // Scaling factor for collision feedback
    public float collisionForce = 40f;

    private void OnEnable()
    {
        // Subscribe to collision events
        RCC_CarControllerV3.OnRCCPlayerCollision += HandleCollisionForce;
    }

    private void OnDisable()
    {
        // Unsubscribe from collision events
        RCC_CarControllerV3.OnRCCPlayerCollision -= HandleCollisionForce;
    }

    private void Update()
    {
        if (useForceFeedback)
        {
            CalculateAndLogForceFeedback();
        }
    }

    /// <summary>
    /// Handles force feedback during collisions.
    /// </summary>
    /// <param name="vehicle">The vehicle involved in the collision.</param>
    /// <param name="collision">Collision details.</param>
    private void HandleCollisionForce(RCC_CarControllerV3 vehicle, Collision collision)
    {
        // Ensure the collision is from the active player vehicle
        if (vehicle != RCC_SceneManager.Instance.activePlayerVehicle)
            return;

        // Calculate collision force feedback
        float collisionFeedback = collision.impulse.magnitude / 10000f * collisionForce;

        // Log collision force feedback
        Debug.Log($"Collision Force Feedback: {collisionFeedback}");
    }

    /// <summary>
    /// Calculates and logs force feedback values from tire slip and ground status.
    /// </summary>
    private void CalculateAndLogForceFeedback()
    {
        // Get the active player vehicle
        RCC_CarControllerV3 playerVehicle = RCC_SceneManager.Instance.activePlayerVehicle;

        if (!playerVehicle)
            return;

        // Calculate sideways force from wheel slip
        float sidewaysForce = playerVehicle.FrontLeftWheelCollider.wheelSlipAmountSideways +
                              playerVehicle.FrontRightWheelCollider.wheelSlipAmountSideways;

        // Apply nonlinear scaling and roughness factor
        sidewaysForce *= Mathf.Abs(sidewaysForce);
        sidewaysForce *= -roughness;

        // Log sideways force
        Debug.Log($"Sideways Force Feedback: {sidewaysForce}");

        // Check if the car is grounded
        bool isGrounded = playerVehicle.isGrounded;

        if (!isGrounded)
        {
            // Log airborne status
            Debug.Log("Airborne: No force feedback applied.");
        }
        else
        {
            // Log grounded status
            Debug.Log("Grounded: Applying force feedback.");
        }
    }
}
