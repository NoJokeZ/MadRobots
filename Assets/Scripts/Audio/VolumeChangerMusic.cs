using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeChangerMusic : MonoBehaviour
{
    private SettingsMenu settingsMenu;

    private AudioSource musicSoruce;

    private void Awake()
    {
        musicSoruce = GetComponent<AudioSource>();
    }

    private void Start()
    {
        settingsMenu = SettingsMenu.Instance;

        settingsMenu.OnMuteMusic.AddListener(ChangeMute);
        settingsMenu.OnMusicVolumeChange.AddListener(ChangeVolume);

        ChangeMute();
        ChangeVolume();
    }

    private void OnDisable()
    {
        settingsMenu.OnMuteMusic.RemoveListener(ChangeMute);
        settingsMenu.OnMusicVolumeChange.RemoveListener(ChangeVolume);
    }

    /// <summary>
    /// Change mute state
    /// </summary>
    private void ChangeMute()
    {
        musicSoruce.mute = settingsMenu.IsMusicMuted;
    }

    /// <summary>
    /// Change volume value
    /// </summary>
    private void ChangeVolume()
    {
        musicSoruce.volume = settingsMenu.MusicVolume;
    }
}
