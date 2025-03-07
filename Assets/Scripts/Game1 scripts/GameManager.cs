using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;  // Score for the player
    public int enemyScore = 0;   // Score for the enemy
    private int playerGoalCount = 0; // Count of how many balls entered the Player Goal

    public int gameOverThreshold = 5; // Number of balls needed to end the game
    private int maxWaves = 5;         // Total waves in the game
    private int currentWave = 0;      // Tracks the current wave

    public Text playerScoreText; // UI Text for Player Score
    public Text enemyScoreText;  // UI Text for Enemy Score
    public Text gameOverText;    // UI Text for Game Over message

    public SpawnManagerGame1 spawnManager; // Reference to the Spawn Manager

    private bool gameOver = false; // Tracks if the game is over

    void Start()
    {
        Debug.Log("Game Started! Scores initialized.");
        UpdateScoreUI();

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false); // Hide Game Over message at start
        else
            Debug.LogError("GameOverText UI reference is missing in the Inspector!");
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

            if (playerGoalCount >= gameOverThreshold)
            {
                EndGame(false); // Player loses
            }
        }

        UpdateScoreUI();
        CheckForWinCondition();
    }

    void UpdateScoreUI()
    {
        if (playerScoreText != null && enemyScoreText != null)
        {
            playerScoreText.text = "Player Score: " + playerScore;
            enemyScoreText.text = "Enemy Score: " + enemyScore;
        }
        else
        {
            Debug.LogError("UI Text references are missing in the GameManager!");
        }
    }

    public void WaveCompleted()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " completed.");

        if (currentWave >= maxWaves)
        {
            CheckForWinCondition();
        }
    }

    void CheckForWinCondition()
    {
        if (currentWave >= maxWaves && AreAllEnemiesDestroyed())
        {
            EndGame(true); // Player wins
        }
    }

    void EndGame(bool playerWon)
    {
        gameOver = true;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = playerWon ? "Victory! You Survived All Waves!" : "Game Over! Enemy Wins!";
        }
        else
        {
            Debug.LogError("Game Over Text UI reference is missing!");
        }

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

        Debug.Log(playerWon ? "Player Wins! All waves survived." : "Game Over! The enemy has won.");
    }

    void DisableAllEnemies()
    {
        EnemyPlayerFollower[] playerFollowers = FindObjectsOfType<EnemyPlayerFollower>();
        EnemyGoalFollower[] goalSeekers = FindObjectsOfType<EnemyGoalFollower>();

        foreach (EnemyPlayerFollower enemy in playerFollowers)
        {
            enemy.enabled = false;
        }

        foreach (EnemyGoalFollower enemy in goalSeekers)
        {
            enemy.enabled = false;
        }

        Debug.Log("Disabled all active enemies.");
    }

    bool AreAllEnemiesDestroyed()
    {
        return FindObjectsOfType<EnemyPlayerFollower>().Length == 0 && FindObjectsOfType<EnemyGoalFollower>().Length == 0;
    }
}
