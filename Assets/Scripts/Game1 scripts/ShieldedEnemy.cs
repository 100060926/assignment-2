using UnityEngine;

public class ShieldedEnemy : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    public bool shieldActive = true; // Starts with a shield
    private Transform enemyGoal;
    private bool movingToGoal = false;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemyGoal = GameObject.Find("EnemyGoal").transform;
    }

    void Update()
    {
        Vector3 direction = movingToGoal ? (enemyGoal.position - transform.position).normalized : (player.transform.position - transform.position).normalized;
        enemyRb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (shieldActive)
            {
                shieldActive = false;
                Debug.Log("Shield Broken!");
            }
            else
            {
                movingToGoal = true; // Only moves to goal after second hit
                Debug.Log("Shielded enemy now moving to goal!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyGoal"))
        {
            Destroy(gameObject);
        }
    }
}
