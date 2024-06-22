using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static System.String;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class ElementsHelper
    {
        public static void SetPositionAndSizeOfElement(GraphElement element, Vector2 newPosition, Vector2 newSize)
        {
            element.SetPosition(new Rect(newPosition, newSize));
        }
        
        public static void AddNewChoiceButtonToNode(DialogueNode dialogueNode)
        {
            var button = new Button(() => { PortHelper.AddChoicePortToNode(dialogueNode); });
            button.text = "New Choice";
            dialogueNode.titleContainer.Add(button);
        }

        // TODO: OPTIONS
        public static void AddStoryTextFieldToNode(DialogueNode dialogueNode)
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

        public static void AddResizableElementToNode(BaseNode baseNode)
        {
            baseNode.style.minWidth = 240;
            baseNode.contentContainer.Add(new ResizableElement());
        }
    }
}