using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the Pause Menu UI Panel
    private bool isPaused = false; // Track if the game is paused

    void Update()
    {
        // Toggle pause when the player presses the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Function to pause the game
    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Pause the game
    }

    // Function to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume the game
    }

    // Function to go back to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure game resumes before loading main menu
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}
