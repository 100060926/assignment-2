using UnityEngine;
using System.Collections;

public class SpeedBoosterEnemy : MonoBehaviour
{
    public float speed = 3.0f;
    public float dashSpeed = 8.0f;
    public float slowSpeed = 2.0f; // Speed after being redirected
    public float dashCooldown = 3.0f;
    public float warningTime = 1.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    private Transform enemyGoal;
    private bool isDashing = false;
    private bool movingToGoal = false;

    [Header("Effects")]
    public GameObject dashEffect;
    private Renderer enemyRenderer;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemyGoal = GameObject.Find("EnemyGoal").transform;
        enemyRenderer = GetComponent<Renderer>();

        StartCoroutine(DashCycle());
    }

    void Update()
    {
        if (!movingToGoal)
        {
            if (!isDashing)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                enemyRb.linearVelocity = direction * speed;
            }
        }
        else
        {
            Vector3 goalDirection = (enemyGoal.position - transform.position).normalized;
            enemyRb.linearVelocity = goalDirection * slowSpeed; // Move slowly to the enemy goal
        }
    }

    IEnumerator DashCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashCooldown);

            // Warning Indicator (Change color before dashing)
            enemyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(warningTime);
            enemyRenderer.material.color = Color.white;

            isDashing = true;
            Vector3 dashDirection = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = dashDirection * dashSpeed;

            // Play Dash Effect
            if (dashEffect != null)
            {
                GameObject effect = Instantiate(dashEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1.0f); // Destroy effect after 1 second
            }

            yield return new WaitForSeconds(1.0f); // Dash duration
            isDashing = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isDashing)
            {
                StartCoroutine(FreezePlayerMovement(other.gameObject));
            }
            else
            {
                // When not dashing, the player can hit it to redirect
                movingToGoal = true;
                Vector3 bounceDirection = (transform.position - player.transform.position).normalized;
                enemyRb.linearVelocity = bounceDirection * speed * 2f; // Bounce away

                Debug.Log("Speed Booster hit! Now moving to enemy goal.");
            }
        }
    }

    private IEnumerator FreezePlayerMovement(GameObject player)
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 originalVelocity = playerRb.linearVelocity;
            playerRb.linearVelocity = Vector3.zero; // Stop the player
            yield return new WaitForSeconds(2f); // Freeze for 2 seconds
            playerRb.linearVelocity = originalVelocity; // Restore movement
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Player"); // Player scores when redirected Speed Booster enters the enemy goal
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("PlayerGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Enemy"); // Enemy scores if it reaches the player goal
            Destroy(gameObject);
        }
    }
}
