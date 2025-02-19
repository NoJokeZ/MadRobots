using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] MusicSounds;

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

    /// <summary>
    /// Plays a music track at an audi source
    /// </summary>
    /// <param name="name"></param>
    /// <param name="audioSource"></param>
    public void PlayMusic(string name, AudioSource audioSource)
    {
        Sound s = Array.Find(MusicSounds, x  => x.Name == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            audioSource.clip = s.AudioClip; //Puts the music in the source
            audioSource.Play(); //Plays it
        }
    }

    /// <summary>
    /// Stops a music track at an audio source
    /// </summary>
    /// <param name="name"></param>
    /// <param name="audioSource"></param>
    public void StopMusic(string name, AudioSource audioSource)
    {
        Sound s = Array.Find(MusicSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            audioSource.clip = s.AudioClip;
            audioSource.Stop();
        }
    }
}
