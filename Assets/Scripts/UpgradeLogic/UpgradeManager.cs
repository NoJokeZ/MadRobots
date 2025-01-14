using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private GameObject player;
    private PlayerBehavior playerBehavior;
    private PlayerBehavior.PlayerType playerType;

    private GameObject upgradeObject;

    private Collider upgradeCollider1;
    private Collider upgradeCollider2;
    private Collider upgradeCollider3;
    private Collider upgradeCollider4;

    private Upgrade[] upgrades = new Upgrade[4];

    private Transform playerResetPosition;

    //Player Values
    //Player health
    public int PlayerHealth { get; private set; } = 20;

    //Player armor
    public int PlayerBulletArmorPoints { get; private set; } = 2;
    public int PlayerExplosionArmorPoints { get; private set; } = 2;

    //Player movement
    public float PlayerMoveSpeed { get; private set; } = 5f;
    public float PlayerGroundAcceleration { get; private set; } = 10f;
    public float PlayerAirAcceleration { get; private set; } = 2f;
    public float PlayerJetPackPower { get; private set; } = 3f;
    public float PlayerJetPackMaxDuration { get; private set; } = 3f;

    //Player damage
    public int PlayerProjectileDamage { get; private set; } = 5;
    public int PlayerMissleDamage { get; private set; } = 1;
    public int PlayerExplosionDamage { get; private set; } = 5;
    public float PlayerExplostionRadius { get; private set; } = 0.5f;


    //Upgrades
    public UnityEvent UpgradeEvent;

    private List<Upgrade> availableCommonUpgrades;
    private List<Upgrade> availableRareUpgrades;
    private List<Upgrade> availableLegendaryUpgrades;

    private const float commonChance = 0.70f; //70%
    private const float rareChance = 0.95f;    //25%
    private const float legendaryChance = 1f; //5%


    private void Awake()
    {
        GetAvailableUpgrades();

        if ( UpgradeEvent == null)
        {
            UpgradeEvent = new UnityEvent();
        }


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }

        playerResetPosition = upgradeObject.transform.Find("PlayerResetPosition");
    }

    private void Update()
    {
        if (player == null)
        {
            GetPlayerObjects();
        }
    }

    private void GetPlayerObjects()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerBehavior = player.GetComponent<PlayerBehavior>();
            playerType = playerBehavior.myType;
        }
    }

    private void GetAvailableUpgrades()
    {
        Upgrade[] allUpgrades;

        allUpgrades = Resources.LoadAll<Upgrade>("Upgrades");

        foreach (Upgrade upgrade in allUpgrades)
        {
            if (upgrade.PlayerType == PlayerBehavior.PlayerType.Universal && upgrade.PlayerType == playerType)
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

    private Upgrade GetCommonUpgrade()
    {
        int random = UnityEngine.Random.Range(0, availableCommonUpgrades.Count);
        Upgrade upgrade = availableCommonUpgrades[random];
        availableCommonUpgrades.Remove(upgrade);
        return upgrade;
    }

    private Upgrade GetRareUpgrade()
    {
        int random = UnityEngine.Random.Range(0, availableRareUpgrades.Count);
        Upgrade upgrade = availableRareUpgrades[random];
        availableCommonUpgrades.Remove(upgrade);
        return upgrade;
    }

    private Upgrade GetLegendaryUpgrade()
    {
        int random = UnityEngine.Random.Range(0, availableLegendaryUpgrades.Count);
        Upgrade upgrade = availableLegendaryUpgrades[random];
        availableCommonUpgrades.Remove(upgrade);
        return upgrade;
    }

    private void DoUpgrade()
    {
        UpgradeEvent.Invoke();

        UpgradeEvent = new UnityEvent();
        
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].IsStackable)
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

            upgrades[i] = null;

        }
    }

    public void DamageUP()
    {
        PlayerExplosionDamage += 5;
    }

    public void UpgradeSelected(Collider other,UpgradeSlot slot)
    {
        gameObject.AddComponent(upgrades[(int)slot].UpgradeScript.GetClass());
    }
}

public enum UpgradeSlot
{
    Slot1,
    Slot2,
    Slot3,
    Slot4
};
