using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSlot4Collision : MonoBehaviour
{
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        upgradeManager = UpgradeManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        upgradeManager.UpgradeSelected(other, UpgradeSlot.Slot4);
    }
}
