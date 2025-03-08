using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGame1 : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    public Transform playerGoal;  // Reference to player's goal
    public Transform enemyGoal;   // Reference to enemy's goal
    public GameManager gameManager; // Reference to game manager to track score
    private bool isMovingToGoal = false; // Track if the enemy is moving towards a goal
    public bool isGoalFollower = false; // Determines if the enemy is a goal follower from the start

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Find goals
        playerGoal = GameObject.Find("PlayerGoal").transform;
        enemyGoal = GameObject.Find("EnemyGoal").transform;

        // If this is a goal follower, set it to move directly toward the enemy goal
        if (isGoalFollower)
        {
            isMovingToGoal = true;
        }
    }

    void Update()
    {
        // Move toward the enemy goal if flagged to do so
        if (isMovingToGoal)
        {
            MoveTowardsGoal(enemyGoal);
        }
        else
        {
            // Default behavior is to follow the player
            MoveTowardsGoal(player.transform);
        }

        if (transform.position.y < -50)
        {
            Destroy(gameObject);
        }
    }

    void MoveTowardsGoal(Transform goal)
    {
        if (goal == null) return;

        Vector3 direction = (goal.position - transform.position).normalized;
        enemyRb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Stop following the player and move toward the enemy goal
            isMovingToGoal = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyGoal"))
        {
            // If a goal follower reaches the enemy's goal, the enemy team scores
            gameManager.AddScore(1, "Enemy");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("PlayerGoal"))
        {
            // If any enemy reaches the player's goal, the enemy scores
            gameManager.AddScore(1, "Enemy");
            Destroy(gameObject);
        }
    }
}
