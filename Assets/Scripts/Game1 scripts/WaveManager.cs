using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyFollowerPrefab;
    public GameObject enemyGoalSeekerPrefab;
    public Transform spawnPoint;
    public GameManager gameManager;

    private int playerGoalCount = 0; // Tracks how many balls entered the player's goal
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f); // Small delay before starting

        while (currentWave < 5)
        {
            currentWave++;

            switch (currentWave)
            {
                case 1:
                    SpawnEnemy(enemyFollowerPrefab, 2);
                    break;
                case 2:
                    SpawnEnemy(enemyFollowerPrefab, 2);
                    SpawnEnemy(enemyGoalSeekerPrefab, 1, 2.0f); // Slow-moving ball to goal
                    break;
                case 3:
                    SpawnEnemy(enemyFollowerPrefab, 2);
                    SpawnEnemy(enemyGoalSeekerPrefab, 2, 2.0f);
                    break;
                case 4:
                    SpawnEnemy(enemyFollowerPrefab, 2);
                    SpawnEnemy(enemyGoalSeekerPrefab, 3, 3.0f);
                    break;
                case 5:
                    SpawnEnemy(enemyFollowerPrefab, 1, 4.0f); // Faster speed
                    SpawnEnemy(enemyGoalSeekerPrefab, 4, 4.0f);
                    break;
            }

            yield return new WaitForSeconds(10f); // Delay between waves
        }
    }

    void SpawnEnemy(GameObject prefab, int count, float speed = 3.0f)
    {
        for (int i = 0; i < count; i++)
        {
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
    }

    public void BallScored()
    {
        playerGoalCount++;
        if (playerGoalCount >= 5)
        {
            Debug.Log("Game Over! 5 balls entered the player's goal.");
            // Add game-over logic here (disable input, show UI, etc.)
        }
    }
}
