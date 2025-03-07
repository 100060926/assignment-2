using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrolling Settings")]
    public Transform[] patrolPoints; // 4 patrol points (forming a square)
    public float patrolSpeed = 2f; // Speed while patrolling

    [Header("Chasing & Detection Settings")]
    public Transform player; // Player reference
    public Transform basket; // The basket being guarded
    public float chaseRange = 5f; // Distance to start chasing
    public float fieldOfView = 60f; // Angle to detect if player is facing the kitty
    public float chaseSpeed = 4f; // Speed while chasing

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent
        agent.speed = patrolSpeed; // Set patrol speed

        if (patrolPoints.Length > 0) // Ensure patrol points exist
        {
            MoveToNextPatrolPoint(); // Start patrolling
        }
        else
        {
            Debug.LogError("No patrol points assigned to " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null || basket == null)
        {
            Debug.LogError("Player or Basket is not assigned in " + gameObject.name);
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.position, basket.position); // Distance between player & basket
        float angleToPlayer = Vector3.Angle(transform.forward, (player.position - transform.position).normalized);

        // Check if the player is near the basket AND facing the kitty
        if (distanceToPlayer <= chaseRange && angleToPlayer < fieldOfView / 2)
        {
            StartChase();
        }
        else if (isChasing)
        {
            StopChase();
        }

        // Keep patrolling if not chasing
        if (!isChasing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolPoint();
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return; // No patrol points? Do nothing

        agent.speed = patrolSpeed; // Set patrol speed
        agent.destination = patrolPoints[currentPatrolIndex].position; // Move to patrol point

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Loop patrol points
    }

    private void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed; // Increase speed
        agent.destination = player.position; // Follow the player
    }

    private void StopChase()
    {
        isChasing = false;
        MoveToNextPatrolPoint(); // Resume patrolling
    }
}
