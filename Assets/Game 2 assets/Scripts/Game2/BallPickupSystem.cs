using UnityEngine;

public class BallPickupSystem : MonoBehaviour
//Shahad
{
    public Transform attachPoint; // The point where the ball will attach to the player
    public float pickupRadius = 2f; // How close the ball needs to be to pick it up
    public GameObject dropEffectPrefab; // Visual effect when scoring

    private GameObject currentBall = null; // Keeps track of the ball we're holding
    private bool isHoldingBall = false; // Are we holding a ball?
    private Rigidbody playerRb; // Reference to the player's Rigidbody

    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Get the player's Rigidbody
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press "E" to pick up or drop
        {
            if (!isHoldingBall)
            {
                TryPickUpBall(); // Try to pick up a nearby ball
            }
            else
            {
                DropBall(); // Drop the ball if we have one
            }
        }
    }

    private void TryPickUpBall()
    {
        // Check for nearby objects within the pickup radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in colliders)
        {
            if (IsBall(collider.tag)) // Check if it's a ball of any color
            {
                PickUpBall(collider.gameObject);
                return;
            }
        }
    }

    private void PickUpBall(GameObject ball)
    {
        isHoldingBall = true;
        currentBall = ball;

        // Stop any unwanted physics movement on the player
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // Disable physics so the ball doesn't move independently
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Stop physics interactions
            rb.useGravity = false; // Disable gravity so it stays in place
            rb.linearVelocity = Vector3.zero; // Stop any movement
            rb.angularVelocity = Vector3.zero; // Stop any spinning
        }

        // Attach ball to the player's attachPoint correctly
        currentBall.transform.SetParent(attachPoint);
        currentBall.transform.localPosition = Vector3.zero;
        currentBall.transform.localRotation = Quaternion.identity; // Prevents rotation issues

        // Slightly move the ball forward if it's attaching behind the player
        currentBall.transform.localPosition = new Vector3(0, 0, 0.5f);
    }

    private void DropBall()
    {
        if (currentBall == null) return;

        isHoldingBall = false;

        // Enable physics again so it falls naturally
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Detach from the player
        currentBall.transform.SetParent(null);

        // Add the BallCollision script to handle basket scoring
        if (currentBall.GetComponent<BallCollision>() == null)
        {
            currentBall.AddComponent<BallCollision>(); // Assign BallCollision to handle scoring
        }

        currentBall = null; 
    }

    // Checks if an object is a ball (Red, Brown, or Blue)
    private bool IsBall(string tag)
    {
        return tag == "Red Ball" || tag == "Brown Ball" || tag == "Blue Ball";
    }
}
