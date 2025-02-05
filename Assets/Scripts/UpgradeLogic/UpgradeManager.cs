using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    //Self instance
    public static UpgradeManager Instance; //property

    //Player objects
    private GameObject player;
    private PlayerBehavior playerBehavior;
    public PlayerType CurrentPlayerType;

    //Upgrade parent object
    private GameObject upgradeObject;

    //Transforms for upgrade cubes
    private Transform upgradeTransform1;
    private Transform upgradeTransform2;
    private Transform upgradeTransform3;
    private Transform upgradeTransform4;

    //Upgrade event
    public UnityEvent UpgradeEvent;

    //Upgrade selected bool
    public bool upgradeSelected = false;

    //Upgrade array
    private Upgrade[] upgrades = new Upgrade[4];

    //List of available Upgrades
    private List<Upgrade> availableCommonUpgrades = new();
    private List<Upgrade> availableRareUpgrades = new();
    private List<Upgrade> availableLegendaryUpgrades = new();

    //Upgrance appearance chance
    private const float commonChance = 0.70f; //70%
    private const float rareChance = 0.95f;    //25%
    private const float legendaryChance = 1f; //5%


    //Player Values
    //Player health
    public int PlayerMaxHealth;
    public int PlayerHealthRegeneration;

    //Player armor
    public int PlayerBulletArmorPoints;
    public int PlayerExplosionArmorPoints;

    //Player movement
    public float PlayerMoveSpeed;
    public float PlayerGroundAcceleration;
    public float PlayerAirAcceleration;
    public float PlayerJetPackPower;
    public float PlayerJetPackMaxDuration;

    //Player damage
    public float PlayerProjectileDamage;
    public float PlayerMissleDamage;
    public float PlayerExplosionDamage;
    public float PlayerExplosionRadius;

    //Player firerate
    public float PlayerFirerate;

    //Ammo
    public int PlayerSpecialAbilityAmmo;

    //RocketV1 bools
    public bool DoubleExplosion = false;
    public bool NapalmRockets = false;

    private PlayerStats playerStats;

    //GameManger
    private GameManager gameManager;



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

        //Even creating
        if (UpgradeEvent == null)
        {
            UpgradeEvent = new UnityEvent();
        }

        StartCoroutine(GetPlayerObjects());


        //Get all available Upgrades for current player type
        GetAvailableUpgrades();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnUpgradeSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnUpgradeSceneLoad;
    }


    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void Update()
    {
        //Only for testing and debugging because player can respawn in testing
        if (player == null)
        {
            GetPlayerObjects();
        }
    }

    /// <summary>
    /// Get random Upgrades on upgrade scene Load
    /// </summary>
    public void OnUpgradeSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }


        if (gameManager.CurrentGameState == GameState.Upgrading)
        {
            //Create new event if not existing
            if (UpgradeEvent == null)
            {
                UpgradeEvent = new UnityEvent();
            }

            upgradeSelected = false;

            GetRandomUpgrade();

            SpawnUpgradeCubes();
        }

    }


    /// <summary>
    /// Gets all needed values from the player
    /// </summary>
    private IEnumerator GetPlayerObjects()
    {
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");
            yield return null;
        }

        playerBehavior = player.GetComponent<PlayerBehavior>();
        CurrentPlayerType = playerBehavior.myType;

        CurrentPlayerType = PlayerType.Rocket; //NEEEDED ONLY UNTIL MENU AND PLAYER SELECTION IS IMPLEMENTED


        //Get player stats
        GetPlayerStats(CurrentPlayerType);
    }

    /// <summary>
    /// Gets all Upgrades and distributes all available for current player type in their respectiv rarity lists
    /// </summary>
    private void GetAvailableUpgrades()
    {
        Upgrade[] allUpgrades; //New upgrade array for all upgrades

        allUpgrades = Resources.LoadAll<Upgrade>("UpgradeObjects"); //Load all upgrades there are

        //Distribut all universal upgrade and player specific upgrade into their rarities
        foreach (Upgrade upgrade in allUpgrades)
        {
            if (upgrade.PlayerType == PlayerType.Universal || upgrade.PlayerType == CurrentPlayerType)
            {
                if (upgrade.Rarity == UpgradeRarity.Common)
                {
                    availableCommonUpgrades.Add(upgrade);
                }
                else if (upgrade.Rarity == UpgradeRarity.Rare)
                {
                    availableRareUpgrades.Add(upgrade);
                }
                else if (upgrade.Rarity == UpgradeRarity.Legendary)
                {
                    availableLegendaryUpgrades.Add(upgrade);
                }
            }
        }
    }

    /// <summary>
    /// Fills the upgrade selection array with random upgrades
    /// </summary>
    private void GetRandomUpgrade()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            float tempUpgradeRarity = UnityEngine.Random.Range(0f, 1f);
            if (tempUpgradeRarity <= commonChance) // <= 0.7
            {
                upgrades[i] = GetCommonUpgrade();
            }
            else if (tempUpgradeRarity <= rareChance)  // <= 0.95
            {
                upgrades[i] = GetRareUpgrade();
            }

            else if (tempUpgradeRarity <= legendaryChance) // <= 1
            {
                upgrades[i] = GetLegendaryUpgrade();
            }
        }
    }

    /// <summary>
    /// Gets a random common upgrade and removes it from the current available common upgrades
    /// </summary>
    /// <returns></returns>
    private Upgrade GetCommonUpgrade()
    {
        int random = UnityEngine.Random.Range(0, availableCommonUpgrades.Count);
        Upgrade upgrade = availableCommonUpgrades[random];
        availableCommonUpgrades.Remove(upgrade);
        return upgrade;
    }

    /// <summary>
    /// Gets a random rare upgrade and removes it from the current available rare upgrades
    /// </summary>
    /// <returns></returns>
    private Upgrade GetRareUpgrade()
    {
        int random = UnityEngine.Random.Range(0, availableRareUpgrades.Count);
        Upgrade upgrade = availableRareUpgrades[random];
        availableCommonUpgrades.Remove(upgrade);
        return upgrade;
    }

    /// <summary>
    /// Gets a random legendary upgrade and removes it from the current available legendary upgrades
    /// </summary>
    /// <returns></returns>
    private Upgrade GetLegendaryUpgrade()
    {
        int random = UnityEngine.Random.Range(0, availableLegendaryUpgrades.Count);
        Upgrade upgrade = availableLegendaryUpgrades[random];
        availableCommonUpgrades.Remove(upgrade);
        return upgrade;
    }

    /// <summary>
    /// Resets the upgrade selection
    /// </summary>
    private void ResetUpgrade(UpgradeSlot slot)
    {

        //Upgrade temp = gameObject.GetComponent<Upgrade>();


        Destroy(gameObject.GetComponent<UpgradeScript>());

        //Clears the upgrade event
        UpgradeEvent = new UnityEvent();

        //Puts the reusable upgrade back in their respectiv lists
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].IsStackable || i != (int)slot) //If the Upgrade is stackable or the upgrade was not chosen
            {
                switch (upgrades[i].Rarity)
                {
                    case UpgradeRarity.Common:
                        availableCommonUpgrades.Add(upgrades[i]);
                        break;
                    case UpgradeRarity.Rare:
                        availableRareUpgrades.Add(upgrades[i]);
                        break;
                    case UpgradeRarity.Legendary:
                        availableLegendaryUpgrades.Add(upgrades[i]);
                        break;
                }
            }

            //Clears the upgrade slot
            upgrades[i] = null;

        }
    }

    /// <summary>
    /// Adds the selected upgrade SO 
    ///     -> SO adds itself to the upgrade event
    ///         -> Upgrade event upgrades palyer
    /// </summary>
    /// <param name="other"></param>
    /// <param name="slot"></param>
    public void UpgradeSelected(Collider other, UpgradeSlot slot)
    {
        if (upgradeSelected) return;
        upgradeSelected = true;

        //Adds the selects SO
        gameObject.AddComponent(upgrades[(int)slot].UpgradeScript.GetClass());

        //Activate the upgrade from the SO
        UpgradeEvent.Invoke();

        //Resets the upgrade selection
        ResetUpgrade(slot);


        gameManager.UpgradeEnd();
        
    }

    /// <summary>
    /// Gets player base stats
    /// </summary>
    /// <param name="playerType"></param>
    private void GetPlayerStats(PlayerType playerType)
    {
        //Load SO
        switch (playerType)
        {
            case PlayerType.Universal:
                playerStats = Resources.Load<PlayerStats>("UniversalStats");
                break;

            case PlayerType.Rocket:
                playerStats = Resources.Load<PlayerStats>("RocketV1Stats");
                break;
        }

        //Player Values
        //Player health
        PlayerMaxHealth = playerStats.PlayerMaxHealth;
        PlayerHealthRegeneration = playerStats.PlayerHealthRegeneration;

        //Player armor
        PlayerBulletArmorPoints = playerStats.PlayerBulletArmorPoints;
        PlayerExplosionArmorPoints = playerStats.PlayerExplosionArmorPoints;

        //Player movement
        PlayerMoveSpeed = playerStats.PlayerMoveSpeed;
        PlayerGroundAcceleration = playerStats.PlayerGroundAcceleration;
        PlayerAirAcceleration = playerStats.PlayerAirAcceleration;
        PlayerJetPackPower = playerStats.PlayerJetPackPower;
        PlayerJetPackMaxDuration = playerStats.PlayerJetPackMaxDuration;

        //Player damage
        PlayerProjectileDamage = playerStats.PlayerProjectileDamage;
        PlayerMissleDamage = playerStats.PlayerMissleDamage;
        PlayerExplosionDamage = playerStats.PlayerExplosionDamage;
        PlayerExplosionRadius = playerStats.PlayerExplosionRadius;

        //Player firerate
        PlayerFirerate = playerStats.PlayerFirerate;

        //Ammo
        PlayerSpecialAbilityAmmo = playerStats.PlayerSpecialAbilityAmmo;
    }


    private void SpawnUpgradeCubes()
    {
        upgradeTransform1 = GameObject.Find("Upgrade1").transform;
        upgradeTransform2 = GameObject.Find("Upgrade2").transform;
        upgradeTransform3 = GameObject.Find("Upgrade3").transform;
        upgradeTransform4 = GameObject.Find("Upgrade4").transform;


        Instantiate(upgrades[0].UpgradeSymbol, upgradeTransform1.position, upgradeTransform1.rotation);
        Instantiate(upgrades[1].UpgradeSymbol, upgradeTransform2.position, upgradeTransform1.rotation);
        Instantiate(upgrades[2].UpgradeSymbol, upgradeTransform3.position, upgradeTransform1.rotation);
        Instantiate(upgrades[3].UpgradeSymbol, upgradeTransform4.position, upgradeTransform1.rotation);
    }
}

