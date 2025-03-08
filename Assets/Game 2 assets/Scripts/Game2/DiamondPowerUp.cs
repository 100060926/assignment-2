using UnityEngine;
using System.Collections;
//shahad
public class DiamondPowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public float powerUpDuration = 5f; // Duration of the power-up effect
    public float pushRadius = 10f; // Radius in which enemies are affected
    public float powerupStrength = 50f; // Stronger push force for a visible effect
    public GameObject powerupIndicator; // UI indicator for power-up
    public ParticleSystem powerupParticle; //Particle effect surrounding the player

    private bool hasPowerup = false; // Tracks whether power-up is active

    void Start()
    {
        //Ensure the Diamond starts as ACTIVE when the game begins
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player picks up the Diamond
        {
            Debug.Log("Diamond Collected! Power-up activated.");

            hasPowerup = true; // Enable power-up effect
            powerupIndicator.SetActive(true); // Show UI effect

            if (powerupParticle != null)
            {
                powerupParticle.Play(); //Start particle effect on player
            }

            gameObject.SetActive(false); //Make the Diamond disappear

            StartCoroutine(PowerupCooldown()); // Start power-up timer

            PushNearbyEnemies(other.transform.position); //Immediately push all nearby enemies away
        }
    }

    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration); // Wait 5 seconds
        hasPowerup = false; // Disable power-up
        powerupIndicator.SetActive(false); // Hide UI effect

        if (powerupParticle != null)
        {
            powerupParticle.Stop(); //Stop particle effect
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
                    //Ensure Rigidbody is NOT Kinematic
                    enemyRigidbody.isKinematic = false;

                    // Push the enemy directly away from the player with strong force
                    Vector3 pushDirection = (collider.transform.position - playerPosition).normalized;
                    enemyRigidbody.AddForce(pushDirection * powerupStrength, ForceMode.VelocityChange);

                    Debug.Log("Kitty pushed FAR AWAY due to power-up!");
                }
            }
        }
    }
}
