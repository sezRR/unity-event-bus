using System;
using UnityEngine;

public abstract class UpgradeBase : ScriptableObject, IUpgradeable
{
    public string id = Guid.NewGuid().ToString();
    public string upgradeName;
    public string description;
    public int level;
    public float cost;

    public abstract void ApplyUpgrade(GameObject target);

    public abstract void RaiseUpgradeEvent();
}
