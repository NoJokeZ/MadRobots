using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //Gameobjects
    public static SettingsMenu Instance;
    private EventSystem eventSystem;
    private GameObject settingsMenuUI;
    public GameObject PreviousMenuUI { get; set; }

    //Button and slider
    private Button muteMusicButton;
    private Slider soundSlider;
    private Button muteSoundButton;
    private Slider musicSlider;
    private Button backButton;

    //Setting bools and values
    public bool IsMusicMuted { get; private set; } = false;
    public bool IsSoundMuted { get; private set; } = false;
    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }

    //Events for setting changes
    public UnityEvent OnMuteMusic { get; private set; }
    public UnityEvent OnMusicVolumeChange { get; private set; }
    public UnityEvent OnMuteSound { get; private set; }
    public UnityEvent OnSoundVolumeChange { get; private set; }

    //Images for the music and sound mute buttons
    [SerializeField] Sprite ImageButtonMusicOn;
    [SerializeField] Sprite ImageButtonMusicOff;
    [SerializeField] Sprite ImageButtonSoundOn;
    [SerializeField] Sprite ImageButtonSoundOff;

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

        //Gameobjects
        settingsMenuUI = transform.Find("SettingsMenu").gameObject;
        eventSystem = EventSystem.current;

        //Button and slider
        muteMusicButton = settingsMenuUI.transform.Find("MusicButton").GetComponent<Button>();
        musicSlider = settingsMenuUI.transform.Find("MusicSlider").GetComponent<Slider>();
        muteSoundButton = settingsMenuUI.transform.Find("SoundButton").GetComponent<Button>();
        soundSlider = settingsMenuUI.transform.Find("SoundSlider").GetComponent<Slider>();
        backButton = settingsMenuUI.transform.Find("Back").GetComponent<Button>();

        //Event subscriptions
        muteMusicButton.onClick.AddListener(MuteMusic);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        muteSoundButton.onClick.AddListener(MuteSound);
        soundSlider.onValueChanged.AddListener(ChangeSoundVulome);
        backButton.onClick.AddListener(Back);

        //Event creation
        OnMuteMusic = new();
        OnMusicVolumeChange = new();
        OnMuteSound = new();
        OnSoundVolumeChange = new();
    }

    private void Start()
    {
        settingsMenuUI.SetActive(false);
        //Gameobjects
        MusicVolume = musicSlider.value;
        SoundVolume = soundSlider.value;
    }

    /// <summary>
    /// Mutes the music, switches the illustration and invokes its associated event
    /// </summary>
    private void MuteMusic()
    {
        eventSystem.SetSelectedGameObject(null);
        if (IsMusicMuted)
        {
            IsMusicMuted = false;
            muteMusicButton.gameObject.GetComponent<Image>().sprite = ImageButtonMusicOn;
        }
        else
        {
            IsMusicMuted = true;
            muteMusicButton.gameObject.GetComponent<Image>().sprite = ImageButtonMusicOff;
        }

        OnMuteMusic.Invoke();
    }

    /// <summary>
    /// Changes the music volume and invokes its associated event
    /// </summary>
    private void ChangeMusicVolume(float volume)
    {
        eventSystem.SetSelectedGameObject(null);
        MusicVolume = volume;
        OnMusicVolumeChange.Invoke();
    }

    /// <summary>
    /// Mutes the sound, switches the illustration and invokes its associated event
    /// </summary>
    private void MuteSound()
    {
        eventSystem.SetSelectedGameObject(null);
        if (IsSoundMuted)
        {
            IsSoundMuted = false;
            muteSoundButton.gameObject.GetComponent<Image>().sprite = ImageButtonSoundOn;
        }
        else
        {
            IsSoundMuted = true;
            muteSoundButton.gameObject.GetComponent<Image>().sprite = ImageButtonSoundOff;
        }
        OnMuteSound.Invoke();
    }

    /// <summary>
    /// Changes the sound volume and invokes its associated event
    /// </summary>
    private void ChangeSoundVulome(float volume)
    {
        eventSystem.SetSelectedGameObject(null);
        SoundVolume = volume;
        OnSoundVolumeChange.Invoke();
    }

    /// <summary>
    /// Goes back to the previous menu
    /// </summary>
    private void Back()
    {
        eventSystem.SetSelectedGameObject(null);
        PreviousMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
}
