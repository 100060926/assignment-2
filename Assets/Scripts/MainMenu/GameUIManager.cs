using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public GameObject gameUI;          // UI Panel for the game
    public GameObject countdownPanel;  // Countdown UI Panel
    public Text countdownText;         // Countdown text inside the panel
    public GameObject gameElements;    // Parent object for all game mechanics

    private bool countdownStarted = false; // Prevents double countdown execution

    void Start()
    {
        if (!countdownStarted) // Ensure countdown runs only once
        {
            countdownStarted = true;
            Debug.Log("Game Scene Loaded. Starting Countdown...");

            // Ensure UI is active
            gameUI.SetActive(true);
            countdownPanel.SetActive(true);
            gameElements.SetActive(false); // Disable game mechanics at start

            // Pause the game while countdown runs
            Time.timeScale = 0f;

            // Start countdown
            StartCoroutine(StartCountdown());
        }
    }

    IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f); // Use Unscaled time since game is paused
        }

        countdownText.text = "START!";
        yield return new WaitForSecondsRealtime(1f);

        // Hide countdown panel and start the game
        countdownPanel.SetActive(false);
        gameElements.SetActive(true);

        Debug.Log("Countdown Over. Game Starting!");

        // Resume game
        Time.timeScale = 1f;
    }
}
