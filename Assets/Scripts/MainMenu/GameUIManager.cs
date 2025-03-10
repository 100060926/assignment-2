using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject gameUI;        // UI Panel for the game
    public GameObject gameElements;  // Parent object for all game mechanics

    public Camera introCamera;       // Camera shown before the game starts
    public Camera gameCamera;        // Main game camera

    public float introDuration = 2f; // Duration to show intro camera before switching

    private bool gameStarted = false; // Prevents multiple starts

    void Start()
    {
        // Ensure everything is disabled at the start
        gameUI.SetActive(false);
        gameElements.SetActive(false);
        if (gameCamera != null) gameCamera.gameObject.SetActive(false);
        if (introCamera != null) introCamera.gameObject.SetActive(true);

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

        Debug.Log("Main Menu Closed. Playing Intro Camera...");

        // Keep the intro camera active for a few seconds
        yield return new WaitForSeconds(introDuration);

        // Switch to game camera
        if (introCamera != null) introCamera.gameObject.SetActive(false);
        if (gameCamera != null) gameCamera.gameObject.SetActive(true);

        // Activate game elements and UI
        gameUI.SetActive(true);
        gameElements.SetActive(true);

        Debug.Log("Game Started!");
    }
}
