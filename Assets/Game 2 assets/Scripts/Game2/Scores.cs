using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
//Shahad
{
    public Text redScoreText; // UI text for Red ball score
    public Text blueScoreText; // UI text for Blue ball score
    public Text brownScoreText; // UI text for Brown ball score

    private int redScore = 0;
    private int blueScore = 0;
    private int brownScore = 0;

    void Start()
    {
        ResetScores(); // Reset all scores to 0 at game start
        UpdateScoreUI(); // Update the UI text with the reset scores
    }

    public void AddScore(string ballColor)
    {
        // Increase the correct ball's score based on its color
        if (ballColor == "Red Ball") redScore++;
        else if (ballColor == "Blue Ball") blueScore++;
        else if (ballColor == "Brown Ball") brownScore++;

        // Save the updated scores to PlayerPrefs
        PlayerPrefs.SetInt("RedScore", redScore);
        PlayerPrefs.SetInt("BlueScore", blueScore);
        PlayerPrefs.SetInt("BrownScore", brownScore);
        PlayerPrefs.Save();

        UpdateScoreUI(); // Refresh the score display
    }

    private void UpdateScoreUI()
    {
        redScoreText.text = redScore.ToString();
        blueScoreText.text = blueScore.ToString();
        brownScoreText.text = brownScore.ToString();
    }

    // Checks if the player collected at least 3 of each ball to win
    public bool HasPlayerWon()
    {
        return redScore >= 3 && blueScore >= 3 && brownScore >= 3;
    }

    // Resets all scores back to 0 (used when restarting the game)
    public void ResetScores()
    {
        PlayerPrefs.SetInt("RedScore", 0);
        PlayerPrefs.SetInt("BlueScore", 0);
        PlayerPrefs.SetInt("BrownScore", 0);
        PlayerPrefs.Save();

        redScore = 0;
        blueScore = 0;
        brownScore = 0;

        UpdateScoreUI();
    }
}
