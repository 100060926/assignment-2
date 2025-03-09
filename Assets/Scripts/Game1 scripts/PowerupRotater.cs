using UnityEngine;

public class PowerupRotater : MonoBehaviour
{
    public float rotationSpeed = 50f;  // Speed of rotation
    public float heightBoost = 1f;     // How much the powerup is lifted

    void Start()
    {
        // Lift the powerup up by the heightBoost amount
        transform.position += Vector3.up * heightBoost;
    }

    void Update()
    {
        // Rotate the powerup around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
