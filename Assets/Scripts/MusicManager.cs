using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip beginning;
    public AudioSource start;
    public AudioSource loop;

    // Start is called before the first frame update
    void Start()
    {
        start.Play();
        loop.PlayScheduled(AudioSettings.dspTime + beginning.length);
    }
}
