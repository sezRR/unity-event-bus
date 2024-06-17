using UnityEngine;

public abstract class MenuUpgradeBase : UpgradeBase, IMenuUpgrade
{
    public float menuVarietyIncrease;

    public override void ApplyUpgrade(GameObject target)
    {
        var menuComponent = target.GetComponent<MenuComponent>();
        if (menuComponent != null)
        {
            menuComponent.IncreaseMenuVariety(menuVarietyIncrease);
        }

        RaiseUpgradeEvent();
    }
}
