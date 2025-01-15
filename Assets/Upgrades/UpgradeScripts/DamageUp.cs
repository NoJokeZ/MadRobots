using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class DamageUp : UpgradeScript
{


    protected override void Awake()
    {
        base.Awake();

        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/DamageUP");

        upgradeManager.UpgradeEvent.AddListener(Upgrade);

        Destroy(this);
    }

    protected override void Upgrade()
    {
        if (upgradeManager.CurrentPlayerType == PlayerType.Rocket)
        {
            upgradeManager.PlayerExplosionDamage += UpgradeObject.AddValue;
        }
        else
        {
            upgradeManager.PlayerProjectileDamage += UpgradeObject.AddValue;
        }
    }
}
