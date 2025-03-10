using UnityEngine;

public class DestroyAfterGame : MonoBehaviour
{
    SpawnManagerXX spawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isGameOver()) {
            if (transform.position.z > 0){
                Destroy(gameObject);
            }
        }
    }
}
