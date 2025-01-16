using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRadiusUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/ExplosionRadiusUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerExplosionRadius += UpgradeObject.AddValue;
    }
}
