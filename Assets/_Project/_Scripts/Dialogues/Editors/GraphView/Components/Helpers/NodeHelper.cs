using System;
using System.Collections.Generic;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static System.String;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class NodeHelper
    {
        public static BoundryNode GenerateBoundryNode<T>(
            T boundryNode,
            PortConfiguration portConfiguration)
            where T : BoundryNode
        {
            PortHelper.GeneratePort(boundryNode, portConfiguration);
            NodeCapabilityHelper.RemoveCapabilityFromNode(boundryNode, Capabilities.Deletable);

            // TODO: SIZE
            boundryNode.SetPosition(new Rect(200, 200, 100, 150));

            return boundryNode;
        }

        public static DialogueNode GenerateDialogueNode(
            DialogueNode dialogueNode,
            Vector2 position,
            PortConfiguration portConfiguration)
        {
            PortHelper.GeneratePort(dialogueNode, portConfiguration, false);
            StyleSheetHelper.AddStyleSheetToVisualElementFromResources(dialogueNode, "Node");
            AddButtonToNode(dialogueNode);
            AddTextFieldToNode(dialogueNode);
            RefreshNode(dialogueNode);
            
            // TODO: SIZE and
            // TODO: AUTO CONNECT TO END
            dialogueNode.SetPosition(new Rect(position, new(150, 200)));

            AddResizableElementToNode(dialogueNode);
            
            return dialogueNode;
        }
        
        private static void AddButtonToNode(DialogueNode dialogueNode)
        {
            var button = new Button(() => { AddChoicePort(dialogueNode); });
            button.text = "New Choice";
            dialogueNode.titleContainer.Add(button);
        }

        // TODO: OPTIONS
        private static void AddTextFieldToNode(DialogueNode dialogueNode)
        {
            var textField = new TextField(Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                dialogueNode.Text = evt.newValue;
                dialogueNode.title = evt.newValue;
            });

            textField.SetValueWithoutNotify(dialogueNode.title);
            dialogueNode.mainContainer.Add(textField);
        }

        private static void AddResizableElementToNode(BaseNode baseNode)
        {
            baseNode.style.minWidth = 240;
            baseNode.contentContainer.Add(new ResizableElement());
        }

        public static void RefreshNode(BaseNode baseNode)
        {
            baseNode.RefreshExpandedState();
            baseNode.RefreshPorts();
        }
        
        public static void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
        {
            var choicePortConfiguration = new PortConfiguration
            {
                Name = "choice",
                Direction = Direction.Output,
                Capacity = Port.Capacity.Single
            };
            var generatedPort = PortHelper.GeneratePort(dialogueNode, choicePortConfiguration, false);
            
            
            
            
            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            oldLabel.style.display = DisplayStyle.None;


            
            
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
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            
            
            generatedPort.contentContainer.Add(new Label("   "));
            generatedPort.contentContainer.Add(textField);
            var deleteButton = new Button(() => PortHelper.RemovePort(dialogueNode, generatedPort))
            {
                text = "-"
            };
            generatedPort.contentContainer.Add(deleteButton);
            generatedPort.portName = choicePortName;
            dialogueNode.outputContainer.Add(generatedPort);
            
            
            
            RefreshNode(dialogueNode);
        }
    }
}