using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Player's Transform
    public Vector3 offset; // Default distance between the camera and the player
    public float smoothSpeed = 0.5f; // Smooth transition speed

    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Set camera position behind the player
            Vector3 targetPosition = player.position + offset;

            // Smoothly interpolate the camera position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
