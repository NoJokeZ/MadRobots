using UnityEngine;

public class UpSlot3Collision : MonoBehaviour
{
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        upgradeManager = UpgradeManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        upgradeManager.UpgradeSelected(other, UpgradeSlot.Slot3);
    }
}
