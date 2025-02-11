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
        if (other.transform.CompareTag("Player"))
        {
            upgradeManager.UpgradeSelected(other, UpgradeSlot.Slot2);

            Destroy(gameObject);
            Destroy(this);
        }
    }
}
