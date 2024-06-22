using System;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Elements;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static System.String;

namespace _Project._Scripts.Dialogues.Editors.GraphView
{
    [Serializable]
    public class DialogueGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        public readonly Vector2 DefaultNodeSize = new(150, 200);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        private NodeSearchWindow _searchWindow;

        public Blackboard Blackboard;
        public List<ExposedProperty> ExposedProperties = new();

        private bool _isRightMouseDragging;

        public DialogueGraphView(EditorWindow editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            var contentDragger = new ContentDragger();
            contentDragger.activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.RightMouse,
            });

            this.AddManipulator(contentDragger);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            InitializeBoundryNodes();
            RegisterPointerEventCallbacks();
            
            AddSearchWindow(editorWindow);
        }

        private void InitializeBoundryNodes()
        {
            var startNode = new StartNode().CreateNode();
            AddElement(startNode);
            
            var endNode = new EndNode().CreateNode();
            AddElement(endNode);
        }

        private void RegisterPointerEventCallbacks()
        {
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);
        }

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Init(this, editorWindow);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        
        public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null)
        {
            if(commentBlockData==null)
                commentBlockData = new CommentBlockData();
            var group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.Title
            };
            AddElement(group);
            group.SetPosition(rect);
            return group;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == (int)MouseButton.RightMouse)
                _isRightMouseDragging = false;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (evt.pressedButtons == (1 << (int)MouseButton.RightMouse))
                _isRightMouseDragging = true;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_isRightMouseDragging) return;

            base.BuildContextualMenu(evt);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(endPort =>
                    endPort.direction != startPort.direction
                    && endPort != startPort
                    && startPort.node != endPort.node)
                .ToList();
        }

        public void CreateNode(DialogueNode dialogueNode)
        {
            AddElement(dialogueNode.CreateNode());
        }

        public void InitializeRemoveFunctionalityOfPort(ButtonWithKey buttonWithKey, BaseNode baseNode, Port port)
        {
            buttonWithKey.RegisterClickEvent(() => PortHelper.RemovePortFromNode(this, baseNode, port));
        }

        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }
        
        // TODO: BLACKBOARD HELPER
        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty, bool loadMode = false)
        {
            var localPropertyName = exposedProperty.Name;
            var localPropertyValue = exposedProperty.Value;

            if (!loadMode)
            {
                while (ExposedProperties.Any(x => x.Name == localPropertyName))
                    localPropertyName = $"{localPropertyName}(1)";
            }
            
            var property = new ExposedProperty
            {
                Name = localPropertyName,
                Value = localPropertyValue
            };
            ExposedProperties.Add(property);

            var container = new VisualElement();
            var blackboardField = new BlackboardField { text = property.Name, typeText = "string property" };
            container.Add(blackboardField);

            var propertyValueTextField = new TextField("Value: ")
            {
                value = localPropertyValue,
            };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var changingPropertyIndex =
                    ExposedProperties.FindIndex(exposedPropertyItem => exposedPropertyItem.Name == property.Name);
                ExposedProperties[changingPropertyIndex].Value = evt.newValue;
            });

            var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
            container.Add(blackBoardValueRow);

            Blackboard.Add(container);
        }
    }
}