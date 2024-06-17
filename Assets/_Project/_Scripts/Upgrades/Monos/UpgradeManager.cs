using System;
using System.Linq;
using UnityEngine;

// TODO: SET COSTS AUTO WITH LEVEL (OWNED), ADD INITIAL PRICE PROPERTY TO UPGRADES
// TODO: COSTSMANAGER? BALANCEMANAGER?
// TODO: RATIOUS MUST BE PERCENTAGES.
public class UpgradeManager : MonoBehaviour
{
    public UpgradeBase[] availableUpgrades;
    public GameObject barista;
    public GameObject equipment;
    public GameObject decor;
    public GameObject menu;

    public void ApplyUpgrade<T>() where T : IUpgradeable
    {
        var upgrades = availableUpgrades.OfType<T>();
        foreach (var upgrade in upgrades)
        {
            var upgradeInstance = Instantiate(upgrade as UpgradeBase);
            if (typeof(IBaristaUpgrade).IsAssignableFrom(typeof(T)))
            {
                upgradeInstance.ApplyUpgrade(barista);
            }
            else if (typeof(IEquipmentUpgrade).IsAssignableFrom(typeof(T)))
            {
                upgradeInstance.ApplyUpgrade(equipment);
            }
            else if (typeof(IDecorUpgrade).IsAssignableFrom(typeof(T)))
            {
                upgradeInstance.ApplyUpgrade(decor);
            }
            else if (typeof(IMenuUpgrade).IsAssignableFrom(typeof(T)))
            {
                upgradeInstance.ApplyUpgrade(menu);
            }
        }
    }

    public void ApplyUpgrade(UpgradeBase upgrade)
    {
        var upgradeInstance = Instantiate(upgrade);

        switch (upgrade)
        {
            case IBaristaUpgrade:
                upgradeInstance.ApplyUpgrade(barista);
                break;
            case IEquipmentUpgrade:
                upgradeInstance.ApplyUpgrade(equipment);
                break;
            case IDecorUpgrade:
                upgradeInstance.ApplyUpgrade(decor);
                break;
            case IMenuUpgrade:
                upgradeInstance.ApplyUpgrade(menu);
                break;
        }
    }
}
