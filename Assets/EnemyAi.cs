using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform[] waypoints; // Waypoints for the enemy car to follow
    private int currentWaypointIndex = 0; // Index of the current waypoint
    public float moveSpeed = 5f; // Speed at which the car moves

    void Update()
    {
        // Move towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the car has reached the current waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Set the next waypoint as the target
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
    
