using _Project._Scripts.NewTasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class UpgradeBase : ScriptableObject, IUpgradeable
{
    public string upgradeName;
    public string description;
    public int level;
    public float cost;
    
    public abstract void ApplyUpgrade(GameObject target);

    public abstract void RaiseUpgradeEvent();
}
