using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSlot2Collision : MonoBehaviour
{
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        upgradeManager = UpgradeManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        upgradeManager.UpgradeSelected(other, UpgradeSlot.Slot2);
    }
}
