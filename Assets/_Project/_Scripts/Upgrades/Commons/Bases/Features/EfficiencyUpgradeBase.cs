using UnityEngine;

public abstract class EfficiencyUpgradeBase : UpgradeBase, IBaristaUpgrade
{
    public float efficiencyIncrease;

    public override void ApplyUpgrade(GameObject target)
    {
        var baristaComponent = target.GetComponent<BaristaComponent>();
        if (baristaComponent != null)
        {
            baristaComponent.IncreaseEfficiency(efficiencyIncrease);
        }
        
        RaiseUpgradeEvent();
    }
}
