using UnityEngine;

public abstract class MachineUpgradeBase : UpgradeBase, IEquipmentUpgrade
{
    public float speedIncrease;

    public override void ApplyUpgrade(GameObject target)
    {
        var machineComponent = target.GetComponent<MachineComponent>();
        if (machineComponent != null)
        {
            machineComponent.IncreaseSpeed(speedIncrease);
        }
    }
}
