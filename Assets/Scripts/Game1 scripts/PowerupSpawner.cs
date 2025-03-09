using System.Collections;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject jumpPowerupPrefab; // Prefab for the jump powerup
    public Vector3 spawnArea = new Vector3(10f, 0f, 10f); // Area within which to spawn the powerup
    public float spawnInterval = 10f; // Time between spawns

    void Start()
    {
        // Start spawning powerups
        StartCoroutine(SpawnPowerups());
    }

    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Calculate a random position within the spawn area
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                0.5f, // Slightly above the ground
                Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
            );

            // Spawn the powerup
            Instantiate(jumpPowerupPrefab, spawnPosition, Quaternion.identity);
        }
    }
}