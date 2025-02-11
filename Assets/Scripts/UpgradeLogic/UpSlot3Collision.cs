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
        if (other.transform.CompareTag("Player"))
        {
            upgradeManager.UpgradeSelected(other, UpgradeSlot.Slot3);

            Destroy(gameObject);
            Destroy(this);
        }
    }
}
