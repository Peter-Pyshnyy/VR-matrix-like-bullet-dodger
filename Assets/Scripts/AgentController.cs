using UnityEngine;

public class AgentController: MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public Transform spawnPoint;    
    
    public float timeBetweenShots = 3f; 
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenShots)
        {
            Shoot();
            timer = 0f; 
        }
    }

    void Shoot()
    {
        // Create a copy of the bulletPrefab at the spawnPoint's position and rotation
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
