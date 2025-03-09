using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoublePointsPowerup : MonoBehaviour
{
    public float powerupDuration = 10f; // Duration of double points effect
    public Text doublePointsTimerText; // UI Timer display
    private GameManager gameManager;
    private bool isActive = false; // Prevents multiple activations

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Get reference to GameManager

        if (doublePointsTimerText != null)
        {
            doublePointsTimerText.gameObject.SetActive(false); // Hide timer at start
        }
    }

    // Detect when player collides with the powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ActivateDoublePoints();
            Destroy(gameObject); // Remove powerup object after activation
        }
    }

    public void ActivateDoublePoints()
    {
        if (!isActive)
        {
            StartCoroutine(DoublePointsEffect());
        }
    }

    private IEnumerator DoublePointsEffect()
    {
        isActive = true;
        Debug.Log("Double Points Activated!");

        // Activate Double Points in GameManager

        // Show timer UI
        if (doublePointsTimerText != null)
        {
            doublePointsTimerText.gameObject.SetActive(true);
        }

        float timeLeft = powerupDuration;

        // Countdown Timer
        while (timeLeft > 0)
        {
            if (doublePointsTimerText != null)
            {
                doublePointsTimerText.text = "Double Points: " + timeLeft.ToString("F1") + "s";
            }
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        // Reset back to normal
        Debug.Log("Double Points Effect Ended!");

        // Hide UI Timer
        if (doublePointsTimerText != null)
        {
            doublePointsTimerText.gameObject.SetActive(false);
        }

        isActive = false; // Allow reactivation later
    }
}
