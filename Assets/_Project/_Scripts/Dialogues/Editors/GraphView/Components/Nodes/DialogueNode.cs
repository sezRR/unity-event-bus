using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes
{
    public class DialogueNode : BaseNode
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public DialogueNode(string name, string text)
        {
            Name = name;
            Text = text;
        }
        
        public DialogueNode(string text)
        {
            Text = text;
        }

        public DialogueNode()
        {
            
        }
        
        public override BaseNode CreateNode()
        {
            var dialogueNode = new DialogueNode
            {
                title = Text,
                Text = Text
            };
            
            var inputPortConfiguration = new PortConfiguration
            {
                Name = "Input",
                Direction = Direction.Input,
                Capacity = Port.Capacity.Multi
            };
            
            return NodeHelper.GenerateDialogueNode(dialogueNode, Position, inputPortConfiguration, Size);
        }
    }
}