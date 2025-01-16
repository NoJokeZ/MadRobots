using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerateUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/FirerateUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerFirerate *= UpgradeObject.AmplifyValue;
    }
}
