using UnityEngine;
using UnityEngine.UIElements;

public class Piston : MonoBehaviour
{
    float startDelay = 2.0f;
    float repeatTime = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            InvokeRepeating("pistonUp", startDelay, repeatTime);
        }
    }
    void pistonUp() {
        if (transform.position.y < 0)
        transform.Translate(0,0.5f,0);
    }
}
