using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllergame1 : MonoBehaviour
{
    private Rigidbody playerRb;
    private float normalSpeed = 500;
    private float boostSpeed = 1000; // Speed when boosting
    private float currentSpeed;
    private bool isBoosting = false;

    private GameObject focalPoint;
    private bool isBraking = false; // Track if braking is active
    public float brakeForce = 5f; // Strength of the braking force

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // How hard to hit enemy without powerup
    private float powerupStrength = 25; // How hard to hit enemy with powerup

    private bool isFrozen = false; // Track if the player is frozen

    [Header("Freeze Effect")]
    public GameObject freezeEffect; // Visual effect when the player is frozen

    [Header("Ice Breaker Effect")]
    public GameObject iceBreakerEffect; // Visual effect when the player breaks free from freeze

    [Header("Turbo Settings")]
    private float maxTurbo = 100f; // Maximum Turbo value
    public float currentTurbo; // Current Turbo amount
    private float turboDrainRate = 20f; // How much turbo drains per second
    private float turboRechargeRate = 5f; // How much turbo regenerates per second
    public Slider turboBar; // UI Bar for Turbo

    [Header("Boost Effect")]
    public GameObject boostEffect; // Visual effect when boosting

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        currentSpeed = normalSpeed;
        currentTurbo = maxTurbo; // Start with full Turbo

        if (turboBar != null)
        {
            turboBar.maxValue = maxTurbo;
            turboBar.value = currentTurbo;
        }

        if (boostEffect != null) boostEffect.SetActive(false); // Hide effect at start

        powerupIndicator.SetActive(false);
        // Disable freeze and ice breaker effects at start
        if (freezeEffect != null) freezeEffect.SetActive(false);
        if (iceBreakerEffect != null) iceBreakerEffect.SetActive(false);
    }

    void Update()
    {
        if (isFrozen) return; // Skip movement if frozen

        HandleTurbo(); // Manage Turbo system

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
        playerRb.AddForce(movementDirection.normalized * currentSpeed * Time.deltaTime, ForceMode.Force);

        // Apply brake if active
        if (isBraking)
        {
            playerRb.linearVelocity *= (1 - brakeForce * Time.deltaTime); // Gradually slow down movement
        }
    }

    void HandleTurbo()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentTurbo > 0)
        {
            StartBoosting();
        }
        else
        {
            StopBoosting();
        }

        // Drain Turbo while boosting
        if (isBoosting && currentTurbo > 0)
        {
            currentTurbo -= turboDrainRate * Time.deltaTime;
            if (currentTurbo <= 0)
            {
                currentTurbo = 0;
                StopBoosting(); // Stop boosting if Turbo is empty
            }
        }
        // Regenerate Turbo when not boosting
        else if (!isBoosting && currentTurbo < maxTurbo)
        {
            currentTurbo += turboRechargeRate * Time.deltaTime;
            if (currentTurbo > maxTurbo) currentTurbo = maxTurbo;
        }

        // Update UI Turbo Bar
        if (turboBar != null) turboBar.value = currentTurbo;
    }

    void StartBoosting()
    {
        if (isBoosting) return; // Already boosting

        isBoosting = true;
        currentSpeed = boostSpeed;

        if (boostEffect != null) boostEffect.SetActive(true);
    }

    void StopBoosting()
    {
        if (!isBoosting) return; // Already stopped

        isBoosting = false;
        currentSpeed = normalSpeed;

        if (boostEffect != null) boostEffect.SetActive(false);
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JumpPowerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
        else if (other.gameObject.CompareTag("SlowMotionPowerup"))
        {
            Destroy(other.gameObject);
            FindObjectOfType<SlowMotionPowerup>().ActivateSlowMotion();
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

            if (hasPowerup) // If have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // If no powerup, hit enemy with normal strength 
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
        StartCoroutine(UnfreezePlayerAfterDelay(2f));
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
            StartCoroutine(DeactivateIceBreakerEffect(1f)); // Deactivate after 1 second
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
