using UnityEngine;

public class spikes : MonoBehaviour
{
    
    private GameObject player;
    private PlayerControllerA pc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControllerA>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
