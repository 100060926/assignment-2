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
    private int maxWaves = 5;                 // Total number of waves
    private int maxEnemiesOnField = 5;        // Max number of enemies on the field

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

            List<int> usedSpawnPoints = new List<int>();

            switch (currentWave)
            {
                case 1: // Only Enemy Followers (Increase Speed)
                    SpawnEnemy(enemyFollowPrefab, 3, usedSpawnPoints, 4.0f);
                    break;

                case 2: // Only Goal Followers (Increase Speed)
                    SpawnEnemy(enemyGoalPrefab, 3, usedSpawnPoints, 4.0f);
                    break;

                case 3: // Only Shielded Enemies (Increase Speed when Going to Goal)
                    SpawnEnemy(shieldedEnemyPrefab, 3, usedSpawnPoints, 3.5f, 4.5f);
                    break;

                case 4: // Only Speed Boosters
                    SpawnEnemy(speedBoosterPrefab, 3, usedSpawnPoints, 5.0f);
                    break;

                case 5: // Mix of 5 enemies at a time (3 rounds)
                    for (int i = 0; i < 3; i++) // Repeat 3 times
                    {
                        int totalEnemies = 5;
                        while (totalEnemies > 0 && CountEnemies() < maxEnemiesOnField)
                        {
                            int enemyType = Random.Range(0, 4);
                            switch (enemyType)
                            {
                                case 0: SpawnEnemy(enemyFollowPrefab, 1, usedSpawnPoints, 4.5f); break;
                                case 1: SpawnEnemy(enemyGoalPrefab, 1, usedSpawnPoints, 4.5f); break;
                                case 2: SpawnEnemy(shieldedEnemyPrefab, 1, usedSpawnPoints, 4.0f, 5.0f); break;
                                case 3: SpawnEnemy(speedBoosterPrefab, 1, usedSpawnPoints, 5.5f); break;
                            }
                            totalEnemies--;
                        }
                        yield return new WaitForSeconds(10f); // Delay between rounds
                    }
                    break;
            }

            // Wait until all enemies are destroyed before moving to the next wave
            yield return new WaitUntil(() => AreAllEnemiesDestroyed());

            Debug.Log("Wave " + currentWave + " cleared.");
        }

        // End game when no more waves
        Debug.Log("Game Over! Checking if player won...");
        HandleGameEnd();
    }

    void SpawnEnemy(GameObject prefab, int count, List<int> usedSpawnPoints, float speed = 3.0f, float goalSpeed = -1)
    {
        for (int i = 0; i < count && CountEnemies() < maxEnemiesOnField; i++)
        {
            int spawnIndex;
            do
            {
                spawnIndex = Random.Range(0, spawnPoints.Length);
            } while (usedSpawnPoints.Contains(spawnIndex)); // Ensure different spawn points

            usedSpawnPoints.Add(spawnIndex);
            Transform spawnPoint = spawnPoints[spawnIndex];

            GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            if (enemy.TryGetComponent(out EnemyGoalFollower goalSeeker))
            {
                goalSeeker.speed = speed;
                if (goalSpeed > 0) goalSeeker.goalSpeed = goalSpeed; // Set faster speed when moving to goal
            }
            else if (enemy.TryGetComponent(out EnemyPlayerFollower follower))
            {
                follower.speed = speed;
            }
            else if (enemy.TryGetComponent(out ShieldedEnemy shielded))
            {
                shielded.speed = speed;
                if (goalSpeed > 0) shielded.goalSpeed = goalSpeed;
            }
            else if (enemy.TryGetComponent(out SpeedBoosterEnemy booster))
            {
                booster.speed = speed;
            }
        }

        Debug.Log("Spawned " + count + " " + prefab.name);
    }

    public void StopSpawning()
    {
        stopSpawning = true;
        Debug.Log("Spawning stopped.");
    }

    public bool AreAllEnemiesDestroyed()
    {
        return CountEnemies() == 0;
    }

    int CountEnemies()
    {
        return FindObjectsOfType<EnemyPlayerFollower>().Length +
               FindObjectsOfType<EnemyGoalFollower>().Length +
               FindObjectsOfType<ShieldedEnemy>().Length +
               FindObjectsOfType<SpeedBoosterEnemy>().Length;
    }

    public bool NoMoreWavesLeft()
    {
        return currentWave >= maxWaves;
    }

    void HandleGameEnd()
    {
        if (gameManager.PlayerHasLost()) // Check if player lost
        {
            Debug.Log("Player Lost! Game Over.");
            gameManager.GameOver(false);
        }
        else
        {
            Debug.Log("Player Won! Survived all waves.");
            gameManager.GameOver(true);
        }

        StopSpawning();
    }
}
