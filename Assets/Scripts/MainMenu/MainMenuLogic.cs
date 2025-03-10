using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainMenuPanel;      // Main Menu UI
    public GameObject gameSelectionPanel; // Game selection UI

    // Individual Confirmation Pop-ups for each game
    public GameObject confirmationPopup1;
    public GameObject confirmationPopup2;
    public GameObject confirmationPopup3;
    public GameObject confirmationPopup4; // New confirmation popup for new game

    public Button playButton;     // Play Button
    public Button scene1Button;   // Game 1 Button
    public Button scene2Button;   // Game 2 Button
    public Button scene3Button;   // Game 3 Button
    public Button scene4Button;   //for Game 4

    // Confirm & Cancel buttons for each game
    public Button confirmButton1, cancelButton1;
    public Button confirmButton2, cancelButton2;
    public Button confirmButton3, cancelButton3;
    public Button confirmButton4, cancelButton4; // New confirm & cancel buttons

    private string selectedScene; // Store selected game scene

    void Start()
    {
        // Ensure correct UI state
        mainMenuPanel.SetActive(true);
        gameSelectionPanel.SetActive(false);
        confirmationPopup1.SetActive(false);
        confirmationPopup2.SetActive(false);
        confirmationPopup3.SetActive(false);
        confirmationPopup4.SetActive(false); // Initialize new pop-up to be hidden

        // Play Button opens Game Selection
        playButton.onClick.AddListener(ShowGameSelection);

        // Game Selection buttons open respective pop-ups
        scene1Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup1, "Game3_2"));
        scene2Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup2, "Game1"));
        scene3Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup3, "PlayGround"));
        scene4Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup4, "PlayGround")); // New scene

        // Confirmation buttons for each game
        confirmButton1.onClick.AddListener(() => LoadGameScene(confirmationPopup1, "Game3_2"));
        confirmButton2.onClick.AddListener(() => LoadGameScene(confirmationPopup2, "Game1"));
        confirmButton3.onClick.AddListener(() => LoadGameScene(confirmationPopup3, "PlayGround"));
        confirmButton4.onClick.AddListener(() => LoadGameScene(confirmationPopup4, "PlayGround")); // Load new scene

        // Cancel buttons to close the respective pop-ups
        cancelButton1.onClick.AddListener(() => ClosePopup(confirmationPopup1));
        cancelButton2.onClick.AddListener(() => ClosePopup(confirmationPopup2));
        cancelButton3.onClick.AddListener(() => ClosePopup(confirmationPopup3));
        cancelButton4.onClick.AddListener(() => ClosePopup(confirmationPopup4)); // Close new pop-up
    }

    void ShowGameSelection()
    {
        mainMenuPanel.SetActive(false);
        gameSelectionPanel.SetActive(true);
    }

    void ShowConfirmation(GameObject popup, string sceneName)
    {
        selectedScene = sceneName;
        popup.SetActive(true);
    }

    void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
    }

    void LoadGameScene(GameObject popup, string sceneName)
    {
        popup.SetActive(false);  // Close the pop-up
        StartCoroutine(LoadScene(sceneName)); // Start scene loading
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.5f); // Small delay for smooth transition

        Debug.Log("Loading Scene: " + sceneName); // Debug log to confirm scene is being loaded

        SceneManager.LoadScene(sceneName);
    }
}
