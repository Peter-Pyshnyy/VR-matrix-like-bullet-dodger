using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    public GameObject BulletPrefab;
    public float shootingSpeed;
    public Transform[] spawnPoint;
    public Transform cameraPos;

    [Header("Shooting Audio")]
    public AudioClip shootingClip;
    [Header("Reloading Audio")]
    public AudioClip reloadClip;
    [Range(1, 5)]
    public float reloadTime = 1;

    float timer = 0.0f;
    float reloadTimer = 0.0f;

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
            StartCoroutine(shootRandom());
            timer = 0.0f;
        }
    }

    IEnumerator shootRandom(){
        int randomBullet = Random.Range(0, spawnPoint.Length);
        Transform bulletPosition = spawnPoint[randomBullet];
        // play reload spatial audio
        if(AudioController.Instance != null)
        {
            AudioController.Instance.PlayAudio(bulletPosition.position, reloadClip);
            Debug.Log("starting to wait");
            yield return new WaitForSeconds(reloadTime);
        }

        Instantiate(BulletPrefab, bulletPosition.position, bulletPosition.rotation);
        AudioController.Instance.PlayAudio(bulletPosition.position, shootingClip);
    }
}
