using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //Gameobjects
    public static PauseMenu Instance;
    private EventSystem eventSystem;
    private GameManager gameManager;
    private PlayerBehaviour playerBehavior;
    private SettingsMenu settingsMenu;
    private GameObject pauseMenuUI;
    private GameObject settingsMenuUI;

    //Game input
    private GameInput gameInput;
    private InputAction menu;

    //Paused and settings values
    private bool isPaused = false;
    private bool isSettingsOpen = false;

    //Buttons
    private Button resumeButton;
    private Button settingsButton;
    private Button backToMainMenuButton;
    private Button quitButton;

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
        eventSystem = EventSystem.current;
        gameInput = new GameInput();
        menu = gameInput.Player.Menu;
        pauseMenuUI = transform.Find("PauseMenu").gameObject;
        settingsMenuUI = GameObject.Find("SettingsMenu").gameObject;

        //Get buttons
        resumeButton = pauseMenuUI.transform.Find("Resume").GetComponent<Button>();
        settingsButton = pauseMenuUI.transform.Find("Settings").GetComponent<Button>();
        backToMainMenuButton = pauseMenuUI.transform.Find("BackToMainMenu").GetComponent<Button>();
        quitButton = pauseMenuUI.transform.Find("Quit").GetComponent<Button>();

        //Add button events
        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(Settings);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        quitButton.onClick.AddListener(Quit);

        //Disavle paus menu on start
        pauseMenuUI.SetActive(false);
    }

    private void Start()
    {
        //Gameobjects
        gameManager = GameManager.Instance;
        settingsMenu = SettingsMenu.Instance;
    }

    private void OnEnable()
    {
        menu.Enable();
    }

    private void OnDisable()
    {
        if (menu != null)
        {
            menu.Disable();
        }
    }

    void Update()
    {
        if (menu.WasPressedThisFrame() && gameManager.CurrentGameState != GameState.Menu && !isSettingsOpen)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (isSettingsOpen && settingsMenuUI.activeSelf == false) isSettingsOpen = false;
    }

    /// <summary>
    /// Pauses the game and shows the pause menu
    /// </summary>
    private void Pause()
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }


        if (playerBehavior != null || gameManager.player.TryGetComponent<PlayerBehaviour>(out playerBehavior)) //Deactivates player controls
        {
            playerBehavior.DisableControls();
        }

        //Cursor setting
        Cursor.lockState = CursorLockMode.Confined;

        pauseMenuUI.SetActive(true);

        Time.timeScale = 0;
        isPaused = true;
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    private void Resume()
    {
        if (playerBehavior != null) //Reactivates player controls
        {
            playerBehavior.EnableControls();
        }

        eventSystem.SetSelectedGameObject(null);

        //Cursor settings
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenuUI.SetActive(false);

        Time.timeScale = 1.0f;

        isPaused = false;
    }

    /// <summary>
    /// Opens the settings menu
    /// </summary>
    private void Settings()
    {
        eventSystem.SetSelectedGameObject(null);
        settingsMenuUI.SetActive(true);
        settingsMenu.PreviousMenuUI = pauseMenuUI;
        isSettingsOpen = true;
        pauseMenuUI.SetActive(false);
    }

    /// <summary>
    /// Calls the game manager game end method
    /// </summary>
    private void BackToMainMenu()
    {
        eventSystem.SetSelectedGameObject(null);

        pauseMenuUI.SetActive(false);

        Time.timeScale = 1.0f;

        isPaused = false;

        gameManager.GameEnd();

    }

    /// <summary>
    /// Closes the game
    /// </summary>
    private void Quit()
    {
        eventSystem.SetSelectedGameObject(null);
        Application.Quit();
    }

}
