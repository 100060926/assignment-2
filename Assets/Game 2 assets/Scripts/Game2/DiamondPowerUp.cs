using UnityEngine;
using System.Collections;

public class DiamondPowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public float powerUpDuration = 5f; // Duration of the power-up effect
    public float pushRadius = 7f; // ✅ Increased radius to affect more enemies
    public float powerupStrength = 40f; // ✅ Stronger push force for a visible effect
    public GameObject powerupIndicator; // UI indicator for power-up
    public ParticleSystem powerupParticle; // 🌟 Particle effect surrounding the player

    private bool hasPowerup = false; // Tracks whether power-up is active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player picks up the Diamond
        {
            Debug.Log("Diamond Collected! Power-up activated.");

            hasPowerup = true; // Enable power-up effect
            powerupIndicator.SetActive(true); // Show UI effect

            if (powerupParticle != null)
            {
                powerupParticle.Play(); // 🌟 Start particle effect on player
            }

            gameObject.SetActive(false); // 💎 Make the Diamond disappear

            PushNearbyEnemies(other.transform.position); // 🚀 Immediately push all nearby enemies away

            StartCoroutine(PowerupCooldown()); // Start power-up timer
        }
    }

    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration); // ⏳ Wait 5 seconds
        hasPowerup = false; // Disable power-up
        powerupIndicator.SetActive(false); // Hide UI effect

        if (powerupParticle != null)
        {
            powerupParticle.Stop(); // ❌ Stop particle effect
        }

        Debug.Log("Power-up expired!");
    }

    private void PushNearbyEnemies(Vector3 playerPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, pushRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy")) // Check if it's a Kitty
            {
                Rigidbody enemyRigidbody = collider.GetComponent<Rigidbody>();

                if (enemyRigidbody != null)
                {
                    // ✅ Push the enemy directly away from the player with strong force
                    Vector3 pushDirection = (collider.transform.position - playerPosition).normalized;
                    enemyRigidbody.AddForce(pushDirection * powerupStrength, ForceMode.Impulse);

                    Debug.Log("Kitty pushed FAR AWAY due to power-up!");
                }
            }
        }
    }
}
