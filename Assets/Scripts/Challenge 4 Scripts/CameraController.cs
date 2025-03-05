using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Player's Transform
    public Vector3 offset = new Vector3(0, 3, -6); // Camera offset (Behind the player)
    public float smoothSpeed = 5f; // Smooth transition speed

    void LateUpdate()
    {
        if (player != null)
        {
            // Set camera position behind the player using world space
            Vector3 targetPosition = player.position + player.transform.rotation * offset;

            // Smooth movement
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Always look at the player but prevent flipping
            Quaternion targetRotation = Quaternion.Euler(10, player.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }
}
