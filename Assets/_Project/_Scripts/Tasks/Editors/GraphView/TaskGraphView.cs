using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project._Scripts.Tasks.Editors.GraphView
{
    public class TaskGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        public TaskGraphView()
        {
            // Add the default capabilities to the GraphView
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // Set up the style
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            // Define some styles for the nodes and edges
            styleSheets.Add(Resources.Load<StyleSheet>("TaskGraphViewStyle"));

            // Listen for graph changes
            graphViewChanged = OnGraphViewChanged;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // Add context menu options for creating tasks
            evt.menu.AppendAction("Create Task", (e) => AddElement(CreateTaskNode()));
        }

        private TaskNode CreateTaskNode()
        {
            var node = new TaskNode
            {
                title = "Task",
                style = { top = 100, left = 100 }
            };

            var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Single);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);

            var outputPort = GeneratePort(node, Direction.Output, Port.Capacity.Single);
            outputPort.portName = "Output";
            node.outputContainer.Add(outputPort);

            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(100, 100, 150, 200));

            return node;
        }

        private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }
    
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()!.Where(endPort =>
                    endPort.direction != startPort.direction &&
                    endPort.node != startPort.node &&
                    endPort.portType == startPort.portType)
                .ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var outputNode = (TaskNode)edge.output.node;
                    var inputNode = (TaskNode)edge.input.node;

                    outputNode.NextTaskNode = inputNode;
                }
            }

            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    var outputNode = (TaskNode)edge.output.node;
                    if (outputNode != null)
                    {
                        outputNode.NextTaskNode = null;
                    }
                }
            }

            return graphViewChange;
        }
    }



    public class TaskNode : Node
    {
        public string GUID;
        public TaskNode NextTaskNode;
        public TaskNode()
        {
            GUID = System.Guid.NewGuid().ToString();
        }
    }
}