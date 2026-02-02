using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float shootingSpeed;
    public Transform[] agentPos;
    public Transform cameraPos;

    [Header("Shooting Audio")]
    public AudioClip shootingClip;
    [Header("Reloading Audio")]
    public AudioClip reloadClip;
    [Range(1, 5)]
    public float reloadTime = 1;

    float timer = 0.0f;
    float reloadTimer = 0.0f;
    public bool runGame = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform t in agentPos)
        {
            //t.rotation = Quaternion.LookRotation((cameraPos.position - new Vector3(0.0f, 0.0f, 0.15f)) - t.position);
            t.rotation = Quaternion.LookRotation(cameraPos.position - t.position);
        }

        timer += Time.deltaTime;
        if(timer > shootingSpeed && runGame) 
        {
            StartCoroutine(shootRandom());
            timer = 0.0f;
        }
    }

IEnumerator shootRandom(){
        int randomBullet = Random.Range(0, agentPos.Length);
        Transform bulletPosition = agentPos[randomBullet];
        Transform audioPosition = bulletPosition.GetChild(0).transform;
        // play reload spatial audio
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlayAudio(audioPosition.position, reloadClip);
            Debug.Log("starting to wait");
            yield return new WaitForSeconds(reloadTime);
        }

        Instantiate(BulletPrefab, bulletPosition.position, bulletPosition.rotation);
        AudioController.Instance.PlayAudio(bulletPosition.position, shootingClip);
    }
}
