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
    public MonoScript UpgradeScript;
    public GameObject UpgradeSymbol;


}


public abstract class UpgradeScript : MonoBehaviour
{
    protected UpgradeManager upgradeManager;
    protected Upgrade UpgradeObject;


    protected virtual void Awake()
    {
        upgradeManager = UpgradeManager.Instance;
        upgradeManager.UpgradeEvent.AddListener(Upgrade);
    }

    protected virtual void Upgrade()
    {

    }
}


