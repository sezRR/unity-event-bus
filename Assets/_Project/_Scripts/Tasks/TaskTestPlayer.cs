using _Project._Scripts.NewTasks;
using UnityEngine;

public class TaskTestPlayer : MonoBehaviour
{
    [SerializeField] private UpgradeBase[] availableUpgrades;
    [SerializeField] private string resourceName = "Wood";
    [SerializeField] private int resourceAmount = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) // Example key to purchase an upgrade
            if (availableUpgrades.Length > 0)
                availableUpgrades[0].RaiseUpgradeEvent();

        if (Input.GetKeyDown(KeyCode.C)) // Example key to collect resources
            EventBus<ResourceCollectedEvent>.Raise(new ResourceCollectedEvent(resourceName, resourceAmount));
    }
}