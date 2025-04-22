using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    public Transform car;

    private void LateUpdate()
    {
        Vector3 newPosition = car.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

    }
}
