using UnityEngine;

public class ShootingController : MonoBehaviour
{

    public GameObject BulletPrefab;
    public float shootingSpeed;
    public Transform[] spawnPoint;
    public Transform cameraPos;

    float timer = 0.0f;

    void Start()
    {
        foreach (Transform t in spawnPoint)
        {
            t.rotation =  Quaternion.LookRotation(cameraPos.position - t.position);
        }
    }

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
       if(AudioController.Instance != null)
        {
            AudioController.Instance.PlayAudio(bulletPosition.position);
        } 
    }
}
