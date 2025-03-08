using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrolling Settings")]
    public Transform[] patrolPoints; // Patrol waypoints (optional)
    public float patrolSpeed = 2f; // Speed while patrolling
    public float wanderRadius = 10f; // Radius for random wandering
    public float wanderTime = 3f; // Time before choosing a new position

    [Header("Chasing Settings")]
    public Transform player;
    public float chaseRange = 3f; // Distance to start chasing
    public float chaseSpeed = 4f; // Speed when chasing

    private NavMeshAgent agent;
    private bool isChasing = false; 
    private float wanderTimer; 
    private static int kittyCounter = 0; 

    private Animator animator; 
    private static Text kittyCounterUI; 

    private bool isCounted = false; // ✅ Prevents multiple counting of a single Kitty

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        animator = GetComponent<Animator>();
        wanderTimer = wanderTime;

        if (!isCounted) // Ensure Kitty is only counted once
        {
            IncreaseKittyCount(1);
            isCounted = true;
        }

        Debug.Log("Kitty Spawned! Total Kitties: " + kittyCounter);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange) StartChase();
        else if (isChasing) StopChase();

        if (!isChasing)
        {
            if (patrolPoints.Length > 0) PatrolWithWaypoints();
            else WanderWithinRadius();
        }
    }

    private void PatrolWithWaypoints()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            int nextPoint = Random.Range(0, patrolPoints.Length);
            agent.SetDestination(patrolPoints[nextPoint].position);
        }
    }

    private void WanderWithinRadius()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius + transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
            
            wanderTimer = wanderTime;
        }
    }

    private void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
    }

    private void StopChase()
    {
        isChasing = false;
        agent.speed = patrolSpeed;
        animator.SetBool("isRunning", false);
    }

    private void OnDestroy()
    {
        if (isCounted)
        {
            IncreaseKittyCount(-1);
            isCounted = false;
        }

        Debug.Log("Kitty Removed! Total Kitties Left: " + kittyCounter);
    }

    public static int GetKittyCount() => kittyCounter; 

    public static void IncreaseKittyCount(int amount)
    {
        kittyCounter = Mathf.Max(0, kittyCounter + amount); // Prevent negative count
        if (kittyCounterUI != null) kittyCounterUI.text = "Kitties: " + kittyCounter;
    }

    public static void AssignKittyCounterUI(Text uiText)
    {
        kittyCounterUI = uiText;
        if (kittyCounterUI != null) kittyCounterUI.text = "Kitties: " + kittyCounter;
    }
}
