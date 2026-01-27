using UnityEngine;

public class ShootingController : MonoBehaviour
{

    public GameObject BulletPrefab;
    public float shootingSpeed;
    public Transform[] spawnPoint;

    float timer = 0.0f;
    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if(timer > shootingSpeed) 
        {
            shootRandom();
            timer = 0.0f;
        }
    }

    void shootRandom(){
        int randomBullet = Random.Range(0, spawnPoint.Length);
        Transform bulletPosition = spawnPoint[randomBullet];
        Instantiate(BulletPrefab, bulletPosition.position, bulletPosition.rotation);

        // Unity already incorporates spatial (and also stereo audio)
        if(AudioController.Instance != null){
            AudioController.Instance.PlayAudio(bulletPosition.position);
        }
    }
}
