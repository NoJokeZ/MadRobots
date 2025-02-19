using UnityEngine;

public class DamageUp : UpgradeScript
{

    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/DamageUP");

        base.Awake();

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
