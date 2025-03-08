using UnityEngine;
using System.Collections;

public class SpeedBoosterEnemy : MonoBehaviour
{
    public float speed = 8.0f; // Speed when shield is on (faster)
    public float slowSpeed = 5.0f; // Speed when shield is off (slower)
    public float goalSpeed = 5.0f; // Speed when moving toward a goal
    public float attackDuration = 4.0f; // Duration of attack mode (shield on)
    public float breakDuration = 4.0f; // Duration of break mode (shield off)
    private Rigidbody enemyRb;
    private GameObject player;
    private Transform playerGoal;
    private Transform enemyGoal;
    private bool isMovingToGoal = false; // True when moving toward a goal
    private bool isInAttackMode = false; // True when in attack mode (shield on)
    private bool attackModePermanentlyOff = false; // True after colliding with the player
    private bool isMovingToPlayerGoal = false; // True when moving toward the player's goal

    [Header("Effects")]
    public GameObject shieldOnEffect; // Shield effect (active during attack mode)
    public GameObject shieldOffEffect; // Shield effect (active during break mode or after hitting player)

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerGoal = GameObject.Find("PlayerGoal").transform;
        enemyGoal = GameObject.Find("EnemyGoal").transform;

        // Start the attack-break cycle
        StartCoroutine(AttackCycle());
    }

    void Update()
    {
        if (isMovingToGoal)
        {
            // Move toward the current goal (player's goal or enemy's goal)
            Vector3 goalDirection = (isMovingToPlayerGoal ? playerGoal.position : enemyGoal.position) - transform.position;
            goalDirection.Normalize();
            enemyRb.linearVelocity = goalDirection * goalSpeed;
        }
        else
        {
            // Follow the player at the appropriate speed
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * (isInAttackMode ? speed : slowSpeed);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isInAttackMode && !attackModePermanentlyOff)
            {
                // Freeze player's vertical input for 2 seconds (only during attack mode)
                PlayerControllergame1 playerController = other.gameObject.GetComponent<PlayerControllergame1>();
                if (playerController != null)
                {
                    playerController.FreezePlayer(); // Call FreezePlayer method in PlayerController
                }

                // Start moving toward the player's goal
                isMovingToGoal = true;
                isMovingToPlayerGoal = true;

                // Turn off attack mode permanently
                attackModePermanentlyOff = true;
                isInAttackMode = false;

                // Disable shield on effect and enable shield off effect
                if (shieldOnEffect != null) shieldOnEffect.SetActive(false);
                if (shieldOffEffect != null) shieldOffEffect.SetActive(true);
            }
            else if (!isInAttackMode || attackModePermanentlyOff)
            {
                // Redirect toward the enemy's goal if hit by the player during shield off mode
                isMovingToGoal = true;
                isMovingToPlayerGoal = false;

                // Turn off attack mode permanently (if not already off)
                attackModePermanentlyOff = true;
                isInAttackMode = false;

                // Disable shield on effect and enable shield off effect
                if (shieldOnEffect != null) shieldOnEffect.SetActive(false);
                if (shieldOffEffect != null) shieldOffEffect.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Player"); // Player scores when Speed Booster enters the enemy goal
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("PlayerGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Enemy"); // Enemy scores if it reaches the player goal
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            if (!attackModePermanentlyOff)
            {
                // Attack mode (shield on)
                isInAttackMode = true;
                if (shieldOnEffect != null) shieldOnEffect.SetActive(true); // Activate shield on effect
                if (shieldOffEffect != null) shieldOffEffect.SetActive(false); // Deactivate shield off effect
                yield return new WaitForSeconds(attackDuration);

                // Break mode (shield off)
                isInAttackMode = false;
                if (shieldOnEffect != null) shieldOnEffect.SetActive(false); // Deactivate shield on effect
                if (shieldOffEffect != null) shieldOffEffect.SetActive(true); // Activate shield off effect
                yield return new WaitForSeconds(breakDuration);
            }
            else
            {
                // If attack mode is permanently off, skip the attack-break cycle
                yield return null;
            }
        }
    }
}