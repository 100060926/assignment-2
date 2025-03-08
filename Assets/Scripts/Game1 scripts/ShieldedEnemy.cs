using UnityEngine;
using System.Collections;

public class ShieldedEnemy : MonoBehaviour
{
    public float speed = 3.0f;
    public float goalSpeed = 2.0f; // Speed after shield breaks when moving to goal
    private Rigidbody enemyRb;
    private GameObject player;
    private Transform playerGoal;
    private bool shieldActive = true;
    private bool movingToGoal = false;

    [Header("Effects")]
    public GameObject shieldEffect;
    public GameObject shieldBreakEffect;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerGoal = GameObject.Find("PlayerGoal").transform;

        // Activate shield effect at start
        if (shieldEffect != null)
        {
            shieldEffect.SetActive(true);
        }
    }

    void Update()
    {
        if (!movingToGoal)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * speed;
        }
        else
        {
            Vector3 goalDirection = (playerGoal.position - transform.position).normalized;
            enemyRb.linearVelocity = goalDirection * goalSpeed; // Moves slowly to the player's goal
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (shieldActive)
            {
                shieldActive = false; // First hit removes shield
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
            }
        }
    }

    private IEnumerator MoveToPlayerGoal()
    {
        yield return new WaitForSeconds(1.0f); // Allow time for bounce
        movingToGoal = true; // Start moving to the player's goal
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerGoal"))
        {
            FindObjectOfType<GameManager>().AddScore(1, "Enemy");
            Destroy(gameObject);
        }
    }
}
