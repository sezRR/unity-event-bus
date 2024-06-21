using System.Linq;
using _Project._Scripts.Dialogues.Editors.GraphView;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";

    [MenuItem("Tools/Editors/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        GenerateMiniMap();
        GenerateBlackBoard();
    }

    private void GenerateBlackBoard()
    {
        var blackboard = new Blackboard(_graphView);
        blackboard.Add(new BlackboardSection
        {
            title = "Exposed Properties"
        });

        blackboard.addItemRequested = _blackboard => { _graphView.AddPropertyToBlackBoard(new ExposedProperty()); };
        blackboard.editTextRequested = (blackboard, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposedProperties.Any(x => x.Name == newValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already exists, please choose another!", "OK");
                return;
            }

            var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.Name == oldPropertyName);
            _graphView.ExposedProperties[propertyIndex].Name = newValue;
            ((BlackboardField) element).text = newValue;
        };
        
        blackboard.SetPosition(new Rect(10, 30, 200, 300));

        _graphView.Blackboard = blackboard;
        _graphView.Add(blackboard);
    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap
        {
            anchored = true,
            maxHeight = 140
        };

        var rightPosition = new Vector2(this.position.xMax - position.xMin - 200 - 10, 30);

        miniMap.SetPosition(new Rect(rightPosition.x, rightPosition.y, 200, 140));
        _graphView.Add(miniMap);
    }

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name: ");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback((evt) => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        // var nodeCreateButton = new Button(() =>
        // {
        //     _graphView.CreateNode("Dialogue Node");
        // });
        // nodeCreateButton.text = "Create Node";
        // toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save)
            saveUtility.SaveGraph(_fileName);
        else
            saveUtility.LoadGraph(_fileName);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new(this)
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }
}