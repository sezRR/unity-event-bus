using _Project._Scripts.Tasks.Commons.Bases;
using UnityEngine;

namespace _Project._Scripts.NewTasks
{
    [CreateAssetMenu(fileName = "NewUpgradeRequirement", menuName = "Tasks/Requirements/Upgrade")]
    public class UpgradeRequirement : BaseTaskRequirement
    {
        public UpgradeBase requiredUpgrade;
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
            if (upgradeEvent.Upgrade.id == requiredUpgrade.id)
            {
                isPurchased = true;
                Debug.Log($"Upgrade '{requiredUpgrade.upgradeName}' purchased!");
            }
        }

        public override void ResetProgress()
        {
            isPurchased = false;
        }
    }

    public class UpgradeEvent : IEvent
    {
        public UpgradeBase Upgrade { get; }

        public UpgradeEvent(UpgradeBase upgrade)
        {
            Upgrade = upgrade;
        }
    }
}