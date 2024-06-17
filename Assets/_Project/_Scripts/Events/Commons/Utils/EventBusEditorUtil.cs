using _Project._Scripts.Events.Commons.Interfaces;
using _Project._Scripts.Tasks.Commons.Bases;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public static class EventBusEditorUtil
{
    static EventBusEditorUtil()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        RegisterAllTaskEvents();
        switch (state)
        {
            case PlayModeStateChange.EnteredPlayMode:
                RegisterAllTaskEvents();
                RefreshTaskEditorWindow();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                EventBusUtil.ClearAllBusses();
                break;
        }
    }

    private static void RegisterAllTaskEvents()
    {
        RegisterAllEventsOf<BaseTask>();
    }
    
    private static void RegisterAllEventsOf<T>() where T: ScriptableObject, IEventRegistrar
    {
        T[] tasks = Resources.FindObjectsOfTypeAll<T>();
        foreach (var task in tasks)
        {
            task.RegisterEvents();
        }
    }

    private static void RefreshTaskEditorWindow()
    {
        var window = EditorWindow.GetWindow<TaskEditorWindow>();

        if (window == null)
        {
            window.RefreshAllTasks();
            window.Repaint();
        }
    }
}
#endif