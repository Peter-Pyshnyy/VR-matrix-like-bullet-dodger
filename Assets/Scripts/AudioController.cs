using UnityEngine;


public enum AudioType{None, ILD, ITD}
[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    // this should be to make the singleton pattern
    public static AudioController Instance;
    

    [SerializeField] private AudioType currentEffect = AudioType.None;
    [Range(0, 1)]
    public float volume = 1f;
    public int sampleRate = 44100;

    [Header("Audio Sample")]
    // the SoundClip is the whole data from the sound
    public AudioClip bulletSoundClip;
    [Tooltip("Maximum ITD delay in milliseconds")]
    [SerializeField] private float maxHaasDelay = 20f;

    
    // from the clip the Source is extracted
    private AudioSource bulletSound;
    private float[] soundData;
    private Transform viewPosition;

    private float HeadRadius = 0.1f;
    private float SpeedOfSound = 343f;
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

        //Debug.Log("Current angle of the shot "+ shootingAngle + " from " + shootingPosition);

        bulletSound.Play();
    }

    // this method is called by Unity and is responsible for stereo audio
    void OnAudioFilterRead(float[] data, int channels)
    {
        if(channels < 2) return;

        for(int i = 0; i < soundData.Length; i++){
            soundData[i] *= volume;
        }

        switch (currentEffect)
        {
            case AudioType.ILD:
                ILD(data, channels);
                break;
            case AudioType.ITD:
                ITD(data, channels);
                break;
        }
    }
    void ILD(float[] data, int channels)
    {
        // input of Sin has to be in radians
        // this will do a remap to 0 (left) and 1 right
        float intensity = Mathf.Sin(shootingAngle) * 0.5f + 0.5f; // so the right/left values with (sin(90)) won't be inaudible
 
        float intensityLeft = 1f - intensity;
        float intensityRigth = intensity;

        // channgels always 2 (stereo); data interleaved LRLRLR...
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] *= intensityLeft;
            data[i+1] *= intensityRigth;
        }
    }

    float delay(float theta){
        if(theta >= 0 && theta <= Mathf.PI /2){
            return HeadRadius/ SpeedOfSound * (theta+Mathf.Sin(theta));
        }else{
            return  HeadRadius/ SpeedOfSound * (Mathf.PI - theta+Mathf.Sin(theta));
        }
    }
    void ITD(float[] data, int channels)
    {
        // 1. Physics Math (Woodworth Model)
        // We calculate the required delay in seconds based on the head radius and angle

        float absTheta = Mathf.Abs(shootingAngle);

        // Formula: Time = (r/c) * (theta + sin(theta))
        float delaySeconds = delay(absTheta);

        // Convert time to samples
        int delaySamples = (int)(delaySeconds * sampleRate);

        // Safety clamp to prevent reading outside the buffer
        delaySamples = Mathf.Clamp(delaySamples, 0, bufferSize - 1);

        // 2. Determine which ear is delayed
        // If angle is positive (Right), the Left ear is delayed.
        // If angle is negative (Left), the Right ear is delayed.
        int leftDelay = (shootingAngle> 0) ? delaySamples : 0;
        int rightDelay = (shootingAngle< 0) ? delaySamples : 0;

        // 3. Process Audio (Circular Buffer)
        for (int i = 0; i < data.Length; i += channels)
        {


            // --- WRITE STEP ---
            // Store the current raw audio into the history buffers
            leftDelayBuffer[leftWriteIndex] = data[i];
            rightDelayBuffer[rightWriteIndex] = data[i + 1];

            // --- READ STEP ---
            // Calculate where to read from: "Current Write Position" minus "Delay"
            // We add bufferSize before modulo (%) to handle wrapping around negative numbers
            int lReadIndex = (leftWriteIndex - leftDelay + bufferSize) % bufferSize;
            int rReadIndex = (rightWriteIndex - rightDelay + bufferSize) % bufferSize;

            // Apply the delayed samples to the output
            data[i] = leftDelayBuffer[lReadIndex];
            data[i + 1] = rightDelayBuffer[rReadIndex];

            // --- ADVANCE ---
            // Move the write pointers forward
            leftWriteIndex = (leftWriteIndex + 1) % bufferSize;
            rightWriteIndex = (rightWriteIndex + 1) % bufferSize;
        }
    }
}
