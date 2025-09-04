using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [Header("Target Settings")]
    public Transform car;
    public Vector3 offset = new Vector3(0.7f, 9f, -6.8f);
    
    
    void LateUpdate()
    {
        if (car != null) return;

        Vector3 newPosition = car.position + car.TransformDirection(offset);

        
        newPosition.y = transform.position.y;
        transform.position = newPosition;

//transform.rotation = Quaternion.Euler(90f, car.eulerAngles.y, 0f);
    }
}
