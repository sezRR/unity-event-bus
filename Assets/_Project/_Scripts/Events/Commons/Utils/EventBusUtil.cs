using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EventBusUtil
{
    public static IReadOnlyList<Type> EventTypes { get; set; }
    public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
    public static PlayModeStateChange PlayModeState { get; set; }

    [InitializeOnLoadMethod]
    public static void InitializeEditor()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        PlayModeState = state;
        if (state == PlayModeStateChange.ExitingPlayMode)
            ClearAllBusses();
    }
#endif 
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
        EventBusTypes = InitializeAllBusses();
    }

    static List<Type> InitializeAllBusses()
    {
        List<Type> eventBusTypes = new();

        var typeDef = typeof(EventBus<>);
        foreach (var eventType in EventTypes)
        {
            var busType = typeDef.MakeGenericType(eventType);
            eventBusTypes.Add(busType);
            Debug.Log($"Initialized EventBus<{eventType.Name}>");
        }

        return eventBusTypes;
    }

    public static void ClearAllBusses()
    {
        Debug.Log("Clearing all busses...");
        for (int i = 0; i < EventTypes.Count; i++)
        {
            var busType = EventBusTypes[i];
            var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
            clearMethod.Invoke(null, null);
        }
    }
}