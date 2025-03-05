using UnityEngine;

public class CameraRotationGame1 : MonoBehaviour
{
     private float speed = 200;
    public GameObject player;

    // Update is called once per frame
    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * speed * Time.deltaTime);

        transform.position = player.transform.position; // Move focal point with player

    }
}
