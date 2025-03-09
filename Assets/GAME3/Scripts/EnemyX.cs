using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private PlayerControllerX pc;
    private GameObject player;
    private float factor = 10;
    SpawnManagerXX spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControllerX>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
        speed = 200;
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
        if (spawnManager.isGameOver()) {
            if (transform.position.z > 0){
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player" && !pc.hasPowerup)
        {
            Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            playerRB.AddForce(lookDirection * (speed/factor) , ForceMode.Impulse);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ocean")) {
            Destroy(gameObject);
        }
    }

}
