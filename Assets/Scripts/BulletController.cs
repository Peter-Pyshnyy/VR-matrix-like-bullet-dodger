using UnityEngine;

public class BulletController: MonoBehaviour
{

    public float speed = 10f;
    public float lifeTime = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
