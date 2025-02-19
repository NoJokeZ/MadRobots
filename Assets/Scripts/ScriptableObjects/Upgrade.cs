using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public PlayerType PlayerType;
    public UpgradeRarity Rarity;
    public UpgradeType Type;
    public bool IsStackable;
    public float AddValue;
    public float AmplifyValue;
    public bool ActivateValue;
    public string UpgradeScriptName;
    public GameObject UpgradeSymbol;

}


