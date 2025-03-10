using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Shield : MonoBehaviour
{
    float rotationSpeed = 45;
    float floatingSpeed = 2;
    float height = 0.5f;
    Vector3 position;
    SpawnManagerXX spawnManager;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
    }

    // Update is called once per frame
    void Update()
    {
        float pos = transform.position.y;
        float newY = Mathf.Sin(Time.time * floatingSpeed);
        transform.position = new Vector3(position.x, position.y +(newY * height), position.z);
        transform.Rotate(Vector3.up, rotationSpeed*Time.deltaTime);
        if (spawnManager.isGameOver()) {
            if (transform.position.z > 0){
                Destroy(gameObject);
            }
        }
    }
}
