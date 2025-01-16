using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleExplosion : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/DoubleExplosion");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.DoubleExplosion = UpgradeObject.ActivateValue;
    }
}
