using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public Transform[] patrolPoints;
    public float patrolSpeed = 2f; // Speed while patrolling
    public float wanderRadius = 10f; // Radius within which the Kitty will move if no patrol points exist
    public float wanderTime = 3f; // Time before picking a new random destination

    public Transform player; // Player reference
    public float chaseRange = 3f; // Distance to start chasing
    public float chaseSpeed = 4f; // Speed while chasing

    private NavMeshAgent agent;
    private bool isChasing = false;
    private float wanderTimer;
    
    private static int kittyCounter = 1; // Starts at 1 since one Kitty exists at game start

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed; // Set patrol speed

        wanderTimer = wanderTime; // Start wandering immediately

        kittyCounter++; // Increase counter when a Kitty is spawned
        Debug.Log("Kitty Spawned! Total Kitties: " + kittyCounter);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange) // Chase if player is close
        {
            StartChase();
        }
        else if (isChasing) // Stop chase if player escapes
        {
            StopChase();
        }

        if (!isChasing)
        {
            if (patrolPoints.Length > 0)
            {
                PatrolWithWaypoints();
            }
            else
            {
                WanderWithinRadius(); // If no patrol points, move randomly in a set radius
            }
        }
    }

    private void PatrolWithWaypoints()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            int nextPoint = Random.Range(0, patrolPoints.Length); // Pick a random patrol point
            agent.SetDestination(patrolPoints[nextPoint].position);
        }
    }

    private void WanderWithinRadius()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position); // Move to a random position within the radius
            }
            wanderTimer = wanderTime; // Reset the wander timer
        }
    }

    private void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed; // Increase speed
        agent.SetDestination(player.position); // Follow player
    }

    private void StopChase()
    {
        isChasing = false;
        agent.speed = patrolSpeed; // Resume normal speed
    }

    private void OnDestroy()
    {
        kittyCounter--; // Decrease counter when a Kitty is destroyed
        Debug.Log("Kitty Removed! Total Kitties Left: " + kittyCounter);
    }

    public static int GetKittyCount()
    {
        return kittyCounter; // Returns the number of active Kitties
    }

    public static void IncreaseKittyCount(int amount)
    {
        kittyCounter += amount; // Increase by the number of spawned Kitties
    }
}
