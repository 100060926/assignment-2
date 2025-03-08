using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerGame1 : MonoBehaviour
{
    public GameObject enemyFollowPrefab;      // Prefab for enemies that follow the player
    public GameObject enemyGoalPrefab;        // Prefab for enemies that move toward the goal
    public GameObject shieldedEnemyPrefab;    // Prefab for Shielded Enemies
    public GameObject speedBoosterPrefab;     // Prefab for Speed Booster (Dasher)
    public Transform[] spawnPoints;           // Array of spawn points
    public GameManager gameManager;           // Reference to GameManager

    private bool stopSpawning = false;        // Flag to control spawning
    private int currentWave = 0;              // Tracks the current wave
    private int maxWaves = 8;                 // Total number of waves
    private int maxEnemiesOnField = 7;        // Max number of enemies on the field (wave 8)

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f); // Small delay before starting

        while (!stopSpawning && currentWave < maxWaves)
        {
            currentWave++;
            Debug.Log("Starting Wave " + currentWave);

            // Ensure enemies spawn from different locations
            List<int> usedSpawnPoints = new List<int>();

            switch (currentWave)
            {
                case 1:
                    SpawnEnemy(enemyFollowPrefab, 2, usedSpawnPoints);
                    break;
                case 2:
                    SpawnEnemy(enemyFollowPrefab, 2, usedSpawnPoints);
                    SpawnEnemy(enemyGoalPrefab, 1, usedSpawnPoints);
                    break;
                case 3:
                    SpawnEnemy(enemyFollowPrefab, 2, usedSpawnPoints);
                    SpawnEnemy(enemyGoalPrefab, 2, usedSpawnPoints);
                    break;
                case 4:
                    SpawnEnemy(enemyFollowPrefab, 2, usedSpawnPoints);
                    SpawnEnemy(enemyGoalPrefab, 3, usedSpawnPoints);
                    break;
                case 5:
                    SpawnEnemy(enemyFollowPrefab, 1, usedSpawnPoints);
                    SpawnEnemy(enemyGoalPrefab, 4, usedSpawnPoints);
                    break;
                case 6: // Introduce Shielded Enemies
                    SpawnEnemy(shieldedEnemyPrefab, 3, usedSpawnPoints);
                    break;
                case 7: // Introduce Speed Booster (Dasher)
                    SpawnEnemy(speedBoosterPrefab, 3, usedSpawnPoints);
                    break;
                case 8:
                    while (FindObjectsOfType<ShieldedEnemy>().Length + FindObjectsOfType<SpeedBoosterEnemy>().Length < maxEnemiesOnField)
                    {
                        int enemyType = Random.Range(0, 4);
                        switch (enemyType)
                        {
                            case 0: SpawnEnemy(enemyFollowPrefab, 1, usedSpawnPoints); break;
                            case 1: SpawnEnemy(enemyGoalPrefab, 1, usedSpawnPoints); break;
                            case 2: SpawnEnemy(shieldedEnemyPrefab, 1, usedSpawnPoints); break;
                            case 3: SpawnEnemy(speedBoosterPrefab, 1, usedSpawnPoints); break;
                        }
                        yield return new WaitForSeconds(1.5f);
                    }
                    break;
            }

            // Wait until all enemies are destroyed before moving to the next wave
            yield return new WaitUntil(() => AreAllEnemiesDestroyed());

            Debug.Log("Wave " + currentWave + " cleared. Moving to next wave...");
        }

        Debug.Log("All waves completed. Player wins!");
        HandlePlayerWin(); // Trigger player win condition
    }

    void SpawnEnemy(GameObject prefab, int count, List<int> usedSpawnPoints)
    {
        for (int i = 0; i < count; i++)
        {
            int spawnIndex;
            do
            {
                spawnIndex = Random.Range(0, spawnPoints.Length);
            } while (usedSpawnPoints.Contains(spawnIndex)); // Ensure different spawn points

            usedSpawnPoints.Add(spawnIndex);
            Transform spawnPoint = spawnPoints[spawnIndex];

            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }

        Debug.Log("Spawned " + count + " " + prefab.name);
    }

    // Method to stop spawning when the game ends
    public void StopSpawning()
    {
        stopSpawning = true;
        Debug.Log("Spawning stopped.");
    }

    // Checks if all enemies are destroyed
    bool AreAllEnemiesDestroyed()
    {
        return FindObjectsOfType<EnemyPlayerFollower>().Length == 0 &&
               FindObjectsOfType<EnemyGoalFollower>().Length == 0 &&
               FindObjectsOfType<ShieldedEnemy>().Length == 0 &&
               FindObjectsOfType<SpeedBoosterEnemy>().Length == 0;
    }

    // Handles Player Win Condition
    void HandlePlayerWin()
    {
        stopSpawning = true;
        DisableAllEnemies();

        if (gameManager.gameOverText != null)
        {
            gameManager.gameOverText.gameObject.SetActive(true);
            gameManager.gameOverText.text = "Victory! You Survived All Waves!";
        }
        else
        {
            Debug.LogError("Game Over Text UI reference is missing!");
        }

        Debug.Log("Player Wins! All waves survived.");
    }

    void DisableAllEnemies()
    {
        EnemyPlayerFollower[] playerFollowers = FindObjectsOfType<EnemyPlayerFollower>();
        EnemyGoalFollower[] goalSeekers = FindObjectsOfType<EnemyGoalFollower>();
        ShieldedEnemy[] shieldedEnemies = FindObjectsOfType<ShieldedEnemy>();
        SpeedBoosterEnemy[] speedBoosters = FindObjectsOfType<SpeedBoosterEnemy>();

        foreach (EnemyPlayerFollower enemy in playerFollowers)
            enemy.enabled = false;

        foreach (EnemyGoalFollower enemy in goalSeekers)
            enemy.enabled = false;

        foreach (ShieldedEnemy enemy in shieldedEnemies)
            enemy.enabled = false;

        foreach (SpeedBoosterEnemy enemy in speedBoosters)
            enemy.enabled = false;

        Debug.Log("Disabled all active enemies.");
    }
}
