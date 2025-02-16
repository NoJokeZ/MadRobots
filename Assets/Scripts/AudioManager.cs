using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] MusicSounds;
    public Sound[] SFXSounds;
    public AudioSource MusicSource;
    public AudioSource SFXSource;


    private void Awake()
    {
        //Instance creating
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x  => x.Name == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            MusicSource.clip = s.AudioClip;
            MusicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            SFXSource.PlayOneShot(s.AudioClip);
            
        }
    }
}
