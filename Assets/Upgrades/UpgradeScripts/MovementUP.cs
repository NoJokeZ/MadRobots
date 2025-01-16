using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUP : UpgradeScript
{
    protected override void Awake()
    {
        UpgradeObject = Resources.Load<Upgrade>("UpgradeObjects/MovementUP");

        base.Awake();

    }

    protected override void Upgrade()
    {
        upgradeManager.PlayerMoveSpeed *= UpgradeObject.AmplifyValue;
        upgradeManager.PlayerGroundAcceleration *= UpgradeObject.AmplifyValue;
        upgradeManager.PlayerAirAcceleration *= UpgradeObject.AmplifyValue;
    }
}
