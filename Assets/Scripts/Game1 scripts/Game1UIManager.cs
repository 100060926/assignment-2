using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game1UIManager : MonoBehaviour
{
    public GameObject gameElements;   // Game mechanics container (includes countdown)
    public Camera gameCamera;         // Game Camera
    public GameObject gameUIPanel;    // UI elements (only activate when the game starts)

    public Text countdownText;        // Countdown text inside gameElements

    private bool gameStarted = false;

    void Start()
    {
        // Ensure game mechanics and UI are disabled at the start
        gameElements.SetActive(false);
        gameUIPanel.SetActive(false);

        // Ensure game camera is active
        gameCamera.gameObject.SetActive(true);

        // Start the coroutine only when the menu is closed
        StartCoroutine(WaitForMenuToClose());
    }

    IEnumerator WaitForMenuToClose()
    {
        // Wait until the Main Menu is closed
        yield return new WaitUntil(() => !GameObject.Find("MenuCanvas"));

        // Now start the countdown
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Enable game mechanics (but UI stays hidden)
        gameElements.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);

        // Activate UI and disable menu camera
        gameUIPanel.SetActive(true);
        gameCamera.gameObject.SetActive(false);

        gameStarted = true;

        // Notify the Game Manager that the game has started
        FindObjectOfType<GameManager>().OnGameStart();
    }
}
