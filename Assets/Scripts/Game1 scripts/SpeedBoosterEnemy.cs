using UnityEngine;
using System.Collections;

public class SpeedBoosterEnemy : MonoBehaviour
{
    public float normalSpeed = 3.0f;
    public float dashSpeed = 8.0f;
    public float dashCooldown = 3.0f;
    public float warningTime = 1.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    private bool isDashing = false;
    private Renderer enemyRenderer;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemyRenderer = GetComponent<Renderer>();
        StartCoroutine(DashCycle());
    }

    void Update()
    {
        if (!isDashing)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * normalSpeed;
        }
    }

    IEnumerator DashCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashCooldown);

            // Warning Indicator (Flashes color before dashing)
            enemyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(warningTime);
            enemyRenderer.material.color = Color.white;

            isDashing = true;
            Vector3 dashDirection = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = dashDirection * dashSpeed;
            yield return new WaitForSeconds(1.0f);
            isDashing = false;
        }
    }
}
