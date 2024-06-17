using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using _Project._Scripts.Tasks.Commons.Bases;

public class TaskEditorWindow : EditorWindow
{
    private BaseTask selectedTask;
    private Vector2 scrollPosition;
    private List<BaseTask> allTasks = new();
    private List<BaseTask> tasksToDelete = new();
    private Texture2D windowIcon;
    private const string IconPath = "Assets/Editor/Icons/TaskEditorIcon.png";

    [MenuItem("Tools/Editors/Task Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<TaskEditorWindow>("Task Editor", typeof(SceneView));
        window.LoadIcon();
        window.RefreshAllTasks();  // Refresh task list when the window is opened
    }

    private void LoadIcon()
    {
        windowIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
        this.titleContent = new GUIContent("Task Editor", windowIcon);
    }

    private void OnGUI()
    {
        GUILayout.Label("Task Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Reset All Progress"))
        {
            if (EditorUtility.DisplayDialog("Reset All Progress", "Are you sure you want to reset progress for all tasks?", "Yes", "No"))
            {
                ResetAllTasksProgress();
            }
        }

        if (GUILayout.Button("Create New Task"))
        {
            CreateNewTask();
        }

        if (GUILayout.Button("Refresh All Tasks"))
        {
            RefreshAllTasks();
        }

        GUILayout.Space(20);

        selectedTask = (BaseTask)EditorGUILayout.ObjectField("Selected Task", selectedTask, typeof(BaseTask), false);

        if (selectedTask != null)
        {
            DisplayTaskDetails();
        }

        GUILayout.Space(20);

        DisplayAllTasks();
    }

    private void CreateNewTask()
    {
        BaseTask newTask = ScriptableObject.CreateInstance<BaseTask>();
        newTask.taskName = "New Task";
        newTask.description = "Task Description";

        string path = EditorUtility.SaveFilePanelInProject("Save New Task", "NewTask", "asset", "Please enter a file name to save the task to");
        if (path == "") return;

        AssetDatabase.CreateAsset(newTask, path);
        AssetDatabase.SaveAssets();

        selectedTask = newTask;
        RefreshAllTasks();
    }

    private void DisplayTaskDetails()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.LabelField("Task Name");
        selectedTask.taskName = EditorGUILayout.TextField(selectedTask.taskName);

        EditorGUILayout.LabelField("Description");
        selectedTask.description = EditorGUILayout.TextField(selectedTask.description);

        GUILayout.Label($"Status: {selectedTask.Status}");

        GUILayout.Space(10);

        DisplayTaskRequirements();

        GUILayout.Space(10);

        DisplaySubTasks();

        GUILayout.Space(20);

        if (GUILayout.Button("Update Task"))
        {
            EditorUtility.SetDirty(selectedTask);
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Delete Task"))
        {
            string path = AssetDatabase.GetAssetPath(selectedTask);
            AssetDatabase.DeleteAsset(path);
            selectedTask = null;
            RefreshAllTasks();
        }

        if (GUILayout.Button("Reset Progress"))
        {
            selectedTask.ResetProgress();
            Debug.Log($"Task '{selectedTask.taskName}' progress has been reset.");
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Check Task Completion"))
        {
            Debug.Log($"Task '{selectedTask.taskName}' completion status: {selectedTask.IsCompleted}");
        }

        GUILayout.EndScrollView();
    }

    private void DisplayTaskRequirements()
    {
        EditorGUILayout.LabelField("Requirements", EditorStyles.boldLabel);

        if (selectedTask.requirements == null)
        {
            selectedTask.requirements = new BaseTaskRequirement[0];
        }

        for (int i = 0; i < selectedTask.requirements.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            selectedTask.requirements[i] = (BaseTaskRequirement)EditorGUILayout.ObjectField(selectedTask.requirements[i], typeof(BaseTaskRequirement), false);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                ArrayUtility.RemoveAt(ref selectedTask.requirements, i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Requirement"))
        {
            ArrayUtility.Add(ref selectedTask.requirements, null);
        }
    }

    private void DisplaySubTasks()
    {
        EditorGUILayout.LabelField("Sub-Tasks", EditorStyles.boldLabel);

        if (selectedTask.subTasks == null)
        {
            selectedTask.subTasks = new BaseTask[0];
        }

        for (int i = 0; i < selectedTask.subTasks.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            selectedTask.subTasks[i] = (BaseTask)EditorGUILayout.ObjectField(selectedTask.subTasks[i], typeof(BaseTask), false);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                ArrayUtility.RemoveAt(ref selectedTask.subTasks, i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Sub-Task"))
        {
            ArrayUtility.Add(ref selectedTask.subTasks, null);
        }
    }

    public void RefreshAllTasks()
    {
        allTasks.Clear();
        string[] guids = AssetDatabase.FindAssets("t:BaseTask");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            BaseTask task = AssetDatabase.LoadAssetAtPath<BaseTask>(path);
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
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(task.taskName))
                {
                    selectedTask = task;
                }
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    tasksToDelete.Add(task);
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            if (tasksToDelete.Count > 0)
            {
                foreach (var task in tasksToDelete)
                {
                    string path = AssetDatabase.GetAssetPath(task);
                    AssetDatabase.DeleteAsset(path);
                }
                tasksToDelete.Clear();
                RefreshAllTasks();
            }
        }
    }

    private void ResetAllTasksProgress()
    {
        foreach (var task in allTasks)
        {
            task.ResetProgress();
        }

        Debug.Log("All tasks progress has been reset.");
    }
}
