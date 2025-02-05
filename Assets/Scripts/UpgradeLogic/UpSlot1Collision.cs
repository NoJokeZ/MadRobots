using UnityEngine;

public class UpSlot1Collision : MonoBehaviour
{
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        upgradeManager = UpgradeManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        upgradeManager.UpgradeSelected(other, UpgradeSlot.Slot1);

        Destroy(gameObject);
        Destroy(this);
    }
}
