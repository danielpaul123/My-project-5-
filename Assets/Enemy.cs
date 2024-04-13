/*using System.Collections;
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
}*/















using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AIWaypoints : MonoBehaviour
{
    public Transform[] waypoints;
    public float maxSpeed = 5f;
    public float acceleration = 1f; // Rate of acceleration
    public float turnSpeed = 5f;
    public float minDistanceToWaypoint = 0.1f;
    private int currentWaypointIndex = 0;
    private float currentSpeed = 0f;

    public Text countdownText; // Reference to the UI text element for countdown

    // Wheel variables
    public Transform[] wheelTransforms;
    public WheelCollider[] wheelColliders;

    private void Start()
    {
        StartCoroutine(CountdownAndStartMovement());

        // Initialize wheel colliders and transforms
        InitializeWheels();
    }

    private void InitializeWheels()
    {
        // Assign wheel colliders and transforms
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            wheelColliders[i] = wheelTransforms[i].GetComponentInChildren<WheelCollider>();
        }
    }

    private IEnumerator CountdownAndStartMovement()
    {
        // Disable AI movement during countdown
        enabled = false;

        // Countdown before AI car starts moving
        int countdownValue = 3;
        while (countdownValue > 0)
        {
            countdownText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1f);
            countdownValue--;
        }
        countdownText.text = "GO!";

        // Enable AI movement after countdown
        enabled = true;
    }

    private void Update()
    {
        if (enabled)
        {
            MoveToWaypoint();
            UpdateWheelPoses();
        }
    }

    private void MoveToWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
            return;

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // Smooth rotation towards the target direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Accelerate until reaching max speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = maxSpeed;
        }

        // Move the car forward at the current speed
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < minDistanceToWaypoint)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Length)
        {
            enabled = false;
        }
    }

    private void UpdateWheelPoses()
    {
        // Update visual wheel transforms based on wheel colliders
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            Quaternion rot;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out rot);
            wheelTransforms[i].position = pos;
            wheelTransforms[i].rotation = rot * Quaternion.Euler(0, 0, 90); // Adjust rotation as needed
        }
    }
}
