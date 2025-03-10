using System;
using System.Collections;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class SpawnManagerXX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spinningBlade;
    public GameObject runningBall;
    public GameObject explodingBall;
    public GameObject spikes;
    public GameObject shield;
    Vector3 spawnLogPos1 = new Vector3(22,13,-88.6f);
    Vector3 spawnLogPos2 = new Vector3(22,13,-86.3f);
    float spawnXS = 10;
    float spawnXF = 60;
    float spawnZS = 85;
    float spawnZF = 125;
    bool swish = true;
    float startTime = 2;
    float repeatTime = 2;
    int waveCount = 1;
    public bool gameOver = false;
    public bool ObstaclesSpawned = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!gameOver) {
            InvokeRepeating("spawnLogs", startTime, repeatTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ObstaclesSpawned && !gameOver) {
            for (int i = 0; i<3; i++){
                Instantiate(spinningBlade, GenerateSpawnPosition(), new quaternion(0,0,0,0));
                Instantiate(runningBall, GenerateSpawnPosition(), new quaternion(0,0,0,0));
                Instantiate(explodingBall, GenerateSpawnPosition() + new Vector3(0,0.8f), new quaternion(0,0,0,0));
                Instantiate(spikes, GenerateSpawnPosition() + new Vector3(0,0.1f), new quaternion(0,0,0,0));
            }
            Instantiate(shield, GenerateSpawnPosition() + new Vector3(0,1), new quaternion(0,0,0,0));
            ObstaclesSpawned = true;
        }
    }

    void spawnLogs() {
        if (swish) {
            Instantiate(enemyPrefab, spawnLogPos1, new quaternion(0,0,0,0));
            swish = !swish;
        } else if(!swish) {
            Instantiate(enemyPrefab, spawnLogPos2, new quaternion(0,0,0,0));
            swish = !swish;
        }
    }
    
    Vector3 GenerateSpawnPosition ()
    {
        float xPos = Random.Range(spawnXS, spawnXF);
        float zPos = Random.Range(spawnZS, spawnZF);
        return new Vector3(xPos, 13, zPos);
    }
    public void addWaveCount() {
        waveCount++;
        ObstaclesSpawned = false;
    }
    public void gameIsOver(){
        ObstaclesSpawned = false;
        gameOver = true;
        waveCount = 1;
    }
    public void restartTheGame() {
        gameOver = false;
    }
    public bool isGameOver() {
        return gameOver;
    }
}
