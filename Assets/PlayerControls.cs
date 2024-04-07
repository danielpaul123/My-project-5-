using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float turnSpeed = 3f;

    private float currentSpeed = 0f;

    void Update()
    {
        float accelerationInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (accelerationInput > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }
        else if (accelerationInput < 0)
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            }
        }

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        if (currentSpeed > 0)
        {
            float turn = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);
        }
    }
}
