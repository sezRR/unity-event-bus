using _Project._Scripts.NewTasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuExpansionUpgrade", menuName = "Upgrades/MenuExpansionUpgrade")]
public class MenuExpansionUpgrade : MenuUpgradeBase
{
    public override void RaiseUpgradeEvent()
    {
        EventBus<UpgradeEvent>.Raise(new UpgradeEvent(this));
    }
}
