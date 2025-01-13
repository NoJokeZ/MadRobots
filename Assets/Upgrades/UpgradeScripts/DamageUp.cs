using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : UpgradeScript
{
    protected override void Awake()
    {
        base.Awake();

        upgradeManager.UpgradeEvent.AddListener(upgradeManager.DamageUP);

        Destroy(this);
    }
}
