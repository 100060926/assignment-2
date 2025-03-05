using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int enemyScore = 0;

    public void AddScore(int points, string scorer)
    {
        if (scorer == "Player")
        {
            playerScore += points;
            Debug.Log("Player Scored! Total: " + playerScore);
        }
        else if (scorer == "Enemy")
        {
            enemyScore += points;
            Debug.Log("Enemy Scored! Total: " + enemyScore);
        }
    }
}
