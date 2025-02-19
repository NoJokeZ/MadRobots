using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //GameObjects
    private EventSystem eventSystem;
    private GameManager gameManager;
    private SettingsMenu settingsMenu;
    private GameObject mainMenuUI;
    private GameObject settingsMenuUI;

    //Buttons
    private Button startGameButton;
    private Button tutorialButton;
    private Button settingsButton;
    private Button quitButton;

    private void Awake()
    {
        //Cursor settings
        Cursor.lockState = CursorLockMode.Confined;
        eventSystem = EventSystem.current;

        //Gameobjects
        mainMenuUI = transform.Find("MainMenu").gameObject;
        settingsMenuUI = GameObject.Find("CanvasSettingsMenu").gameObject.transform.Find("SettingsMenu").gameObject; //After playing once the SettingsMenu GO is inactive and can't be found via normal GO.Find -> Transform.Find also finds inactive GO

        //Get all buttons
        startGameButton = mainMenuUI.transform.Find("StartGame").GetComponent<Button>();
        tutorialButton = mainMenuUI.transform.Find("Tutorial").GetComponent<Button>();
        settingsButton = mainMenuUI.transform.Find("Settings").GetComponent<Button>();
        quitButton = mainMenuUI.transform.Find("Quit").GetComponent<Button>();

        //Add all button events
        startGameButton.onClick.AddListener(PlayGame);
        tutorialButton.onClick.AddListener(PlayTutorial);
        settingsButton.onClick.AddListener(Settings);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        //Gameobjects
        gameManager = GameManager.Instance;
        settingsMenu = SettingsMenu.Instance;
    }

    /// <summary>
    /// Calls game start from game manager
    /// </summary>
    private void PlayGame()
    {
        eventSystem.SetSelectedGameObject(null);
        //Call gameManager to start the game
        gameManager.GameStart();
    }

    /// <summary>
    /// Calls tutorial start from game manager
    /// </summary>
    private void PlayTutorial()
    {
        eventSystem.SetSelectedGameObject(null);
        //Call gameManager to load Tutorial
        gameManager.TutorialStart();
    }

    /// <summary>
    /// Opens setting menu
    /// </summary>
    private void Settings()
    {
        eventSystem.SetSelectedGameObject(null);
        settingsMenuUI.SetActive(true);
        settingsMenu.PreviousMenuUI = mainMenuUI; //Sets the previous menu to main menu so settings menu knows from where it was called
        mainMenuUI.SetActive(false);

    }

    /// <summary>
    /// Quits the game
    /// </summary>
    private void QuitGame()
    {
        eventSystem.SetSelectedGameObject(null);

        Application.Quit();
    }
}

