using UnityEngine;

public class VolumeChangerSound : MonoBehaviour
{
    private SettingsMenu settingsMenu;

    private AudioSource soundSoruce;

    private void Awake()
    {
        soundSoruce = GetComponent<AudioSource>();
    }

    private void Start()
    {
        settingsMenu = SettingsMenu.Instance;

        settingsMenu.OnMuteSound.AddListener(ChangeMute);
        settingsMenu.OnSoundVolumeChange.AddListener(ChangeVolume);

        ChangeMute();
        ChangeVolume();
    }


    private void OnDisable()
    {
        settingsMenu.OnMuteSound.RemoveListener(ChangeMute);
        settingsMenu.OnSoundVolumeChange.RemoveListener(ChangeVolume);
    }

    /// <summary>
    /// Change mute state
    /// </summary>
    private void ChangeMute()
    {
        soundSoruce.mute = settingsMenu.IsSoundMuted;
    }

    /// <summary>
    /// Change volume value
    /// </summary>
    private void ChangeVolume()
    {
        soundSoruce.volume = settingsMenu.SoundVolume;
    }

}
