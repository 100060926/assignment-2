using System.Collections;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject slowMotionPowerupPrefab;  // Prefab for Slow Motion Powerup
    public GameObject jumpPowerupPrefab;        // Prefab for Jump Powerup
    public Transform[] spawnCenters;            // Array of spawn points

    public float spawnRadius = 5f;              // Radius around the spawn point where powerups can spawn
    public float spawnHeight = 1.0f;            // Height at which powerups will spawn

    private bool isPowerupActive = false;       // Track if a powerup is currently active
    private GameObject currentPowerup;          // Track the current active powerup

    void Start()
    {
        if (spawnCenters.Length == 0)
        {
            Debug.LogError("No spawn points assigned in PowerupSpawner!");
        }
    }

    public void SpawnPowerup()
    {
        if (isPowerupActive || spawnCenters.Length == 0) return; // Prevent spawning if a powerup is active or no spawn points

        // Select a random spawn center from the available spawn points
        Transform chosenSpawnCenter = spawnCenters[Random.Range(0, spawnCenters.Length)];

        // Generate a random position within the spawnRadius around the chosen spawn point
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0; // Keep it on the same plane

        Vector3 spawnPosition = chosenSpawnCenter.position + randomOffset;
        spawnPosition.y = spawnHeight; // Adjust height

        // Randomly choose between Slow Motion and Jump Powerup
        GameObject powerupToSpawn = Random.value < 0.5f ? slowMotionPowerupPrefab : jumpPowerupPrefab;

        // Spawn the powerup and track it
        currentPowerup = Instantiate(powerupToSpawn, spawnPosition, Quaternion.identity);
        isPowerupActive = true;
    }

    public void PowerupCollected()
    {
        isPowerupActive = false;  // Allow new powerup to spawn in the next wave
        currentPowerup = null;     // Clear reference to current powerup
    }
}
