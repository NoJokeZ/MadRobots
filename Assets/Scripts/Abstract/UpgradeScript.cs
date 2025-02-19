using System.Collections;
using UnityEngine;

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
