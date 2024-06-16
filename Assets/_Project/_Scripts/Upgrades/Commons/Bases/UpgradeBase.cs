using UnityEngine;
using UnityEngine.Events;

public abstract class UpgradeBase : ScriptableObject, IUpgradeable
{
    // TODO: ENCAPSULATION
    public string upgradeName;
    public int level;
    public float cost;
    
    public abstract void ApplyUpgrade(GameObject target);
}
