using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplodeObs : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject player;
    private Rigidbody playerRB;
    private PlayerControllerA pc;
    public ParticleSystem explosion;
    private float respawnWaitTime = 0.25f;
    SpawnManagerXX spawnManager;
    void Start()
    {
        player = GameObject.Find("Player");
        playerRB = player.GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayerControllerA>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerXX>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isGameOver()) {
            if (transform.position.z > 0){
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !pc.hasPowerup){
            Vector3 awayFromPlayer =  other.gameObject.transform.position - transform.position;
            playerRB.AddForce(awayFromPlayer * 25, ForceMode.Impulse);
            explosion.Play();
            StartCoroutine(respawnTime());
        }
    }
    IEnumerator respawnTime()
    {
        yield return new WaitForSeconds(respawnWaitTime);
        pc.ResetPlayerPosition();
    }
}
