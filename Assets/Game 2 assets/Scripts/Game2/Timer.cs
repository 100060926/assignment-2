using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
//Shahad
{
    public Text timerText; // UI text that shows the countdown
    public GameObject gameOverPanel; // Panel that appears when time runs out
    public GameObject youWinPanel; // Panel that appears when the player wins

    private float timeRemaining = 120f; // Timer starts at 2 minutes (120 seconds)
    private bool timerRunning = true; // Controls whether the timer is active
    private Scores scoreManager; // Reference to the Scores script

    [System.Obsolete]
    void Start()
    {
        gameOverPanel.SetActive(false); // Hide Game Over panel initially
        youWinPanel.SetActive(false); // Hide You Win panel initially
        scoreManager = FindObjectOfType<Scores>(); // Find the Scores script in the scene
    }

    void Update()
    {
        if (timerRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Reduce timer every second
            UpdateTimerUI(); // Update the displayed time

            // If the player collects enough balls before time runs out, they win
            if (scoreManager.HasPlayerWon())
            {
                timerRunning = false;
                ShowYouWinPanel();
            }
        }
        else if (timeRemaining <= 0 && timerRunning) // If time runs out
        {
            timeRemaining = 0;
            timerRunning = false;
            CheckGameOverCondition();
        }
    }

    private void UpdateTimerUI()
    {
        // Convert time to MM:SS format (example: 02:00)
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CheckGameOverCondition()
    {
        if (!scoreManager.HasPlayerWon())
        {
            Debug.Log("Game Over! Time's Up!");
            gameOverPanel.SetActive(true); // Show Game Over UI
        }
        else
        {
            ShowYouWinPanel(); // Show win screen if player meets the score goal
        }
    }

    private void ShowYouWinPanel()
    {
        Debug.Log("You Win!");
        youWinPanel.SetActive(true); // Show You Win UI
    }

    public void RestartGame()
    {
        timeRemaining = 120f; // Reset timer to 2 minutes
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    // This method allows other scripts (like the clock) to add extra time
    public void AddTime(float extraTime)
    {
        timeRemaining += extraTime; // Increase the timer by the given amount
        Debug.Log("Time increased by " + extraTime + " seconds!"); // Log message
    }
}
