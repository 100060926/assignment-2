using System.Collections;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    public float hopForce = 10f; // Force applied when the player hops
    public float smashForce = 20f; // Force applied to enemies during smash
    public float smashRadius = 5f; // Radius of the smash effect
    public GameObject shockwaveEffect; // Visual effect for the shockwave

    private bool hasJumpPowerup = false; // Track if the player has the jump powerup

    void Update()
    {
        // Perform hop and smash if the player has the jump powerup and presses Space
        if (hasJumpPowerup && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PerformHopAndSmash());
        }
    }

    // When the player collects the powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Grant the player the jump powerup
            hasJumpPowerup = true;
            Debug.Log("Jump Powerup Collected! Press Space to hop and smash.");

            // Disable the powerup object
            gameObject.SetActive(false);
        }
    }

    // Perform the hop and smash attack
    private IEnumerator PerformHopAndSmash()
    {
        Rigidbody playerRb = GetComponent<Rigidbody>();

        // Hop into the air
        playerRb.AddForce(Vector3.up * hopForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f); // Wait for the player to reach the peak of the hop

        // Smash down
        playerRb.AddForce(Vector3.down * hopForce * 2, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f); // Wait for the player to land

        // Create shockwave effect
        if (shockwaveEffect != null)
        {
            Instantiate(shockwaveEffect, transform.position, Quaternion.identity);
        }

        // Apply force to nearby enemies
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
                    float force = smashForce * (1 - (distance / smashRadius)); // Force decreases with distance
                    enemyRb.AddForce(direction * force, ForceMode.Impulse);
                }
            }
        }

        // Deactivate jump powerup after use
        hasJumpPowerup = false;
        Debug.Log("Hop and Smash Attack Used!");
    }
}