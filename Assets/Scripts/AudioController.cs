using System.ComponentModel.Design;
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
    [Tooltip("Maximum ITD delay in milliseconds")]
    [SerializeField] private float maxHaasDelay = 20f;

    private const float SpeedOfSound = 343;
    private const float HeadRadius = 0.1f;
    
    // from the clip the Source is extracted
    private AudioSource bulletSound;
    private float[] soundData;
    private Transform viewPosition;

    private float[] leftDelayBuffer;
    private float[] rightDelayBuffer;
    private int bufferSize;
    private int leftWriteIndex = 0;
    private int rightWriteIndex = 0;
    private int leftReadIndex = 0;
    private int rightReadIndex = 0;
    private int leftDelaySamples;
    private int rightDelaySamples;

    // the angle from which the bullet was shot in relation to main camera
    private float shootingAngle;

    void Awake(){
        Instance = this;
        bulletSound = GetComponent<AudioSource>();
        soundData = new float[bulletSoundClip.samples * bulletSoundClip.channels];

        bufferSize = Mathf.CeilToInt(maxHaasDelay/1000f * sampleRate);
        leftDelayBuffer = new float[bufferSize];
        rightDelayBuffer = new float[bufferSize];
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
        Vector3 viewDirection = viewPosition.forward;
        viewDirection =viewDirection.normalized;

        Vector3 toAgent = shootingPosition - viewPosition.position;
        toAgent = toAgent.normalized;

        // if shooting agent is on the right, then positive,  dot product negative if from the left
        // front and back = 0 because orthogonal
        shootingAngle = Mathf.Acos(Vector3.Dot(viewDirection, toAgent)); 
        shootingAngle = toAgent.x < 0 ? -shootingAngle: shootingAngle; // this should change the sign of the angle (left negative and right pos)

        Debug.Log("Current angle of the shot "+ shootingAngle + " from " + shootingPosition);

        bulletSound.Play();
    }

    // this method is called by Unity and is responsible for stereo audio
    void OnAudioFilterRead(float[] data, int channels)
    {
        if(channels < 2) return;

        for(int i = 0; i < soundData.Length; i++){
            soundData[i] *= volume;
        }


        ILD(data, channels);
    }

    void ILD(float[] data, int channels)
    {
        // float theta = shootingAngle * (float)Mathf.PI / 180f; // transform to degree
        // float intensity = Mathf.Sin(theta) * 0.5f + 0.5f; // so the right/left values with (sin(90)) won't be inaudible

        // float intensityLeft = 1f - intensity;
        // float intensityRigth = intensity;

        // channgels always 2 (stereo); data interleaved LRLRLR...
        for (int i = 0; i < data.Length; i += channels)
        {
            // data[i] *= intensityLeft;
            // data[i+1] *= intensityRigth;
        }
    }
}
