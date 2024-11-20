//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

#if RCC_LOGITECH
using UnityEngine;


public class RCC_LogitechSteeringWheel : MonoBehaviour {

    #region singleton
    private static RCC_LogitechSteeringWheel instance;
    public static RCC_LogitechSteeringWheel Instance {

        get {

            if (instance == null) {

                instance = FindObjectOfType<RCC_LogitechSteeringWheel>();

                if (instance == null) {

                    GameObject sceneManager = new GameObject("_RCCLogitechSteeringWheelManager");
                    instance = sceneManager.AddComponent<RCC_LogitechSteeringWheel>();

                }

            }

            return instance;

        }

    }
    #endregion

    public bool useForceFeedback = true;
    public float roughness = 70f;
    public float collisionForce = 40f;

    void Start() {

        LogitechGSDK.LogiSteeringInitialize(false);

    }

    void OnEnable() {

        RCC_CarControllerV3.OnRCCPlayerCollision += RCC_CarControllerV3_OnRCCPlayerCollision;
        RCC_InputManager.Instance.logitechSteeringUsed = true;

    }

    void RCC_CarControllerV3_OnRCCPlayerCollision(RCC_CarControllerV3 RCC, Collision collision) {

        if (RCC == RCC_SceneManager.Instance.activePlayerVehicle)
            LogitechGSDK.LogiPlayFrontalCollisionForce(0, Mathf.CeilToInt(collision.impulse.magnitude / 10000f * collisionForce));

    }

    void Update() {

        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0)) {

            if (useForceFeedback)
                ForceFeedback();

        }

    }

    void ForceFeedback() {

        RCC_CarControllerV3 playerVehicle = RCC_SceneManager.Instance.activePlayerVehicle;

        if (!playerVehicle)
            return;

        float sidewaysForce = playerVehicle.FrontLeftWheelCollider.wheelSlipAmountSideways + playerVehicle.FrontRightWheelCollider.wheelSlipAmountSideways;
        sidewaysForce *= Mathf.Abs(sidewaysForce);
        sidewaysForce *= -roughness;

        LogitechGSDK.LogiStopConstantForce(0);
        LogitechGSDK.LogiPlayConstantForce(0, (int)(sidewaysForce));

        bool isGrounded = playerVehicle.isGrounded;

        if (!isGrounded)
            LogitechGSDK.LogiPlayCarAirborne(0);
        else
            LogitechGSDK.LogiStopCarAirborne(0);

    }

    void OnDisable() {

        RCC_CarControllerV3.OnRCCPlayerCollision -= RCC_CarControllerV3_OnRCCPlayerCollision;
        RCC_InputManager.Instance.logitechSteeringUsed = false;

    }

    void OnApplicationQuit() {

        LogitechGSDK.LogiSteeringShutdown();

    }

}
#endif