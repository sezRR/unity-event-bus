using _Project._Scripts.NewTasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CoffeeMachineUpgrade", menuName = "Upgrades/CoffeeMachineUpgrade")]
public class CoffeeMachineUpgrade : MachineUpgradeBase
{
    public override void RaiseUpgradeEvent()
    {
        EventBus<UpgradeEvent>.Raise(new UpgradeEvent(this));
    }
}
