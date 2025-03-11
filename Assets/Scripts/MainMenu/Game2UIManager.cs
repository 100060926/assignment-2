using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game2UIManager : MonoBehaviour
{
    public GameObject gameUI;        // UI Panel for the game
    public GameObject gameElements;  // Parent object for all game mechanics
    public Camera gameCamera;        // Main game camera

    private bool gameStarted = false; // Prevents multiple starts

    void Start()
    {
        // Ensure everything is disabled at the start
        gameUI.SetActive(false);
        gameElements.SetActive(false);

        if (gameCamera != null) gameCamera.gameObject.SetActive(true); // Activate game camera immediately

        // Start coroutine to wait for the menu to close before starting the game
        StartCoroutine(WaitForMenuToClose());
    }

    IEnumerator WaitForMenuToClose()
    {
        // Wait until the Main Menu is no longer loaded
        while (SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            yield return null; // Wait until the next frame to check again
        }

        Debug.Log("Main Menu Closed. Starting Game 2...");

        // Activate game elements and UI immediately
        gameUI.SetActive(true);
        gameElements.SetActive(true);

    }
}
