using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{

    [Range(0, 1)]
    public float volume = 1f;

    [Tooltip("-90 left and 90 right")]
    public float angle = 0; // TODO : should the player be able to rotate camera?
    public int sampleRate = 44100;

    public AudioClip bulletSound;
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
