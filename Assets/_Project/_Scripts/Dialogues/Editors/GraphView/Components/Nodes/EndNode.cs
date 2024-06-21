using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes
{
    public class EndNode : BoundryNode
    {
        public override string title { get; set; } = "END";
        
        public override BaseNode CreateNode()
        {
            var portConfiguration = new PortConfiguration
            {
                Name = "Final",
                Direction = Direction.Input,
                Capacity = Port.Capacity.Multi
            };

            return NodeHelper.GenerateBoundryNode(this, portConfiguration);
        }
    }
}