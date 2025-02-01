using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    private EventSystem eventSystem;

    private GameManager gameManager;
    private UpgradeManager upgradeManager;
    private PlayerBehavior playerBehavior;

    private GameInput gameInput;
    private InputAction menu;

    private GameObject pauseMenuUI;

    private bool isPaused = false;

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

        eventSystem = EventSystem.current;

        gameInput = new GameInput();
        menu = gameInput.Player.Menu;

        pauseMenuUI = transform.Find("PauseMenu").gameObject;

        resumeButton = pauseMenuUI.transform.Find("Resume").GetComponent<Button>();
        settingsButton = pauseMenuUI.transform.Find("Settings").GetComponent<Button>();
        backToMainMenuButton = pauseMenuUI.transform.Find("BackToMainMenu").GetComponent<Button>();
        quitButton = pauseMenuUI.transform.Find("Quit").GetComponent<Button>();

        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(Settings);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        quitButton.onClick.AddListener(Quit);

        pauseMenuUI.SetActive(false);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        upgradeManager = UpgradeManager.Instance;
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
        if (menu.WasPressedThisFrame())
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
    }

    private void Pause()
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }


        if (playerBehavior != null || GameObject.FindWithTag("Player").TryGetComponent<PlayerBehavior>(out playerBehavior))
        {
            playerBehavior.DisableControls();
        }

        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    private void Resume()
    {
        if (playerBehavior != null || GameObject.FindWithTag("Player").TryGetComponent<PlayerBehavior>(out playerBehavior))
        {
            playerBehavior.EnableControls();
        }

        eventSystem.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    private void Settings()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    private void BackToMainMenu()
    {
        eventSystem.SetSelectedGameObject(null);


        Time.timeScale = 1.0f;
        pauseMenuUI.SetActive(false);

        gameManager.GameEnd();

    }

    private void Quit()
    {
        eventSystem.SetSelectedGameObject(null);
        Application.Quit();
    }

}
