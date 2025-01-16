using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/HealthUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerMaxHealth += (int)UpgradeObject.AddValue;
    }
}
