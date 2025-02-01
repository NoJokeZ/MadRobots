using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject player;
    private Transform playerSpawn;
    private GameObject ChosenPlayerPrefab;
    public bool IsPlayerAlive { get; private set; } = false;

    private int easyLevelAmount = 3;
    private int levelCounter = 0;

    private bool isGameRunning = false;
    private bool isTutorialRunning = false;

    private GameState gameState;

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
                Instantiate(ChosenPlayerPrefab, playerSpawn.position, playerSpawn.rotation);
                IsPlayerAlive = true;

                isTutorialRunning = true;

                if (currentScene.SceneType == SceneType.Tutorial)
                {
                    gameState = GameState.Tutorial;
                }
                else
                {
                    gameState = GameState.Running;
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
    }

    public void TutorialStart()
    {
        SceneManager.LoadScene(tutorialLevel.name);
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

        //Set the game state to not running //Probably changing to an enum soon
        isGameRunning = false;

        //Loads the main menu
        SceneManager.LoadScene(mainMenu.name);
        currentScene = mainMenu;

        GetSceneInfos();
    }

    private void LoadBossLevel()
    {
        //Selecet random level from basic levels
        int level = Random.Range(0, bossLevels.Count);

        //Load that level
        SceneManager.LoadScene(bossLevels[level].name);
        currentScene = bossLevels[level];

        //Remove the level from the available levels
        bossLevels.Remove(bossLevels[level]);
    }

    public void LevelEnd()
    {
        if (currentScene.SceneType == SceneType.BossLevel)
        {
            GameEnd(); //Krank einfach gewonnen
        }
        else if (levelCounter < easyLevelAmount)
        {
            GameStart();
        }
        else if (levelCounter == easyLevelAmount)
        {
            LoadBossLevel();
        }
    }

}
