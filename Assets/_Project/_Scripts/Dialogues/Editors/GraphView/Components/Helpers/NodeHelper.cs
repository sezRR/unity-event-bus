using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class NodeHelper
    {
        public static BoundryNode GenerateBoundryNode<T>(
            T boundryNode,
            PortConfiguration portConfiguration,
            Vector2 size)
            where T : BoundryNode
        {
            PortHelper.GeneratePort(boundryNode, portConfiguration);
            NodeCapabilityHelper.RemoveCapabilityFromNode(boundryNode, Capabilities.Deletable);
            ElementsHelper.SetPositionAndSizeOfElement(
                boundryNode,
                new Vector2(
                    boundryNode.Position.x,
                    boundryNode.Position.y
                ),
                size);

            return boundryNode;
        }

        public static DialogueNode GenerateDialogueNode(
            DialogueNode dialogueNode,
            Vector2 position,
            PortConfiguration inputPortConfiguration,
            Vector2 size)
        {
            PortHelper.GeneratePort(dialogueNode, inputPortConfiguration, false);
            StyleSheetHelper.AddStyleSheetToVisualElementFromResources(dialogueNode, "Node");

            ElementsHelper.AddNewChoiceButtonToNode(dialogueNode);
            ElementsHelper.AddStoryTextFieldToNode(dialogueNode);
            ElementsHelper.SetPositionAndSizeOfElement(dialogueNode, position, size);
            ElementsHelper.AddResizableElementToNode(dialogueNode);

            RefreshNode(dialogueNode);
            return dialogueNode;
        }

        public static void RefreshNode(BaseNode baseNode)
        {
            baseNode.RefreshExpandedState();
            baseNode.RefreshPorts();
        }
    }
}