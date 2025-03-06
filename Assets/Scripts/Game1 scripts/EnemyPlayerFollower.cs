using UnityEngine;

public class EnemyPlayerFollower : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    private GameManager gameManager;
    private bool isMovingToEnemyGoal = false; // Track if moving towards the enemy goal

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.FindObjectOfType<GameManager>(); // Cache GameManager
    }

    void Update()
    {
        if (!isMovingToEnemyGoal && player != null)
        {
            // Follow the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * speed;
        }
        else
        {
            // Move towards the enemy's goal
            GameObject enemyGoal = GameObject.Find("EnemyGoal");
            if (enemyGoal != null)
            {
                Vector3 direction = (enemyGoal.transform.position - transform.position).normalized;
                enemyRb.linearVelocity = direction * speed;
            }
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
            isMovingToEnemyGoal = true;

            // Bounce enemy away from the player
            Vector3 bounceDirection = (transform.position - player.transform.position).normalized;
            enemyRb.linearVelocity = Vector3.zero; // Reset velocity
            enemyRb.AddForce(bounceDirection * speed * 3, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyGoal"))
        {
            // Player scores when the enemy reaches the enemy's goal
            gameManager.AddScore(1, "Player");
            Destroy(gameObject);
        }
        else if (other.CompareTag("PlayerGoal"))
        {
            // Enemy scores when they enter the player's goal
            gameManager.AddScore(1, "Enemy");
            Destroy(gameObject);
        }
    }
}
