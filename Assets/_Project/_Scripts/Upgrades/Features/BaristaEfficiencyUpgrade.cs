using _Project._Scripts.NewTasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BaristaEfficiencyUpgrade", menuName = "Upgrades/BaristaEfficiencyUpgrade")]
public class BaristaEfficiencyUpgrade : EfficiencyUpgradeBase
{
    public override void RaiseUpgradeEvent()
    {
        EventBus<UpgradeEvent>.Raise(new UpgradeEvent(this));
    }
}
