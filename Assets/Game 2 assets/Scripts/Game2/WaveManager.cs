using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Spawning Settings")]
    public GameObject enemyPrefab; // Assign the Kitty prefab
    public Transform[] spawnPoints; // 3 Spawn locations
    public Transform[] patrolPoints; // Shared patrol waypoints (optional)
    public Transform player; // Player reference
    public Text kittyCounterText; // Assign UI Text to display the number of Kitties

    public float spawnInterval = 10f; // Time between new waves

    void Start()
    {
        StartCoroutine(SpawnEnemies()); // Start the enemy wave system
        UpdateKittyCounterUI(); // Initialize UI with correct count
    }

    IEnumerator SpawnEnemies()
    {
        while (true) // Infinite loop to keep spawning
        {
            yield return new WaitForSeconds(spawnInterval); // Wait 10 seconds

            int spawnedKitties = 0; // Track how many kitties spawn this wave

            // Spawn an enemy from each spawn point
            foreach (Transform spawnPoint in spawnPoints)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                spawnedKitties++; // Increase the number of spawned Kitties

                // Set up the enemy's patrol points and player reference
                EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.player = player;
                    if (patrolPoints.Length > 0)
                    {
                        enemyAI.patrolPoints = patrolPoints;
                    }
                }
            }

            EnemyAI.IncreaseKittyCount(spawnedKitties); // Update static counter
            UpdateKittyCounterUI(); // Update UI text
            Debug.Log("Spawned " + spawnedKitties + " new Kitties! Total Now: " + EnemyAI.GetKittyCount());
        }
    }

    void UpdateKittyCounterUI()
    {
        if (kittyCounterText != null)
        {
            kittyCounterText.text = "Kitties: " + EnemyAI.GetKittyCount(); // Update UI text
        }
    }
}
