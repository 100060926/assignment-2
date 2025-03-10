using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerA : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public int boostDuration = 1;
    Vector3 origin = new Vector3(2.5f,3,-22.5f);
    public Vector3 checkPoint;
    Vector3 finalChallange = new Vector3(33,14,83);
    private SpawnManagerXX spawnManager;

    
    void Start()
{
    playerRb = GetComponent<Rigidbody>();
    focalPoint = GameObject.Find("Focal Point");
    checkPoint = origin;
    spawnManager = GameObject.Find("Game Manager").GetComponent<SpawnManagerXX>();
    powerupIndicator.SetActive(false);
}


    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 
        // Add force to player in direction of the focal point (and camera)
            // Set powerup indicator position to beneath player
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }
    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
        if (other.gameObject.CompareTag("Ocean"))
        {
            ResetPlayerPosition();
        }
        if (other.gameObject.CompareTag("piston")) 
        {
            playerRb.position = finalChallange;
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
{
    if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ocean"))
    {
        
    }
}
    public void ResetPlayerPosition ()
    {
        transform.position = checkPoint;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void setSpawn(Vector3 a) {
        checkPoint = a;
    }
    public void setSpawnOrigin() {
        checkPoint = origin;
    }

}
