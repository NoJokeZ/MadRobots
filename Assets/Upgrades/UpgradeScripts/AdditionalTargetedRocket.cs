using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalTargetedRocket : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/AdditionalTargetedRocket");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerSpecialAbilityAmmo += (int)UpgradeObject.AddValue;
    }
}
