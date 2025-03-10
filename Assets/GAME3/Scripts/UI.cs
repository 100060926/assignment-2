using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour
{
    SpawnManagerXX spawnManager;
    timer timer;  
    PlayerControllerX pc;
    public TMP_Text bestTime;
    public GameObject mainMenuButton;
    public GameObject restartButton;
    public TMP_Text gameComplete;
    public TMP_Text gameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = GetComponent<timer>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
        pc = GameObject.Find("Player").GetComponent<PlayerControllerX>();
        bestTime.gameObject.SetActive(false);
        mainMenuButton.SetActive(false);
        restartButton.SetActive(false);
        gameComplete.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isGameOver()) {
            bestTime.gameObject.SetActive(true);
            mainMenuButton.SetActive(true);
            restartButton.SetActive(true);
            gameComplete.gameObject.SetActive(true);
        } 
    }
    public void restart() {
        spawnManager.restartTheGame();
        pc.setSpawnOrigin();
        pc.ResetPlayerPosition();
        timer.StartTimer();
        bestTime.gameObject.SetActive(false);
        mainMenuButton.SetActive(false);
        restartButton.SetActive(false);
        gameComplete.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
    }
    public void returnToMainMenu() {
         UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void highScore(int score) {
        bestTime.text = "Best Time: "+ score.ToString();
    }
}
