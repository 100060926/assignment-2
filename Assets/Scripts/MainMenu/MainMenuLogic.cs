using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainMenuPanel;      // Main Menu UI
    public GameObject gameSelectionPanel; // Game selection UI

    // Individual Confirmation Pop-ups for each game
    public GameObject confirmationPopup1;
    public GameObject confirmationPopup2;
    public GameObject confirmationPopup3;

    public Button playButton;     // Play Button
    public Button scene1Button;   // Game 1 Button
    public Button scene2Button;   // Game 2 Button
    public Button scene3Button;   // Game 3 Button

    // Confirm & Cancel buttons for each game
    public Button confirmButton1, cancelButton1;
    public Button confirmButton2, cancelButton2;
    public Button confirmButton3, cancelButton3;

    private string selectedScene; // Store selected game scene

    void Start()
    {
        // Ensure correct UI state
        mainMenuPanel.SetActive(true);
        gameSelectionPanel.SetActive(false);
        confirmationPopup1.SetActive(false);
        confirmationPopup2.SetActive(false);
        confirmationPopup3.SetActive(false);

        // Play Button opens Game Selection
        playButton.onClick.AddListener(ShowGameSelection);

        // Game Selection buttons open respective pop-ups
        scene1Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup1, "Game3_2"));
        scene2Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup2, "Game1")); // Make sure "Game1" is the correct scene name
        scene3Button.onClick.AddListener(() => ShowConfirmation(confirmationPopup3, "PlayGround"));

        // Confirmation buttons for each game
        confirmButton1.onClick.AddListener(() => LoadGameScene(confirmationPopup1, "Game3_2"));
        confirmButton2.onClick.AddListener(() => LoadGameScene(confirmationPopup2, "Game1")); // Make sure "Game1" is correct
        confirmButton3.onClick.AddListener(() => LoadGameScene(confirmationPopup3, "PlayGround"));

        // Cancel buttons to close the respective pop-ups
        cancelButton1.onClick.AddListener(() => ClosePopup(confirmationPopup1));
        cancelButton2.onClick.AddListener(() => ClosePopup(confirmationPopup2));
        cancelButton3.onClick.AddListener(() => ClosePopup(confirmationPopup3));
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
