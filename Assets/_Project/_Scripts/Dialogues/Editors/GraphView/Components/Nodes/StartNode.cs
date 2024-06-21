using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes
{
    public class StartNode : BoundryNode
    {
        public override string title { get; set; } = "START";

        public override BaseNode CreateNode()
        {
            var portConfiguration = new PortConfiguration
            {
                Name = "Next",
                Direction = Direction.Output,
                Capacity = Port.Capacity.Single
            };

            return NodeHelper.GenerateBoundryNode(this, portConfiguration);
        }
    }
}