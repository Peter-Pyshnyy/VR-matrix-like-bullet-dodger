using UnityEngine;
using TMPro;
public class CollisionScript : MonoBehaviour
{
    public HealthScript health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit");
        health.reduceHealth();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
