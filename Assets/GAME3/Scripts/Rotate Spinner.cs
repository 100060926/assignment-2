using Unity.VisualScripting;
using UnityEngine;

public class RotateSpinner : MonoBehaviour
{
    private float rotationSpeed  = 100.0f;
    private PlayerControllerX pc;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        pc = player.gameObject.GetComponent<PlayerControllerX>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")){
            if (!pc.hasPowerup) {
                pc.ResetPlayerPosition();
            }
        }
    }
}
