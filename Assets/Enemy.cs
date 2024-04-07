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
    private NavMeshAgent agent;
    private float pickupEffectTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        currentSpeed = 0f;
        agent.speed = maxSpeed;

        if (targetDestination != null)
        {
            SetTargetDestination(targetDestination.position);
        }
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


        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);


        Vector3 desiredVelocity = agent.velocity;

        float horizontalInput = Mathf.Sign(Vector3.Dot(transform.right, Vector3.Cross(transform.forward, desiredVelocity)));
        float rotationAmount = horizontalInput * turnSpeed;


        transform.Rotate(Vector3.up, rotationAmount);

        foreach (Transform wheelTransform in wheelTransforms)
        {
            wheelTransform.Rotate(Vector3.right, rotationAmount * wheelRotationMultiplier);
        }
    }

    // Method to apply pickup effect
    public void ApplyPickupEffect()
    {
        pickupEffectTimer = pickupEffectDuration;
    }


    public void SetTargetDestination(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
    }

}
