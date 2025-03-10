using UnityEngine;

public class checkpointSet : MonoBehaviour
{
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
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pc.setSpawn(transform.position);
        }
    }
}
