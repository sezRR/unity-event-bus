using System.Collections.Generic;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using UnityEditor.Experimental.GraphView;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class NodeCapabilityHelper
    {
        public static void AddCapabilitiesToNode(BaseNode baseNode, List<Capabilities> capabilities)
        {
            foreach (var capability in capabilities)
                AddCapabilityToNode(baseNode, capability);
        }

        public static void AddCapabilityToNode(BaseNode baseNode, Capabilities capability)
        {
            baseNode.capabilities |= capability;
        }

        public static void RemoveCapabilitiesFromNode(BaseNode baseNode, List<Capabilities> capabilities)
        {
            foreach (var capability in capabilities)
                RemoveCapabilityFromNode(baseNode, capability);
        }

        public static void RemoveCapabilityFromNode(BaseNode baseNode, Capabilities capability)
        {
            baseNode.capabilities &= ~capability;
        }
    }
}