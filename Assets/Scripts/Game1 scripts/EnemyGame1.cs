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

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Find goals
        playerGoal = GameObject.Find("PlayerGoal").transform;
        enemyGoal = GameObject.Find("EnemyGoal").transform;
    }

    void Update()
    {
        // Only follow the player if not moving towards a goal
        if (!isMovingToGoal)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed, ForceMode.Force);
        }

        if (transform.position.y < -50)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Stop following the player and move toward the goal
            isMovingToGoal = true;

            // Bounce enemy away from the player
            Vector3 bounceDirection = (transform.position - player.transform.position).normalized;
            enemyRb.linearVelocity = Vector3.zero; // Reset velocity
            enemyRb.AddForce(bounceDirection * speed * 2, ForceMode.Impulse);

            // After bounce, set target towards the player's goal
            StartCoroutine(MoveToGoal());
        }
    }

    IEnumerator MoveToGoal()
    {
        yield return new WaitForSeconds(0.5f); // Short delay before redirecting

        // Move toward the correct goal (Player's goal)
        Vector3 goalDirection = (playerGoal.position - transform.position).normalized;
        enemyRb.linearVelocity = Vector3.zero;
        enemyRb.AddForce(goalDirection * speed * 5, ForceMode.Impulse); // Strong push towards goal
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyGoal"))
        {
            gameManager.AddScore(1, "Player"); // Player scores when the ball enters the enemy's goal
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("PlayerGoal"))
        {
            gameManager.AddScore(1, "Enemy"); // Enemy scores when the ball enters the player's goal
            Destroy(gameObject);
        }
    }
}
