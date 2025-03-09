using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;  // Score for the player
    public int enemyScore = 0;   // Score for the enemy
    private int playerGoalCount = 0; // Count of how many balls entered the Player Goal

    public int gameOverThreshold = 5; // Number of balls needed to end the game

    // UI Elements
    public Text playerScoreText;       // UI Text for Player Score
    public Text enemyScoreText;        // UI Text for Enemy Score

    public GameObject gameUIPanel;     // Game UI (Score, Turbo, etc.)
    public GameObject gameOverPanel;   // Game Over UI
    public GameObject victoryPanel;    // Victory UI

    public Text gameOverScoreText;     // Final score display on Game Over screen
    public Text victoryScoreText;      // Final score display on Victory screen

    public Button retryButton;         // Retry button (Game Over)
    public Button menuButton;          // Menu button (Game Over)
    public Button victoryMenuButton;   // Menu button (Victory)
    public Button victoryRetryButton;  // Retry button (Victory)

    public SpawnManagerGame1 spawnManager; // Reference to the Spawn Manager

    private bool gameOver = false; // Tracks if the game is over
    private bool gameStarted = false; // Ensures the game starts properly

    void Start()
    {
        Debug.Log("Game Initialized. Waiting for Start...");

        // Hide all UI at start
        if (gameUIPanel != null) gameUIPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void OnGameStart()
    {
        if (gameStarted) return; // Prevent multiple calls
        gameStarted = true;

        Debug.Log("Game Started! Scores initialized.");
        UpdateScoreUI();

        // Activate the game UI
        if (gameUIPanel != null) gameUIPanel.SetActive(true);
    }

    public void AddScore(int points, string scorer)
    {
        if (gameOver)
        {
            Debug.LogWarning("Score update attempted after game over! Ignored.");
            return; // Prevents scoring after the game ends
        }

        if (scorer == "Player")
        {
            playerScore += points;
            Debug.Log("Player scored! Current Player Score: " + playerScore);
        }
        else if (scorer == "Enemy")
        {
            enemyScore += points;
            playerGoalCount += points; // Track how many balls entered the player's goal
            Debug.Log("Enemy scored! Current Enemy Score: " + enemyScore + " | Player Goal Count: " + playerGoalCount);
        }

        UpdateScoreUI();
        CheckForGameEnd(); // Check if the game should end after updating the score
    }

    void UpdateScoreUI()
    {
        if (playerScoreText != null) playerScoreText.text = " " + playerScore;
        if (enemyScoreText != null) enemyScoreText.text = " " + enemyScore;
    }

    public void CheckForGameEnd()
    {
        if (gameOver) return; // Prevent checking if the game is already over

        if (playerGoalCount >= gameOverThreshold)
        {
            EndGame(false); // Player loses if too many balls enter their goal
        }
        else if (spawnManager != null && spawnManager.AreAllEnemiesDestroyed())
        {
            EndGame(true); // Player wins if all waves are completed and no enemies remain
        }
    }

    public void EndGame(bool playerWon)
    {
        if (gameOver) return; // Prevent multiple calls
        gameOver = true;
        Time.timeScale = 0f; // Pause the game

        Debug.Log("Game Over! " + (playerWon ? "Player Wins!" : "Enemy Wins!"));

        // Stop enemy spawning
        if (spawnManager != null)
        {
            Debug.Log("Stopping enemy spawning...");
            spawnManager.StopSpawning();
        }
        else
        {
            Debug.LogWarning("SpawnManager reference is missing!");
        }

        // Stop all enemy movement
        DisableAllEnemies();

        // Display final score and show the correct UI panel
        if (playerWon)
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
                if (victoryScoreText != null)
                    victoryScoreText.text = "" + playerScore;
            }
        }
        else
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (gameOverScoreText != null)
                    gameOverScoreText.text = "" + playerScore;
            }
        }
    }

    void DisableAllEnemies()
    {
        EnemyPlayerFollower[] playerFollowers = FindObjectsOfType<EnemyPlayerFollower>();
        EnemyGoalFollower[] goalSeekers = FindObjectsOfType<EnemyGoalFollower>();
        ShieldedEnemy[] shieldedEnemies = FindObjectsOfType<ShieldedEnemy>();
        SpeedBoosterEnemy[] speedBoosters = FindObjectsOfType<SpeedBoosterEnemy>();

        foreach (EnemyPlayerFollower enemy in playerFollowers) enemy.enabled = false;
        foreach (EnemyGoalFollower enemy in goalSeekers) enemy.enabled = false;
        foreach (ShieldedEnemy enemy in shieldedEnemies) enemy.enabled = false;
        foreach (SpeedBoosterEnemy enemy in speedBoosters) enemy.enabled = false;

        Debug.Log("Disabled all active enemies.");
    }

    public bool PlayerHasLost()
    {
        return playerGoalCount >= gameOverThreshold;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Resume game
        SceneManager.LoadScene("MainMenu"); 
    }

    void OnEnable()
    {
        if (retryButton != null) retryButton.onClick.AddListener(RestartGame);
        if (menuButton != null) menuButton.onClick.AddListener(GoToMainMenu);
        if (victoryRetryButton != null) victoryRetryButton.onClick.AddListener(RestartGame);
        if (victoryMenuButton != null) victoryMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void OnDisable()
    {
        if (retryButton != null) retryButton.onClick.RemoveListener(RestartGame);
        if (menuButton != null) menuButton.onClick.RemoveListener(GoToMainMenu);
        if (victoryRetryButton != null) victoryRetryButton.onClick.RemoveListener(RestartGame);
        if (victoryMenuButton != null) victoryMenuButton.onClick.RemoveListener(GoToMainMenu);
    }
}
