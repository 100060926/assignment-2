using UnityEngine;

public class pendulum : MonoBehaviour
{
    private float limit = 75.0f;
    public float random;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = limit * Mathf.Sin((Time.time+random) * 2);
		transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
