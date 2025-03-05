using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//script done by Hour Alzaabi
public class GameController : MonoBehaviour
{
    public GameObject startPanel;      // First screen with "Play" button
    public GameObject gameSelectPanel; // Panel for choosing a game
    public GameObject startGamePanel;  // Panel with "Start" button

    private int selectedGame = -1;     // To track which game is selected

    void Start()
    {
        ShowStartPanel(); // Show the start panel initially
    }

    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gameSelectPanel.SetActive(false);
        startGamePanel.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        startPanel.SetActive(false);
        gameSelectPanel.SetActive(true);
    }

    public void OnGameSelected(int gameIndex)
    {
        selectedGame = gameIndex;
        startGamePanel.SetActive(true);
    }

    public void OnStartGame()
    {
        if (selectedGame != -1)
        {
            // Load the appropriate scene (Change Scene Names as needed)
            if (selectedGame == 0)
                SceneManager.LoadScene("Game1Scene"); // Prevent balls from entering the goal
            else if (selectedGame == 1)
                SceneManager.LoadScene("Game2Scene"); // Color coordinated balls
            else if (selectedGame == 2)
                SceneManager.LoadScene("Game3Scene"); // Guide the ball through obstacles
        }
    }
}
