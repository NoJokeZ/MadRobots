using UnityEngine;

public class HealthRegenerationUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/HeatlhRegenerationUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerHealthRegeneration += (int)UpgradeObject.AddValue;
    }
}
