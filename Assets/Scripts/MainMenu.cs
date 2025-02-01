using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private EventSystem eventSystem;

    private GameManager gameManager;

    private Button startGameButton;
    private Button tutorialButton;
    private Button settingsButton;
    private Button creditsButton;
    private Button quitButton;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        eventSystem = EventSystem.current;

        startGameButton = transform.Find("StartGame").GetComponent<Button>();
        tutorialButton = transform.Find("Tutorial").GetComponent<Button>();
        settingsButton = transform.Find("Settings").GetComponent<Button>();
        creditsButton = transform.Find("Credits").GetComponent<Button>();
        quitButton = transform.Find("Quit").GetComponent<Button>();


        startGameButton.onClick.AddListener(PlayGame);
        tutorialButton.onClick.AddListener(PlayTutorial);
        settingsButton.onClick.AddListener(Settings);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(QuitGame);

    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void PlayGame()
    {
        eventSystem.SetSelectedGameObject(null);
        //Call gameManager to start the game
        gameManager.GameStart();
    }

    private void PlayTutorial()
    {
        eventSystem.SetSelectedGameObject(null);
        //Call gameManager to load Tutorial
        gameManager.TutorialStart();
    }

    private void Settings()
    {
        eventSystem.SetSelectedGameObject(null);

    }

    private void Credits()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    private void QuitGame()
    {
        eventSystem.SetSelectedGameObject(null);

        Application.Quit();
    }

}

