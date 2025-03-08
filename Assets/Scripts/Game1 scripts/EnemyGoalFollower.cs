using UnityEngine;

public class EnemyGoalFollower : MonoBehaviour
{
    public float speed = 3.0f; // Normal speed
    public float goalSpeed = 4.5f; // Faster speed after being redirected
    private Rigidbody enemyRb;
    public Transform playerGoal;
    public Transform enemyGoal;
    private GameManager gameManager;
    private bool isMovingToEnemyGoal = false; // Determines if moving to enemy goal

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindObjectOfType<GameManager>(); // Cache GameManager
        playerGoal = GameObject.Find("PlayerGoal").transform;
        enemyGoal = GameObject.Find("EnemyGoal").transform;
    }

    void Update()
    {
        Transform target = isMovingToEnemyGoal ? enemyGoal : playerGoal;
        MoveTowardsGoal(target);
    }

    void MoveTowardsGoal(Transform goal)
    {
        if (goal == null) return;

        float currentSpeed = isMovingToEnemyGoal ? goalSpeed : speed;
        Vector3 direction = (goal.position - transform.position).normalized;
        enemyRb.linearVelocity = direction * currentSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isMovingToEnemyGoal = true; // Switch direction when player collides
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyGoal"))
        {
            gameManager.AddScore(1, "Player");
            Destroy(gameObject);
        }
        else if (other.CompareTag("PlayerGoal"))
        {
            gameManager.AddScore(1, "Enemy");
            Destroy(gameObject);
        }
    }
}
