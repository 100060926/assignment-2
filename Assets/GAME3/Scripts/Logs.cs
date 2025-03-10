using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Logs : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerControllerA pc;
    private Vector3 direction = new Vector3(-1,0,0);
    private float force = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * force, ForceMode.Impulse);
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        pc = GameObject.Find("Player").GetComponent<PlayerControllerA>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(direction * force, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ocean"))
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            pc.ResetPlayerPosition();
        }
    }
}
