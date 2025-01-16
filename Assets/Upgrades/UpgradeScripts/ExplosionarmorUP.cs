using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionarmorUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/ExplosionarmorUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerExplosionArmorPoints += (int)UpgradeObject.AddValue;
    }
}
