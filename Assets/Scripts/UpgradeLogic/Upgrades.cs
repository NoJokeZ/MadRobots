using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static PlayerBehavior;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public PlayerBehavior.PlayerType PlayerType;
    public UpgradeRarity Rarity;
    public UpgradeType Type;
    public float AddValue;
    public float AmplifyValue;
    public bool ActivateValue;
    public MonoScript UpgradeScript;
    public GameObject UpgradeSymbol;
}

public enum UpgradeRarity
{
    Common,
    Rare,
    Legendary
}

public enum UpgradeType
{
    Add,
    Amplify,
    Activate
}

public abstract class UpgradeScript : MonoBehaviour
{
    protected UpgradeManager upgradeManager;

    protected virtual void Awake()
    {
        upgradeManager = UpgradeManager.Instance;
    }
}


