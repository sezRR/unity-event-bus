using _Project._Scripts.NewTasks;
using _Project._Scripts.Tasks.Commons.Bases;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTask))]
public class TaskEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BaseTask task = (BaseTask)target;

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
                EditorGUILayout.ObjectField(subTask, typeof(BaseTask), false);
            }
        }

        if (GUILayout.Button("Add Sub-Task"))
        {
            ArrayUtility.Add(ref task.subTasks, CreateInstance<BaseTask>());
        }
        
        if (GUILayout.Button("Reset Progress"))
        {
            task.ResetProgress();
            Debug.Log($"Task '{task.taskName}' progress has been reset.");
        }
    }
}