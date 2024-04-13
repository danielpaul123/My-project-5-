using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float turnSpeed = 3f;
    public Transform[] wheelTransforms;
    public float wheelRotationMultiplier = 100f;
    public float pickupSpeedIncrease = 1f;
    public float pickupEffectDuration = 5f;
    public Transform targetDestination;

    private float currentSpeed = 0f;
    private Rigidbody rb;
    private float pickupEffectTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = 0f;
    }

    void FixedUpdate()
    {
        if (pickupEffectTimer > 0f)
        {
            currentSpeed += pickupSpeedIncrease * Time.deltaTime;
            pickupEffectTimer -= Time.deltaTime;
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Apply forward movement
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);

        // Calculate rotation amount based on desired direction
        Vector3 desiredDirection = (targetDestination.position - transform.position).normalized;
        float rotationAmount = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);

        // Apply rotation
        Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));

        // Rotate wheels
        foreach (Transform wheelTransform in wheelTransforms)
        {
            wheelTransform.Rotate(Vector3.right, rotationAmount * Time.deltaTime * wheelRotationMultiplier);
        }
    }

    // Method to apply pickup effect
    public void ApplyPickupEffect()
    {
        pickupEffectTimer = pickupEffectDuration;
    }

    // Method to set target destination
    public void SetTargetDestination(Transform destination)
    {
        targetDestination = destination;
    }
}
