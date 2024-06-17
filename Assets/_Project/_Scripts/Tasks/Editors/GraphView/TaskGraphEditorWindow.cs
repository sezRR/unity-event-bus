using _Project._Scripts.Tasks.Editors.GraphView;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskGraphEditorWindow : EditorWindow
{
    private TaskGraphView _graphView;

    [MenuItem("Tools/Editors/Task Graph")]
    public static void OpenTaskGraphWindow()
    {
        var window = GetWindow<TaskGraphEditorWindow>();
        window.titleContent = new GUIContent("Task Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new TaskGraphView
        {
            name = "Task Graph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var saveButton = new Button(() => { SaveData(); })
        {
            text = "Save"
        };
        toolbar.Add(saveButton);

        var loadButton = new Button(() => { LoadData(); })
        {
            text = "Load"
        };
        toolbar.Add(loadButton);

        rootVisualElement.Add(toolbar);
    }

    private void SaveData()
    {
        // Implement saving logic
    }

    private void LoadData()
    {
        // Implement loading logic
    }
}