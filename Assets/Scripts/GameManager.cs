using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player { get; private set; }
    private Transform playerSpawn;
    private GameObject ChosenPlayerPrefab;
    public bool IsPlayerAlive { get; private set; } = false;

    private int easyLevelAmount = 3;
    private int levelCounter = 0;

    public GameState CurrentGameState { get; private set; }

    private List<SceneInfo> basicLevels = new();
    private List<SceneInfo> bossLevels = new();
    private List<SceneInfo> upgradeLevels = new();
    private SceneInfo tutorialLevel;
    private SceneInfo mainMenu;

    private SceneInfo currentScene;





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

        ChosenPlayerPrefab = Resources.Load<GameObject>("RocketV1");

        GetSceneInfos();

        CurrentGameState = GameState.Menu;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void OnPlayerDeath()
    {
        //GameEnd();
    }

    private void GetSceneInfos()
    {
        SceneInfo[] allScenes = Resources.LoadAll<SceneInfo>(""); //Load all scene infos there are

        //Sort all scene infos
        foreach (SceneInfo sceneInfo in allScenes)
        {
            switch (sceneInfo.SceneType)
            {
                case SceneType.BasicLevel:
                    basicLevels.Add(sceneInfo);
                    break;
                case SceneType.BossLevel:
                    bossLevels.Add(sceneInfo);
                    break;
                case SceneType.UpgradeArea:
                    upgradeLevels.Add(sceneInfo);
                    break;
                case SceneType.Tutorial:
                    tutorialLevel = sceneInfo;
                    break;
                case SceneType.Menu:
                    mainMenu = sceneInfo;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (currentScene != null)
        {
            if (currentScene.SceneType != SceneType.Menu)
            {

                playerSpawn = GameObject.Find("PlayerSpawn").transform;
                player = Instantiate(ChosenPlayerPrefab, playerSpawn.position, playerSpawn.rotation);
                IsPlayerAlive = true;

                if (currentScene.SceneType == SceneType.Tutorial)
                {
                    CurrentGameState = GameState.Tutorial;
                }
                else
                {
                    CurrentGameState = GameState.Running;
                }
            }

        }
    }

    public void GameStart()
    {
        //Selecet random level from basic levels
        int level = Random.Range(0, basicLevels.Count);

        //Load that level
        SceneManager.LoadScene(basicLevels[level].name);
        currentScene = basicLevels[level];

        //Remove the level from the available levels
        basicLevels.Remove(basicLevels[level]);
        //Up the level count
        levelCounter++;

        CurrentGameState = GameState.Running;
    }

    public void TutorialStart()
    {
        SceneManager.LoadScene(tutorialLevel.name);
        CurrentGameState = GameState.Tutorial;
    }

    public void GameEnd()
    {
        //Clears all level Lists
        basicLevels.Clear();
        bossLevels.Clear();
        upgradeLevels.Clear();

        //Resets the level count
        levelCounter = 0;

        //Destroys the player if still alive (Menu exit)
        if (player != null)
        {
            Destroy(player);
            IsPlayerAlive = false;
        }

        //Loads the main menu
        SceneManager.LoadScene(mainMenu.name);
        currentScene = mainMenu;

        GetSceneInfos();
    }

    private void BossStart()
    {
        //Selecet random level from basic levels
        int level = Random.Range(0, bossLevels.Count);

        //Load that level
        SceneManager.LoadScene(bossLevels[level].name);
        currentScene = bossLevels[level];

        //Remove the level from the available levels
        bossLevels.Remove(bossLevels[level]);

        CurrentGameState = GameState.Running;
    }

    public void LevelEnd()
    {
        if (currentScene.SceneType == SceneType.BossLevel)
        {
            GameEnd(); 
        }
        else
        {
            UpgradeStart();
        }
    }

    private void UpgradeStart()
    {
        //Selecet random level from basic levels
        int level = Random.Range(0, upgradeLevels.Count);

        //Load that level
        SceneManager.LoadScene(upgradeLevels[level].name);
        currentScene = upgradeLevels[level];

        CurrentGameState = GameState.Upgrading;
    }

    public void UpgradeEnd()
    {
        if (levelCounter < easyLevelAmount)
        {
            GameStart();
        }
        else if (levelCounter == easyLevelAmount)
        {
            BossStart();
        }
    }

}
