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

        if (upgrade is IBaristaUpgrade)
        {
            upgradeInstance.ApplyUpgrade(barista);
        }
        else if (upgrade is IEquipmentUpgrade)
        {
            upgradeInstance.ApplyUpgrade(equipment);
        }
        else if (upgrade is IDecorUpgrade)
        {
            upgradeInstance.ApplyUpgrade(decor);
        }
        else if (upgrade is IMenuUpgrade)
        {
            upgradeInstance.ApplyUpgrade(menu);
        }
    }
}
