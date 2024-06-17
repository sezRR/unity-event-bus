using _Project._Scripts.NewTasks;
using UnityEngine;

[CreateAssetMenu(fileName = "WallArtUpgrade", menuName = "Upgrades/WallArtUpgrade")]
public class WallArtUpgrade : DecorationUpgradeBase
{
    public override void RaiseUpgradeEvent()
    {
        EventBus<UpgradeEvent>.Raise(new UpgradeEvent(this));
    }
}
