using UnityEngine;
using System.Collections;

public class PlayerControllergame1 : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    private bool isBraking = false; // Track if braking is active
    public float brakeForce = 5f; // Strength of the braking force

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private bool isFrozen = false; // Track if the player is frozen

    [Header("Freeze Effect")]
    public GameObject freezeEffect; // Visual effect when the player is frozen

    [Header("Ice Breaker Effect")]
    public GameObject iceBreakerEffect; // Visual effect when the player breaks free from freeze

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        // Disable freeze and ice breaker effects at start
        if (freezeEffect != null)
        {
            freezeEffect.SetActive(false);
        }
        if (iceBreakerEffect != null)
        {
            iceBreakerEffect.SetActive(false);
        }
    }

    void Update()
    {
        if (isFrozen) return; // Skip movement if frozen

        // Activate brake when pressing 'E'
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

        // Apply brake if active
        if (isBraking)
        {
            playerRb.linearVelocity *= (1 - brakeForce * Time.deltaTime); // Gradually slow down movement
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

    // Freeze the player
    public void FreezePlayer()
    {
        isFrozen = true;
        playerRb.linearVelocity = Vector3.zero; // Stop movement

        // Activate freeze effect
        if (freezeEffect != null)
        {
            freezeEffect.SetActive(true);
        }

        // Start coroutine to unfreeze after 2 seconds
        StartCoroutine(UnfreezePlayerAfterDelay(5f));
    }

    // Unfreeze the player
    public void UnfreezePlayer()
    {
        isFrozen = false;

        // Deactivate freeze effect
        if (freezeEffect != null)
        {
            freezeEffect.SetActive(false);
        }

        // Activate ice breaker effect
        if (iceBreakerEffect != null)
        {
            iceBreakerEffect.SetActive(true);
            StartCoroutine(DeactivateIceBreakerEffect(4f)); // Deactivate after 1 second
        }
    }

    // Coroutine to unfreeze the player after a delay
    private IEnumerator UnfreezePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnfreezePlayer();
    }

    // Coroutine to deactivate the ice breaker effect after a delay
    private IEnumerator DeactivateIceBreakerEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (iceBreakerEffect != null)
        {
            iceBreakerEffect.SetActive(false);
        }
    }
}