using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainMenuPanel;  // Reference to Main Menu Panel
    public GameObject gameplayPanel;  // Reference to Gameplay Panel
    public Button playButton;         // Reference to Play Button
    public Button scene1Button;       // Reference to Scene 1 Button
    public Button scene2Button;       // Reference to Scene 2 Button
    public Button scene3Button;       // Reference to Scene 3 Button
    public Button scene4Button;       // Reference to Scene 4 Button

    void Start()
    {
        // Ensure Main Menu is active and Gameplay panel is inactive at start
        mainMenuPanel.SetActive(true);
        gameplayPanel.SetActive(false);

        // Add listeners to buttons
        playButton.onClick.AddListener(StartGame);
        scene1Button.onClick.AddListener(() => LoadScene("Game3_2")); 
        scene2Button.onClick.AddListener(() => LoadScene("Game1"));
        scene3Button.onClick.AddListener(() => LoadScene("PlayGround"));
        //scene4Button.onClick.AddListener(() => LoadScene("Scene4"));
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false); // Hide Main Menu
        gameplayPanel.SetActive(true);  // Show Gameplay Panel
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
