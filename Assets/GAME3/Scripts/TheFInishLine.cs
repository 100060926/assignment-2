using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class The : MonoBehaviour
{
    Vector3 position1 = new Vector3(35,13,130);
    Vector3 position2 = new Vector3(60,13,90);
    Vector3 position3 = new Vector3(15,13,130);
    quaternion rotation1 = new quaternion(0,270,0,0);
    quaternion rotation2 = new quaternion(0,0,0,0);
    quaternion rotation3 = new quaternion(0,270,0,0);
    Vector3[] positions = new Vector3[3];
    quaternion[] rotations = new quaternion[3];
    int index = 0;
    Vector3 finish = new Vector3(-100,2,47.5f);
    SpawnManagerXX spawnManager;
    timer timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positions[0] = position1;
        positions[1] = position2;
        positions[2] = position3;
        rotations[0] = rotation1;
        rotations[1] = rotation2;
        rotations[2] = rotation3;
        transform.position = positions[index];
        transform.rotation = rotations[index];
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
        timer = GameObject.Find("Game Manager").GetComponent<timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            index++;
            if (index <= 2) {
                transform.position = positions[index];
                transform.rotation = rotations[index];
                spawnManager.addWaveCount();
            } else {
                spawnManager.gameIsOver();
                timer.StopTimer();
                other.transform.position = finish;
                index = 0;
            }
        }
    }
}
