using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private AudioSource source;
    public AudioClip bgm;
    public AudioClip bgm_Ghost;
    public AudioClip ghostTransision;
    public AudioClip ouch1;
    public AudioClip ouch2;
    public AudioClip ouch3;
    public AudioClip doorSwitch;
    public AudioClip doorOpen;
    public AudioClip hit;
    private float time;

    private void Awake()
    {
        
        instance = this;
        source = GetComponent<AudioSource>();
        source.clip = bgm;
        source.Play();
    }

    public void PlayTalkSound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    public void enemyHit()
    {
        source.PlayOneShot(hit);
    }

    public void PlayDoorOpen()
    {
        source.PlayOneShot(doorOpen);
    }

    public void PlayLeverSwitch()
    {
        source.PlayOneShot(doorSwitch);
    }

    public void changeMusic()
    {
        if (source.clip == bgm)
        {
            source.PlayOneShot(ghostTransision);
            time = source.time;
            source.clip = bgm_Ghost;
            source.time = time;
            source.Play();
        }
        else if (source.clip == bgm_Ghost)
        {
            time = source.time;
            source.clip = bgm;
            source.time = time;
            source.Play();
        }

    }

    public void takeDamageSound()
    {
        int randomizer = (int)Random.Range(1, 3);
        switch(randomizer)
        {
            case 1:
                source.PlayOneShot(ouch1);
                break;
            case 2:
                source.PlayOneShot(ouch2);
                break;
            case 3:
                source.PlayOneShot(ouch3);
                break;
        }
        
    }
}
