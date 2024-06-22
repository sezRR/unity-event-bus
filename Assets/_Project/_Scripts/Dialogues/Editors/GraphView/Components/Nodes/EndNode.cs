using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes
{
    public class EndNode : BoundryNode
    {
        public override Vector2 Position { get; set; } = new(750, 200);
        
        public EndNode()
        {
            title = "END";
        }
        
        public override BaseNode CreateNode()
        {
            var portConfiguration = new PortConfiguration
            {
                Name = "Final",
                Direction = Direction.Input,
                Capacity = Port.Capacity.Multi
            };

            return NodeHelper.GenerateBoundryNode(this, portConfiguration, Size);
        }
    }
}