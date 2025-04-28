using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform car;

    void LateUpdate()
    {
        Vector3 newPosition = car.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, car.eulerAngles.y, 0f);
    }
}
