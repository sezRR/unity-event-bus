using System;
using System.Linq;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Elements;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class PortHelper
    {
        public static Port GeneratePort(BaseNode node, PortConfiguration portConfiguration, bool refreshNodeAfterProcess = true)
        {
            var generatedPort = node.InstantiatePort(
                portConfiguration.Orientation,
                portConfiguration.Direction,
                portConfiguration.Capacity,
                portConfiguration.Type
            );

            generatedPort.portName = portConfiguration.Name;

            AddPortToNode(node, generatedPort, portConfiguration.Direction);

            if (refreshNodeAfterProcess)
            {
                NodeHelper.RefreshNode(node);
            }

            return generatedPort;
        }

        public static void RemovePortFromNode(UnityEditor.Experimental.GraphView.GraphView targetGraphView, BaseNode baseNode, Port generatedPort)
        {
            var targetedEdges = targetGraphView.edges
                .Where(edge => edge.output.portName == generatedPort.portName && edge.output.node == generatedPort.node)
                .ToList();

            if (targetedEdges.Any())
            {
                var edge = targetedEdges.First();
                edge.input.Disconnect(edge);

                targetGraphView.RemoveElement(edge);
            }

            baseNode.outputContainer.Remove(generatedPort);
            NodeHelper.RefreshNode(baseNode);
        }

        public static void AddChoicePortToNode(BaseNode dialogueNode, string overriddenPortName = "")
        {
            // BUG:
            // New Choice -> 1, 2, 3, 4
            // Remove Choice -> 1, 2, 3
            // New Choice, pressed 3 times
            // New Choices -> 4, 3, 2, 1
            var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
            var choicePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Choice {outputPortCount}" : overriddenPortName;

            var choicePortConfiguration = new PortConfiguration
            {
                Name = choicePortName,
                Direction = Direction.Output,
                Capacity = Port.Capacity.Single
            };

            var generatedPort = GeneratePort(dialogueNode, choicePortConfiguration, false);
            HidePortLabel(generatedPort);
            AddTextFieldToPort(generatedPort, choicePortName);
            AddDeleteButtonToPort(dialogueNode, generatedPort);

            NodeHelper.RefreshNode(dialogueNode);
        }

        private static void AddPortToNode(BaseNode node, Port port, Direction direction)
        {
            switch (direction)
            {
                case Direction.Input:
                    node.inputContainer.Add(port);
                    break;
                case Direction.Output:
                    node.outputContainer.Add(port);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void HidePortLabel(Port port)
        {
            var oldLabel = port.contentContainer.Q<Label>("type");
            oldLabel.style.display = DisplayStyle.None;
        }

        private static void AddTextFieldToPort(Port port, string portName)
        {
            var textField = new TextField
            {
                name = string.Empty,
                value = portName,
                style =
                {
                    minWidth = 60,
                    maxWidth = 100
                }
            };
            textField.RegisterValueChangedCallback(evt => port.portName = evt.newValue);

            port.contentContainer.Add(new Label("   "));
            port.contentContainer.Add(textField);
        }

        private static void AddDeleteButtonToPort(BaseNode node, Port port)
        {
            var dialogueGraph = (DialogueGraph)DialogueGraph.GetWindow(typeof(DialogueGraph));
            var deleteButton = new ButtonWithKey(port.portName, "-", () => RemovePortFromNode(dialogueGraph.GraphView, node, port));
            port.contentContainer.Add(deleteButton);
        }
    }
}
