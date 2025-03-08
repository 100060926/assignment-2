using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllergame1 : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    private bool isBraking = false; // Track if braking is active -Hoor
    public float brakeForce = 5f; // Strength of the braking force -Hoor

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private bool isFrozen = false; // Track if the player is frozen

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        if (isFrozen) return; // Skip movement if frozen

        // Activate brake when pressing 'E' -Hoor
        if (Input.GetKey(KeyCode.E))
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }

        // Move player normally
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = focalPoint.transform.forward * verticalInput;
        playerRb.AddForce(movementDirection.normalized * speed * Time.deltaTime, ForceMode.Force);

        // Apply brake if active -Hoor
        if (isBraking)
        {
            playerRb.linearVelocity *= (1 - brakeForce * Time.deltaTime); // Gradually slow down movement -Hoor
        }

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = transform.position - other.gameObject.transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }

    // Freeze the player's vertical input for 2 seconds
    public void FreezePlayer()
    {
        StartCoroutine(DisableVerticalInputForSeconds(5f));
    }

    // Coroutine to disable vertical input for a specified duration
    private IEnumerator DisableVerticalInputForSeconds(float seconds)
    {
        isFrozen = true; // Disable vertical input
        yield return new WaitForSeconds(seconds); // Wait for the specified duration
        isFrozen = false; // Re-enable vertical input
    }
}