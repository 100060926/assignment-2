using UnityEngine;

public class BallCollision : MonoBehaviour

//Shahad

{
    public GameObject disappearEffect;
    private Scores scoreManager;

    [System.Obsolete]
    void Start()
    {
        // Find the Scores script in the scene
        scoreManager = FindObjectOfType<Scores>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsMatchingBasket(collision.collider.tag))
        {
            // Play effect before removing the ball
            if (disappearEffect != null)
            {
                Instantiate(disappearEffect, transform.position, Quaternion.identity);
            }

            // Add score to the matching color
            if (scoreManager != null)
            {
                scoreManager.AddScore(gameObject.tag);
            }

            Destroy(gameObject); // Remove the ball
        }
    }

    private bool IsMatchingBasket(string basketTag)
    {
        return (gameObject.CompareTag("Red Ball") && basketTag == "Red Basket") ||
               (gameObject.CompareTag("Blue Ball") && basketTag == "Blue Basket") ||
               (gameObject.CompareTag("Brown Ball") && basketTag == "Brown Basket");
    }
}

