using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
        eventSystem = EventSystem.current;

        gameManager = GameManager.Instance;

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

    private void PlayGame()
    {
        eventSystem.SetSelectedGameObject(null);
        gameManager.GameStart();
    }

    private void PlayTutorial()
    {
        eventSystem.SetSelectedGameObject(null);
        //StartCoroutine(gameManager.TutorialStartCO());
        SceneManager.LoadScene("Tutorial");
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

    private IEnumerator TempCO()
    {
        yield return null;
    }
}

