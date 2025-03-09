using UnityEngine;

public class ClockPowerUp : MonoBehaviour
//Shahad
{
    public float timeBonus = 30f; // Amount of time to add (30 seconds)
    private Timer timerScript; // Reference to the Timer script

    [System.Obsolete]
    void Start()
    {
        // Find the Timer script in the scene (so we can modify the timer)
        timerScript = FindObjectOfType<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the clock
        if (other.CompareTag("Player"))
        {
            if (timerScript != null)
            {
                timerScript.AddTime(timeBonus); // Add 30 seconds to the timer
                Debug.Log("Clock collected! +30 seconds added."); // Log message
            }

            Destroy(gameObject); // Remove the clock from the game after collection
        }
    }
}

