﻿using System;

public interface IEventBinding<T>
{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    private Action<T> _onEvent = _ => { };
    private Action _onEventNoArgs = () => { };

    public Action<T> OnEvent
    {
        get => _onEvent;
        set => _onEvent = value;
    }

    public Action OnEventNoArgs
    {
        get => _onEventNoArgs;
        set => _onEventNoArgs = value;
    }

    public EventBinding(Action<T> onEvent) => this._onEvent = onEvent;

    public EventBinding(Action onEventNoArgs) => this._onEventNoArgs = onEventNoArgs;

    public void Add(Action<T> onEvent) => this._onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this._onEvent += onEvent;
    
    public void Add(Action onEventNoArgs) => onEventNoArgs += onEventNoArgs;
    public void Remove(Action onEventNoArgs) => onEventNoArgs += onEventNoArgs;
}