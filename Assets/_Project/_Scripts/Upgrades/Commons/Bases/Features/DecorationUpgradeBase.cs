using UnityEngine;

public abstract class DecorationUpgradeBase : UpgradeBase, IDecorUpgrade
{
    public float attractivenessIncrease;

    public override void ApplyUpgrade(GameObject target)
    {
        var decorComponent = target.GetComponent<DecorComponent>();
        if (decorComponent != null)
        {
            decorComponent.IncreaseAttractiveness(attractivenessIncrease);
        }
    }
}
