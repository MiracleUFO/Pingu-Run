using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset = new Vector3(0, 1.25f, -2.5f);

    void Start()
    {
        transform.position = lookAt.position + offset;
    }

    void LateUpdate()
    {
       Vector3 desiredPosition = lookAt.position + offset;
       desiredPosition.x = 0;
       transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
