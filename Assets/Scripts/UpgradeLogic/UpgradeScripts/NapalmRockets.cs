using UnityEngine;

public class NapalmRockets : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/NapalmRockets");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.NapalmRockets = UpgradeObject.ActivateValue;
    }
}
