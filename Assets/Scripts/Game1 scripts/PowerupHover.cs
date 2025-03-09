using UnityEngine;

public class PowerupHover : MonoBehaviour
{
    public float hoverSpeed = 2f; // Speed of the hover motion
    public float hoverHeight = 0.5f; // How high the object hovers

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
