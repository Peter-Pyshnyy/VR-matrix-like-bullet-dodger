using UnityEngine;
using TMPro;
public class CollisionScript : MonoBehaviour
{
    //public HealthScript health;
    public GameOver gameOver;
    public bool runGame = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit");
        //health.reduceHealth();
        gameOver.SetUp();
        runGame = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
