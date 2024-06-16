using _Project._Scripts.NewTasks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EventBusEditorUtil
{
    static EventBusEditorUtil()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            RegisterAllTaskEvents();
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            EventBusUtil.ClearAllBusses();
        }
    }

    private static void RegisterAllTaskEvents()
    {
        Task[] tasks = Resources.FindObjectsOfTypeAll<Task>();
        foreach (var task in tasks)
        {
            task.RegisterEvents();
        }
    }
}