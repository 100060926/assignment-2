using UnityEngine;
using UnityEngine.UI;

//shahad
public class InstructionsMenu : MonoBehaviour
{
    public GameObject instructionsPanel; // The panel showing game instructions
    public GameObject instructionBoard; //  The instruction board sprite
    public GameObject instructionsButton; //  The button to open instructions
    public GameObject backButton; //  The button to return to the game

    private bool isPaused = false; // Tracks if the game is paused

    void Start()
    {
        instructionsPanel.SetActive(false); // Hide panel at game start
        instructionBoard.SetActive(false); // Hide instruction board at game start
        backButton.SetActive(false); // Hide "Back to Game" button at game start
        instructionsButton.SetActive(true); // Show "Instructions" button
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true); // Show the instruction panel
        instructionBoard.SetActive(true); //  Show the instruction board sprite
        backButton.SetActive(true); //  Show "Back to Game" button
        instructionsButton.SetActive(false); //  Hide "Instructions" button
        backButton.transform.SetAsLastSibling(); //  Ensure the button is on top
        Time.timeScale = 0f; // ⏸ Pause the game
        isPaused = true;
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false); // Hide the instruction panel
        instructionBoard.SetActive(false); //  Hide the instruction board sprite
        backButton.SetActive(false); // Hide "Back to Game" button
        instructionsButton.SetActive(true); // Show "Instructions" button
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }
}
