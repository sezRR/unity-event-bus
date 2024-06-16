using _Project._Scripts.NewTasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Task))]
public class TaskEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Task task = (Task)target;

        if (GUILayout.Button("Register Task Events"))
        {
            task.RegisterEvents();
        }

        if (GUILayout.Button("Unregister Task Events"))
        {
            task.UnregisterEvents();
        }

        if (GUILayout.Button("Check Task Completion"))
        {
            Debug.Log($"Task '{task.taskName}' completion status: {task.IsCompleted}");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sub-Tasks", EditorStyles.boldLabel);

        if (task.subTasks != null)
        {
            foreach (var subTask in task.subTasks)
            {
                EditorGUILayout.ObjectField(subTask, typeof(Task), false);
            }
        }

        if (GUILayout.Button("Add Sub-Task"))
        {
            ArrayUtility.Add(ref task.subTasks, CreateInstance<Task>());
        }
    }
}