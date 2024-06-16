using UnityEngine;

namespace _Project._Scripts.NewTasks
{
    [CreateAssetMenu(fileName = "NewUpgradeRequirement", menuName = "Tasks/Requirements/Upgrade")]
    public class UpgradeRequirement : BaseTaskRequirement
    {
        public Upgrade requiredUpgrade;
        private bool isPurchased;

        public override bool IsSatisfied() => isPurchased;

        public override void RegisterEvent()
        {
            if (eventBinding != null) return;
            
            var binding = new EventBinding<UpgradeEvent>(OnUpgradePurchased);
            Register(binding);
        }

        public override void UnregisterEvent()
        {
            Unregister<UpgradeEvent>();
        }

        private void OnUpgradePurchased(UpgradeEvent upgradeEvent)
        {
            if (upgradeEvent.Upgrade != requiredUpgrade) return;
            
            isPurchased = true;
            Debug.Log($"Upgrade '{requiredUpgrade.upgradeName}' purchased!");
        }
    }

    public class UpgradeEvent : IEvent
    {
        public Upgrade Upgrade { get; }

        public UpgradeEvent(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }
}