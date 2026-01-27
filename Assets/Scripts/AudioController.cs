using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    // this should be to make the singleton pattern
    public static AudioController Instance;
    
    [Range(0, 1)]
    public float volume = 1f;
    public int sampleRate = 44100;

    [Header("Audio Sample")]
    // the SoundClip is the whole data from the sound
    public AudioClip bulletSoundClip;

    private const float SpeedOfSound = 343;
    private const float HeadRadius = 0.1f;
    
    // from the clip the Source is extracted
    private AudioSource bulletSound;
    private float[] soundData;
    private Transform viewPosition;



    void Awake(){
        Instance = this;
        bulletSound = GetComponent<AudioSource>();
        soundData = new float[bulletSoundClip.samples * bulletSoundClip.channels];
    }

    void Start()
    {
        bulletSound.clip = bulletSoundClip;
        bulletSound.loop = false; 
        
        // if the player view direction were to move, then this in the update function
        viewPosition = Camera.main.transform;
        if(soundData == null){
            soundData = new float[bulletSoundClip.samples * bulletSoundClip.channels];
        }

    }

    void OnValidate(){

        if (bulletSound== null)
        {
            bulletSound= GetComponent<AudioSource>();
        }
        bulletSound.hideFlags = HideFlags.HideInInspector;
    }

    // this should be used by ShootingController to play the audio
    public void PlayAudio(Vector3 shootingPosition){
        // viewPosRight will be the vector to the right direction of the position of the player
        Vector3 viewPosRight = viewPosition.right;
        viewPosRight = viewPosRight.normalized;

        Vector3 toAgent = shootingPosition - viewPosition.position;
        toAgent = toAgent.normalized;

        // if shooting agent is on the right, then positive,  dot product negative if from the left
        // front and back = 0 because orthogonal
        float shootingAngle = Vector3.Dot(viewPosRight, toAgent); 

        //Debug.Log("Current angle of the shot "+ shootingAngle + " from " + shootingPosition + " with pan : " + shootingAngle);
        bulletSound.panStereo = shootingAngle; // this controls ITD  
        bulletSound.PlayOneShot(bulletSoundClip);
    }

    void ApplyAudio(int channels){
        for(int i = 0; i < soundData.Length; i++){
            soundData[i] *= volume;
        }
    }
}
