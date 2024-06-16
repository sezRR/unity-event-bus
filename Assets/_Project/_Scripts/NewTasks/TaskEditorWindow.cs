using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using _Project._Scripts.NewTasks;

public class TaskEditorWindow : EditorWindow
{
    private Task selectedTask;
    private Vector2 scrollPosition;
    private List<Task> allTasks = new List<Task>();

    [MenuItem("Window/Task Editor")]
    public static void ShowWindow()
    {
        GetWindow<TaskEditorWindow>("Task Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Task Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Create New Task"))
        {
            CreateNewTask();
        }

        if (GUILayout.Button("Show All Tasks"))
        {
            LoadAllTasks();
        }

        GUILayout.Space(20);

        selectedTask = (Task)EditorGUILayout.ObjectField("Selected Task", selectedTask, typeof(Task), false);

        if (selectedTask != null)
        {
            DisplayTaskDetails();
        }

        GUILayout.Space(20);

        DisplayAllTasks();
    }

    private void CreateNewTask()
    {
        Task newTask = ScriptableObject.CreateInstance<Task>();
        newTask.taskName = "New Task";
        newTask.description = "Task Description";

        string path = EditorUtility.SaveFilePanelInProject("Save New Task", "NewTask", "asset", "Please enter a file name to save the task to");
        if (path == "") return;

        AssetDatabase.CreateAsset(newTask, path);
        AssetDatabase.SaveAssets();

        selectedTask = newTask;
    }

    private void DisplayTaskDetails()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.LabelField("Task Name");
        selectedTask.taskName = EditorGUILayout.TextField(selectedTask.taskName);

        EditorGUILayout.LabelField("Description");
        selectedTask.description = EditorGUILayout.TextField(selectedTask.description);

        GUILayout.Space(10);

        DisplayTaskRequirements();

        GUILayout.Space(10);

        DisplaySubTasks();

        GUILayout.Space(20);

        if (GUILayout.Button("Save Task"))
        {
            EditorUtility.SetDirty(selectedTask);
            AssetDatabase.SaveAssets();
        }

        GUILayout.EndScrollView();
    }

    private void DisplayTaskRequirements()
    {
        EditorGUILayout.LabelField("Requirements", EditorStyles.boldLabel);

        if (selectedTask.requirements != null)
        {
            for (int i = 0; i < selectedTask.requirements.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                selectedTask.requirements[i] = (BaseTaskRequirement)EditorGUILayout.ObjectField(selectedTask.requirements[i], typeof(ITaskRequirement), false);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    ArrayUtility.RemoveAt(ref selectedTask.requirements, i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Add Requirement"))
        {
            ArrayUtility.Add(ref selectedTask.requirements, null);
        }
    }

    private void DisplaySubTasks()
    {
        EditorGUILayout.LabelField("Sub-Tasks", EditorStyles.boldLabel);

        if (selectedTask.subTasks != null)
        {
            for (int i = 0; i < selectedTask.subTasks.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                selectedTask.subTasks[i] = (Task)EditorGUILayout.ObjectField(selectedTask.subTasks[i], typeof(Task), false);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    ArrayUtility.RemoveAt(ref selectedTask.subTasks, i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Add Sub-Task"))
        {
            ArrayUtility.Add(ref selectedTask.subTasks, null);
        }
    }

    private void LoadAllTasks()
    {
        allTasks.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Task");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Task task = AssetDatabase.LoadAssetAtPath<Task>(path);
            if (task != null)
            {
                allTasks.Add(task);
            }
        }
    }

    private void DisplayAllTasks()
    {
        if (allTasks.Count > 0)
        {
            GUILayout.Label("All Tasks", EditorStyles.boldLabel);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (var task in allTasks)
            {
                if (GUILayout.Button(task.taskName))
                {
                    selectedTask = task;
                }
            }

            GUILayout.EndScrollView();
        }
    }
}
