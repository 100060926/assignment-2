using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGame0 : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    public Transform[] enemySpawnPoints; // Array of enemy spawn points
    public Transform[] powerupSpawnPoints; // Array of powerup spawn points

    public float spawnRadius = 3f; // Radius around the spawn point --Hour

    public int enemyCount;
    public int waveCount = 1;

    private EnemyX enemy;
    public GameObject player;
    public Transform playerSpawnPoint; // Transform for player reset position --Hour

    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        enemy = enemyPrefab.GetComponent<EnemyX>();

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        // Spawn a powerup if there are no powerups left
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            SpawnPowerup();
        }

        // Spawn enemies at designated spawn points
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }

        waveCount++;
        enemy.speed += 100;
        ResetPlayerPosition(); // Put player back at start --Hour
    }

    // Spawns an enemy at a random position within a radius of a spawn point --Hour
    void SpawnEnemy()
    {
        if (enemySpawnPoints.Length > 0)
        {
            Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            Vector3 randomPosition = GetRandomPositionAround(spawnPoint);
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No enemy spawn points assigned in SpawnGame0! --Hour");
        }
    }

    // Spawns a powerup at a random position within a radius of a spawn point --Hour
    void SpawnPowerup()
    {
        if (powerupSpawnPoints.Length > 0)
        {
            Transform spawnPoint = powerupSpawnPoints[Random.Range(0, powerupSpawnPoints.Length)];
            Vector3 randomPosition = GetRandomPositionAround(spawnPoint);
            Instantiate(powerupPrefab, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No powerup spawn points assigned in SpawnGame0! --Hour");
        }
    }

    // Generate a random position within a radius around a spawn point --Hour
    Vector3 GetRandomPositionAround(Transform spawnPoint)
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(0f, spawnRadius);
        float xOffset = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
        float zOffset = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;

        return spawnPoint.position + new Vector3(xOffset, 0, zOffset);
    }

    // Move player back to predefined spawn position --Hour
    void ResetPlayerPosition()
    {
        if (playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
            player.transform.rotation = playerSpawnPoint.rotation; // Ensures correct rotation --Hour
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Player spawn point is not assigned in SpawnGame0! --Hour");
        }
    }
}
