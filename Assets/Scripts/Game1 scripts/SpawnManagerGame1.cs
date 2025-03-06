using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerGame1 : MonoBehaviour
{
    public GameObject enemyFollowPrefab; // Prefab for enemies that follow the player
    public GameObject enemyGoalPrefab;   // Prefab for enemies that move toward the goal
    public Transform[] spawnPoints;      // Array of spawn points
    public GameManager gameManager;      // Reference to GameManager

    private bool stopSpawning = false;   // Flag to control spawning
    private int currentWave = 0;         // Tracks the current wave
    private float waveDelay = 10f;       // Time delay between waves

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f); // Small delay before starting

        while (!stopSpawning && currentWave < 5)
        {
            currentWave++;
            Debug.Log("Starting Wave " + currentWave);

            switch (currentWave)
            {
                case 1:
                    SpawnEnemy(enemyFollowPrefab, 2);
                    break;
                case 2:
                    SpawnEnemy(enemyFollowPrefab, 2);
                    SpawnEnemy(enemyGoalPrefab, 1, 2.0f); // Slower goal follower
                    break;
                case 3:
                    SpawnEnemy(enemyFollowPrefab, 2);
                    SpawnEnemy(enemyGoalPrefab, 2, 2.0f);
                    break;
                case 4:
                    SpawnEnemy(enemyFollowPrefab, 2);
                    SpawnEnemy(enemyGoalPrefab, 3, 3.0f);
                    break;
                case 5:
                    SpawnEnemy(enemyFollowPrefab, 1, 4.0f); // Faster chaser
                    SpawnEnemy(enemyGoalPrefab, 4, 4.0f); // Faster goal seekers
                    break;
            }

            yield return new WaitForSeconds(waveDelay);
        }

        Debug.Log("All waves completed. No more enemies will spawn.");
    }

    void SpawnEnemy(GameObject prefab, int count, float speed = 3.0f)
    {
        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            if (enemy.TryGetComponent(out EnemyGoalFollower goalSeeker))
            {
                goalSeeker.speed = speed;
            }
            else if (enemy.TryGetComponent(out EnemyPlayerFollower follower))
            {
                follower.speed = speed;
            }
        }

        Debug.Log("Spawned " + count + " " + prefab.name + "(s) with speed: " + speed);
    }

    // Method to stop spawning when the game ends
    public void StopSpawning()
    {
        stopSpawning = true;
        Debug.Log("Spawning stopped by GameManager.");
    }
}
