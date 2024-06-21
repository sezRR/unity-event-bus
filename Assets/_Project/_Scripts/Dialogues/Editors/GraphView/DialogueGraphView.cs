using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static System.String;

namespace _Project._Scripts.Dialogues.Editors.GraphView
{
    public class DialogueGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        public readonly Vector2 DefaultNodeSize = new(150, 200);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        private NodeSearchWindow _searchWindow;

        public Blackboard Blackboard;
        private EditorWindow _editorWindow;
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

            AddElement(GenerateEntryPointNode());

            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);

            _editorWindow = editorWindow;
            AddSearchWindow(editorWindow);
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

            // var mousePosition = contentViewContainer.LocalToWorld(evt.mousePosition + _editorWindow.position.position + new Vector2(125, 0));
            // nodeCreationRequest(new NodeCreationContext()
            // {
            //     screenMousePosition = mousePosition,
            //     target = null,
            //     index = -1
            // });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(endPort =>
                    endPort.direction != startPort.direction
                    && endPort != startPort
                    && startPort.node != endPort.node)
                // && !endPort.connections.ToList().Exists(connection => connection.output.node == startPort.node))
                .ToList();
        }

        // Port Direction -> is it input or output
        private Port GeneratePort(DialogueNode node, Direction portDirection,
            Port.Capacity capacity = Port.Capacity.Single, Orientation orientation = Orientation.Horizontal)
        {
            return node.InstantiatePort(orientation, portDirection, capacity, typeof(float)); // Arbitrary type
        }

        private DialogueNode GenerateEntryPointNode()
        {
            var node = new DialogueNode()
            {
                title = "START",
                Guid = Guid.NewGuid().ToString(),
                DialogueText = "Sample Dialogue",
                EntryPoint = true
            };

            var generatedPort = GeneratePort(node, Direction.Output);
            generatedPort.portName = "Next";
            node.outputContainer.Add(generatedPort);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(100, 200, 100, 150));
            return node;
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            AddElement(CreateDialogueNode(nodeName, position));
        }

        public DialogueNode CreateDialogueNode(string nodeName, Vector2 position)
        {
            var dialogueNode = new DialogueNode
            {
                title = nodeName,
                DialogueText = nodeName,
                Guid = Guid.NewGuid().ToString()
            };

            var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            dialogueNode.inputContainer.Add(inputPort);

            dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            var button = new Button(() => { AddChoicePort(dialogueNode); });

            button.text = "New Choice";
            dialogueNode.titleContainer.Add(button);

            var textField = new TextField(Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                dialogueNode.DialogueText = evt.newValue;
                dialogueNode.title = evt.newValue;
            });

            textField.SetValueWithoutNotify(dialogueNode.title);
            dialogueNode.mainContainer.Add(textField);

            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
            dialogueNode.SetPosition(new Rect(position, DefaultNodeSize));

            // Add initial choice port TODO
            // AddChoicePort(dialogueNode);

            // TO RE-SIZE
            dialogueNode.style.minWidth = 240;
            // dialogueNode.style.maxWidth = 360;
            // dialogueNode.capabilities |= Capabilities.Resizable; 
            var a = new ResizableElement();
            dialogueNode.contentContainer.Add(a);
            //---

            return dialogueNode;
        }

        public void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
        {
            var generatedPort = GeneratePort(dialogueNode, Direction.Output);

            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            oldLabel.style.display = DisplayStyle.None;
            // generatedPort.contentContainer.Remove(oldLabel);

            var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
            var choicePortName = IsNullOrEmpty(overridenPortName)
                ? $"Choice {outputPortCount}"
                : overridenPortName;

            var textField = new TextField()
            {
                name = Empty,
                value = choicePortName
            };

            textField.style.minWidth = 60;
            textField.style.maxWidth = 100;

            // TODO: a
            Debug.Log("AAAAA");

            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("   "));
            generatedPort.contentContainer.Add(textField);

            var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
            {
                text = "-"
            };
            generatedPort.contentContainer.Add(deleteButton);

            generatedPort.portName = choicePortName;

            dialogueNode.outputContainer.Add(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
        }

        private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
        {
            var targetedEdge = edges.ToList().Where(edge =>
                edge.output.portName == generatedPort.portName && edge.output.node == generatedPort.node);

            if (targetedEdge.Any())
            {
                var edge = targetedEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetedEdge.First());
            }

            ;

            dialogueNode.outputContainer.Remove(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
        }

        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }
        
        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty, bool loadMode = false)
        {
            var localPropertyName = exposedProperty.Name;
            var localPropertyValue = exposedProperty.Value;

            if (!loadMode)
            {
                while (ExposedProperties.Any(x => x.Name == localPropertyName))
                    localPropertyName = $"{localPropertyName}(1)";
            }
            
            var property = new ExposedProperty();
            property.Name = localPropertyName;
            property.Value = localPropertyValue;
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