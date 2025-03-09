using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public Transform[] patrolPoints;
    public Transform player;
    public Text kittyCounterText;

    public float spawnInterval = 10f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        UpdateKittyCounterUI();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnPoints.Length == 0)
            {
                Debug.LogError("No spawn points available! Cannot spawn enemies.");
                continue;
            }

            int spawnedKitties = 0;

            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint == null) continue;

                GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                spawnedKitties++;

                EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.player = player;
                    if (patrolPoints.Length > 0) enemyAI.patrolPoints = patrolPoints;
                }
            }

            // Ensure the counter updates correctly
            UpdateKittyCounterUI();
        }
    }

    void UpdateKittyCounterUI()
    {
        if (kittyCounterText != null)
        {
            kittyCounterText.text = "Kitties: " + EnemyAI.GetKittyCount();
        }
    }
}
