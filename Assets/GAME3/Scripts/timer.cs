using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class timer : MonoBehaviour
{
    PlayerControllerA pc;
    GameObject player;
    private int timeElapsed = 0;
    bool timerStarted;
    public TMP_Text timerText;
    public int bestTime = 0;
    SpawnManagerXX spawnManager;  
    public int timeStart;
    public int timef;
    UI ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerStarted = true;
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControllerA>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
        timeStart = (int) Time.timeSinceLevelLoad;
        ui = GameObject.Find("Game Manager").GetComponent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnManager.isGameOver()) {
        timeElapsed = (int)Time.timeSinceLevelLoad - timeStart;
        timerText.text = "Timer: "+timeElapsed;    
        } 
    }
    public void StartTimer()
    {
            timerStarted = true;
            timeStart = (int) Time.timeSinceLevelLoad;
    }

    public void StopTimer()
    {
        timerStarted = false;
        if (bestTime>timeElapsed) {
            bestTime = timeElapsed;
        } else if (bestTime == 0) {
            bestTime = timeElapsed;
        }
        ui.highScore(bestTime);
        timeElapsed = 0;
    }
    public int getBestTime() {
        return bestTime;
    }
}
