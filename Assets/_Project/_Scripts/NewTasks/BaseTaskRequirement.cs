using UnityEngine;

namespace _Project._Scripts.NewTasks
{
    public abstract class BaseTaskRequirement : ScriptableObject, ITaskRequirement
    {
        protected object eventBinding;

        public abstract bool IsSatisfied();

        public abstract void RegisterEvent();

        public abstract void UnregisterEvent();

        protected void Register<T>(EventBinding<T> binding) where T : IEvent
        {
            eventBinding = binding;
            EventBus<T>.Register(binding);
        }

        protected void Unregister<T>() where T : IEvent
        {
            if (eventBinding is not EventBinding<T> binding) return;
            
            EventBus<T>.DeRegister(binding);
            eventBinding = null;
        }
    }
}