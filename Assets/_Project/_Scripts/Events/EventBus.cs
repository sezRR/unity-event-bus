using System.Collections.Generic;
using UnityEngine;

// SHOULD BE NEW INTERFACE FOR T, IBaseEvent? IBaseEventGroup?
public static class EventBus<T> where T : IEvent
{
    static readonly HashSet<IEventBinding<T>> _bindings = new();

    public static void Register(EventBinding<T> binding) => _bindings.Add(binding);
    public static void DeRegister(EventBinding<T> binding) => _bindings.Remove(binding);

    public static void Raise(T @event)
    {
        foreach (var binding in _bindings)
        {
            Debug.Log($"Bindings length: {_bindings.Count}");
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }

    static void Clear()
    {
        Debug.Log($"Clearing {typeof(T).Name} bindings.");
        _bindings.Clear();
    }
}