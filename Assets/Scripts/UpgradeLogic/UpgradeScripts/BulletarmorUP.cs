using UnityEngine;

public class BulletarmorUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/BulletarmorUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerBulletArmorPoints += (int)UpgradeObject.AddValue;
    }
}
