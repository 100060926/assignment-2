using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
//Shahad
{
    private Scores scoreManager;
    private Timer timerManager;

    [System.Obsolete]
    void Start()
    {
        scoreManager = FindObjectOfType<Scores>(); // Find the score manager in the scene
        timerManager = FindObjectOfType<Timer>(); // Find the timer manager in the scene
    }

    public void OnYesButton() // When "Yes" is clicked in Game Over panel
    {
        ResetGame(); // Reset everything and restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current level
    }

    public void OnNoButton() // When "No" is clicked in Game Over panel
    {
        SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
    }

    public void OnMainMenuButton() // When "Main Menu" is clicked in You Win panel
    {
        SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
    }

    private void ResetGame()
    {
        if (scoreManager != null) scoreManager.ResetScores(); // Reset scores
        if (timerManager != null) timerManager.RestartGame(); // Reset timer
    }
}
