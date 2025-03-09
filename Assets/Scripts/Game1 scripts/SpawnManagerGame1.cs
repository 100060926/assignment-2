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
    [Header("Powerup System")]
    public GameObject slowMotionPowerupPrefab;  // Slow Motion Powerup
    public GameObject jumpPowerupPrefab;        // Jump Powerup
    public Transform[] powerupSpawnCenters;     // Array of powerup spawn points
    public float powerupSpawnRadius = 5f;       // Range around the center where powerups can spawn

    private bool isPowerupActive = false;       // Track if a powerup is currently active
    private GameObject currentPowerup;          // Track the current active powerup

    private bool stopSpawning = false;        // Flag to control spawning
    private int currentWave = 0;              // Tracks the current wave
    private int maxWaves = 6;                 // Total number of waves (updated to 7)
    private int maxEnemiesOnField = 6;        // Max number of enemies on the field (updated to 7)

    private Coroutine waveCoroutine;          // Reference to the wave coroutine

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing in SpawnManagerGame1!");
            return;
        }

        waveCoroutine = StartCoroutine(StartWaves());
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
                    SpawnPowerup();
                    SpawnEnemy(enemyFollowPrefab, 3, usedSpawnPoints, 4.0f);
                    break;

                case 2: // Only Goal Followers (Increase Speed)
                    SpawnPowerup();
                    SpawnEnemy(enemyGoalPrefab, 3, usedSpawnPoints, 4.0f);
                    break;

                case 3: // Only Shielded Enemies (Increase Speed when Going to Goal)
                    SpawnPowerup();
                    SpawnEnemy(shieldedEnemyPrefab, 3, usedSpawnPoints, 3.5f, 4.5f);
                    break;

                case 4: // Only Speed Boosters
                    SpawnPowerup();
                    SpawnEnemy(speedBoosterPrefab, 3, usedSpawnPoints, 5.0f);
                    break;

                case 5: // Wave 5: 4 enemies (mix of all types)
                    SpawnPowerup();
                    SpawnMixedEnemies(4, usedSpawnPoints);
                    break;

                case 6: // Wave 6: 5 enemies (mix of all types)
                    SpawnPowerup();
                    SpawnMixedEnemies(5, usedSpawnPoints);
                    break;

                //case 7: // Wave 7: 7 enemies (mix of all types)
                    //SpawnMixedEnemies(7, usedSpawnPoints);
                    //break;
            }

            // Wait until all enemies are destroyed before moving to the next wave
            yield return new WaitUntil(() => AreAllEnemiesDestroyed());

            Debug.Log("Wave " + currentWave + " cleared.");

            // Check for game end condition
            if (gameManager.PlayerHasLost() || NoMoreWavesLeft())
            {
                HandleGameEnd();
                yield break; // Stop coroutine
            }
        }
    }

    void SpawnMixedEnemies(int count, List<int> usedSpawnPoints)
    {
        for (int i = 0; i < count && CountEnemies() < maxEnemiesOnField; i++)
        {
            int enemyType = Random.Range(0, 4);
            switch (enemyType)
            {
                case 0: SpawnEnemy(enemyFollowPrefab, 1, usedSpawnPoints, 4.5f); break;
                case 1: SpawnEnemy(enemyGoalPrefab, 1, usedSpawnPoints, 4.5f); break;
                case 2: SpawnEnemy(shieldedEnemyPrefab, 1, usedSpawnPoints, 4.0f, 5.0f); break;
                case 3: SpawnEnemy(speedBoosterPrefab, 1, usedSpawnPoints, 5.5f); break;
            }
        }

        Debug.Log("Spawned " + count + " mixed enemies in Wave " + currentWave);
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
        // Only count active enemies
        return FindObjectsOfType<EnemyPlayerFollower>().Length +
               FindObjectsOfType<EnemyGoalFollower>().Length +
               FindObjectsOfType<ShieldedEnemy>().Length +
               FindObjectsOfType<SpeedBoosterEnemy>().Length == 0;
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
        if (stopSpawning) return; // Prevent multiple calls

        stopSpawning = true;

        // Stop the wave coroutine
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }

        Debug.Log("Game Over! Checking win/loss...");

        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing in SpawnManagerGame1!");
            return;
        }

        if (gameManager.PlayerHasLost()) // Check if player lost
        {
            Debug.Log("Player Lost! Game Over.");
            gameManager.EndGame(false);
        }
        else
        {
            Debug.Log("Player Won! Survived all waves.");
            gameManager.EndGame(true);
        }
    }
    void SpawnPowerup()
    {
        if (isPowerupActive || powerupSpawnCenters.Length == 0) return; // Prevent multiple active powerups

        // Choose a random spawn point
        Transform chosenSpawnCenter = powerupSpawnCenters[Random.Range(0, powerupSpawnCenters.Length)];

        // Generate a random X and Z position within the defined radius
        float randomX = chosenSpawnCenter.position.x + Random.Range(-powerupSpawnRadius, powerupSpawnRadius);
        float fixedY = -43.8f; // Keep Y fixed 
        float randomZ = chosenSpawnCenter.position.z + Random.Range(-powerupSpawnRadius, powerupSpawnRadius);

        Vector3 spawnPosition = new Vector3(randomX, fixedY, randomZ);

        // Randomly select a powerup type (50% chance each)
        GameObject powerupToSpawn = Random.value < 0.5f ? slowMotionPowerupPrefab : jumpPowerupPrefab;

        // Instantiate the powerup and track it
        currentPowerup = Instantiate(powerupToSpawn, spawnPosition, Quaternion.identity);
        isPowerupActive = true;
    }

    public void PowerupCollected()
    {
        isPowerupActive = false;  // Allow new powerup to spawn in the next wave
        currentPowerup = null;     // Clear reference to current powerup
    }

}