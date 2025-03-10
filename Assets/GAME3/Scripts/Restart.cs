using UnityEngine;

public class Restart : MonoBehaviour
{
    SpawnManagerXX spawnManager; 
    PlayerControllerX pc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
        pc = GameObject.Find("Player").GetComponent<PlayerControllerX>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
