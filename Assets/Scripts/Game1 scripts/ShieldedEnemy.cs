using UnityEngine;
using System.Collections;

public class ShieldedEnemy : MonoBehaviour
{
    public float speed = 3.0f;
    public float goalSpeed = 2.0f; // Speed when moving to player goal
    public float enemyGoalSpeed = 3.5f; // Speed when moving to enemy goal (after second hit)
    private Rigidbody enemyRb;
    private GameObject player;
    private Transform playerGoal;
    private Transform enemyGoal;
    private int hitsTaken = 0; // Track how many times it was hit
    private bool movingToGoal = false;
    private bool movingToEnemyGoal = false; // True if moving to enemy goal

    [Header("Effects")]
    public GameObject shieldEffect;
    public GameObject shieldBreakEffect;

    private bool canCollide = true; // Flag to control collision registration

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerGoal = GameObject.Find("PlayerGoal").transform;
        enemyGoal = GameObject.Find("EnemyGoal").transform;

        // Activate shield effect at start
        if (shieldEffect != null)
        {
            shieldEffect.SetActive(true);
        }
    }

    void Update()
    {
        if (!movingToGoal && !movingToEnemyGoal)
        {
            // Move toward the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * speed;
        }
        else if (movingToEnemyGoal) // Prioritize moving to enemy goal if both flags are true
        {
            // Move toward the enemy goal
            Vector3 enemyGoalDirection = (enemyGoal.position - transform.position).normalized;
            enemyRb.linearVelocity = enemyGoalDirection * enemyGoalSpeed;
        }
        else if (movingToGoal) // Move toward the player goal
        {
            // Move toward the player goal
            Vector3 goalDirection = (playerGoal.position - transform.position).normalized;
            enemyRb.linearVelocity = goalDirection * goalSpeed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && canCollide)
        {
            hitsTaken++;

            if (hitsTaken == 1) // First hit (Shield breaks)
            {
                Debug.Log("Shield Broken!");

                // Hide shield effect
                if (shieldEffect != null)
                {
                    shieldEffect.SetActive(false);
                }

                // Play shield break effect
                if (shieldBreakEffect != null)
                {
                    Instantiate(shieldBreakEffect, transform.position, Quaternion.identity);
                }

                // Bounce away from the player
                Vector3 bounceDirection = (transform.position - player.transform.position).normalized;
                enemyRb.linearVelocity = bounceDirection * speed * 2f; // Strong bounce

                // Start moving slowly toward Player Goal
                StartCoroutine(MoveToPlayerGoal());

                // Disable collision for 2 seconds
                StartCoroutine(DisableCollisionForSeconds(2f));
            }
            else if (hitsTaken == 2) // Second hit (Redirect to Enemy Goal)
            {
                Debug.Log("Second Hit! Redirecting to Enemy Goal.");
                movingToGoal = false; // Stop moving to player goal
                movingToEnemyGoal = true; // Start moving to enemy goal
            }
        }
    }

    private IEnumerator MoveToPlayerGoal()
    {
        yield return new WaitForSeconds(1.0f); // Allow time for bounce
        movingToGoal = true; // Start moving to the player's goal
    }

    private IEnumerator DisableCollisionForSeconds(float seconds)
    {
        canCollide = false; // Disable collision
        yield return new WaitForSeconds(seconds); // Wait for the specified time
        canCollide = true; // Re-enable collision
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Enemy");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("EnemyGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Player");
            Destroy(gameObject);
        }
    }
}