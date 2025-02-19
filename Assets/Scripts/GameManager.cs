using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Gamestate
    public GameState CurrentGameState { get; private set; }
    
    //Gameobjects
    public static GameManager Instance;
    private AudioManager audioManager;
    private UpgradeManager upgradeManager;
    public GameObject player { get; private set; }
    private Transform playerSpawn;
    private GameObject ChosenPlayerPrefab;

    //Player isalive value
    public bool IsPlayerAlive { get; private set; } = false;

    //Level values
    private int easyLevelAmount = 3;
    private int levelCounter = 0;

    //Scene info
    private List<SceneInfo> basicLevels = new();
    private List<SceneInfo> bossLevels = new();
    private List<SceneInfo> upgradeLevels = new();
    private SceneInfo tutorialLevel;
    private SceneInfo mainMenu;
    public SceneInfo currentScene { get; private set; }

    //AudioSource for music
    private AudioSource cameraMusicSource;



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

        ChosenPlayerPrefab = Resources.Load<GameObject>("Player/RocketV1");

        GetSceneInfos();

        CurrentGameState = GameState.Menu;

    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
        upgradeManager = UpgradeManager.Instance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Gets all scene infos
    /// </summary>
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

    /// <summary>
    /// On scene load logic
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (currentScene != null)
        {
            if (currentScene.SceneType != SceneType.Menu)
            {
                playerSpawn = GameObject.Find("PlayerSpawn").transform;
                player = Instantiate(ChosenPlayerPrefab, playerSpawn.position, playerSpawn.rotation);
                IsPlayerAlive = true;
                cameraMusicSource = Camera.main.GetComponent<AudioSource>();
                cameraMusicSource.loop = true;
                audioManager.PlayMusic("Theme", cameraMusicSource); 

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

    /// <summary>
    /// Start the game or next level
    /// </summary>
    public void GameStart()
    {
        if (levelCounter == 0)
        {
            upgradeManager.GetPlayerStats(PlayerType.Rocket);
        }

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

    /// <summary>
    /// Starts the tutorial level
    /// </summary>
    public void TutorialStart()
    {
        SceneManager.LoadScene(tutorialLevel.name);
        CurrentGameState = GameState.Tutorial;
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    public void GameEnd()
    {
        audioManager.StopMusic("Theme", cameraMusicSource);

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
        CurrentGameState = GameState.Menu;

        GetSceneInfos();
    }

    /// <summary>
    /// Starts the boss level
    /// </summary>
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

    /// <summary>
    /// Checks what happens after level end
    /// </summary>
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

    /// <summary>
    /// Starts the upgrade level
    /// </summary>
    private void UpgradeStart()
    {
        //Selecet random level from basic levels
        int level = Random.Range(0, upgradeLevels.Count);

        //Load that level
        SceneManager.LoadScene(upgradeLevels[level].name);
        currentScene = upgradeLevels[level];

        CurrentGameState = GameState.Upgrading;
    }

    /// <summary>
    /// Ends the upgrade level
    /// </summary>
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
