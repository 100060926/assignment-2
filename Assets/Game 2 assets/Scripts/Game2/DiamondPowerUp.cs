using UnityEngine;
using System.Collections;

//shahad
public class DiamondPowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public float powerUpDuration = 5f; // Duration of the power-up effect
    public float pushRadius = 15f; // Increased push radius for a bigger effect
    public float powerupStrength = 150f; // Extremely strong push force
    public GameObject powerupIndicator; // UI indicator for power-up
    public ParticleSystem powerupParticle; //  Particle effect surrounding the player

    private bool hasPowerup = false; // Tracks whether power-up is active

    void Start()
    {
        gameObject.SetActive(true); //  Ensure the Diamond starts active
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //  Ensure only the Player can collect the Diamond
        {
            Debug.Log("Diamond Collected! Power-up activated.");

            hasPowerup = true; // Enable power-up effect
            powerupIndicator.SetActive(true); // Show UI effect

            if (powerupParticle != null)
            {
                powerupParticle.Play(); // Start particle effect on player
            }

            gameObject.SetActive(false); //  Make the Diamond disappear

            PushNearbyEnemies(other.transform.position); //  Immediately push ONLY enemies away

            StartCoroutine(PowerupCooldown()); // Start power-up timer
        }
    }

    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration); //  Wait for power-up duration
        hasPowerup = false; // Disable power-up
        powerupIndicator.SetActive(false); // Hide UI effect

        if (powerupParticle != null)
        {
            powerupParticle.Stop(); //  Stop particle effect
        }

        Debug.Log("Power-up expired!");
    }

    private void PushNearbyEnemies(Vector3 playerPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, pushRadius);

        foreach (Collider collider in hitColliders)
        {
            //  Check ONLY if the object is tagged "Enemy"
            if (collider.CompareTag("Enemy"))
            {
                Rigidbody enemyRigidbody = collider.GetComponent<Rigidbody>();

                if (enemyRigidbody != null)
                {
                    enemyRigidbody.isKinematic = false; // Ensure Rigidbody is NOT Kinematic

                    // Push the enemy directly away from the player with STRONG force
                    Vector3 pushDirection = (collider.transform.position - playerPosition).normalized;
                    enemyRigidbody.AddForce(pushDirection * powerupStrength, ForceMode.Impulse);

                    Debug.Log("Enemy pushed FAR AWAY due to power-up!");
                }
            }
        }
    }
}
