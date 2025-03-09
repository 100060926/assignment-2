using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game1UIManager : MonoBehaviour
{
    public GameObject gameElements;   // Game mechanics container (UI & gameplay)
    public Camera gameCamera;         // Game Camera
    public GameObject countdownPanel; // Countdown UI inside gameElements
    public Text countdownText;        // Countdown text inside the panel
    private bool gameStarted = false; // Ensure game only starts once

    void Start()
    {
        // Ensure game mechanics and UI are disabled at start
        gameElements.SetActive(false);

        // Ensure game camera is active
        gameCamera.gameObject.SetActive(true);

        // Start countdown coroutine when the game begins
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Ensure countdown panel is visible at the start
        countdownPanel.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);

        // Hide countdown panel and start the game
        countdownPanel.SetActive(false);
        gameElements.SetActive(true);  // Enable all game mechanics and UI
        gameCamera.gameObject.SetActive(false); // Disable camera

        Debug.Log("Game Started!");
    }
}
