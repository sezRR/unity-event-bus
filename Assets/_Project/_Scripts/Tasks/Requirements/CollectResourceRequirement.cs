using _Project._Scripts.Tasks.Commons.Bases;
using UnityEngine;

namespace _Project._Scripts.NewTasks
{
    [CreateAssetMenu(fileName = "NewCollectResourceRequirement", menuName = "Tasks/Requirements/Collect Resource")]
    public class CollectResourceRequirement : BaseTaskRequirement
    {
        public string resourceName;
        public int requiredAmount;
        private int currentAmount;

        public override bool IsSatisfied() => currentAmount >= requiredAmount;

        public override void RegisterEvent()
        {
            if (eventBinding != null) return;
            
            var binding = new EventBinding<ResourceCollectedEvent>(OnResourceCollected);
            Register(binding);
        }

        public override void UnregisterEvent()
        {
            Unregister<ResourceCollectedEvent>();
        }

        private void OnResourceCollected(ResourceCollectedEvent resourceEvent)
        {
            if (resourceEvent.ResourceName != resourceName) return;
            
            currentAmount += resourceEvent.Amount;
            Debug.Log($"Collected {currentAmount}/{requiredAmount} {resourceName}");
        }

        public override void ResetProgress()
        {
            currentAmount = 0;
        }
    }

    public class ResourceCollectedEvent : IEvent
    {
        public string ResourceName { get; }
        public int Amount { get; }

        public ResourceCollectedEvent(string resourceName, int amount)
        {
            ResourceName = resourceName;
            Amount = amount;
        }
    }
}