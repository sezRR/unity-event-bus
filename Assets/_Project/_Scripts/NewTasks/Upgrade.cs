namespace _Project._Scripts.NewTasks
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public string upgradeName;
        public string description;

        public void PurchaseUpgrade()
        {
            Debug.Log($"Upgrade '{upgradeName}' purchased!");
            EventBus<UpgradeEvent>.Raise(new UpgradeEvent(this));
        }
    }

}