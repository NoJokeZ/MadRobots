using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/ArmorUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerBulletArmorPoints += (int)UpgradeObject.AddValue;
        upgradeManager.PlayerExplosionArmorPoints += (int)UpgradeObject.AddValue;
    }
}
