using System.Collections;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    public float hopForce = 15f; // Force applied when the player hops
    public float smashForce = 50f; // Force applied to enemies during smash
    public float smashRadius = 30f; // Reduced radius for better knockback
    public float stunDuration = 3f; // Duration enemies are stunned

    private bool hasJumpPowerup = false; // Track if the player has the jump powerup
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hasJumpPowerup && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PerformHopAndSmash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpPowerup"))
        {
            hasJumpPowerup = true;
            Debug.Log("Jump Powerup Collected! Press Space to hop and smash.");
            other.gameObject.SetActive(false);
        }
    }

    private IEnumerator PerformHopAndSmash()
    {
        // Hop into the air
        playerRb.AddForce(Vector3.up * hopForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f); // Wait for peak

        // Smash down
        playerRb.AddForce(Vector3.down * hopForce * 2, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f); // Wait for landing

        // Apply knockback and stun
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, smashRadius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 direction = (enemy.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    // Apply knockback force
                    float force = smashForce * (1 - (distance / smashRadius)); // Closer enemies get stronger knockback
                    enemyRb.AddForce(direction * force, ForceMode.Impulse);

                    // Stun enemy
                    StartCoroutine(StunEnemy(enemyRb, enemy.gameObject, stunDuration));
                }
            }
        }

        hasJumpPowerup = false;
        Debug.Log("Hop and Smash Attack Used!");
    }

    private IEnumerator StunEnemy(Rigidbody enemyRb, GameObject enemy, float duration)
    {
        if (enemyRb != null)
        {
            Vector3 originalVelocity = enemyRb.linearVelocity;
            enemyRb.linearVelocity = Vector3.zero;
            enemyRb.angularVelocity = Vector3.zero;

            // Disable enemy movement script (if applicable)
            MonoBehaviour enemyScript = enemy.GetComponent<MonoBehaviour>();
            if (enemyScript != null)
            {
                enemyScript.enabled = false;
            }

            yield return new WaitForSeconds(duration);

            enemyRb.linearVelocity = originalVelocity; // Restore movement
            if (enemyScript != null)
            {
                enemyScript.enabled = true;
            }

            Debug.Log("Enemy is no longer stunned!");
        }
    }
}
