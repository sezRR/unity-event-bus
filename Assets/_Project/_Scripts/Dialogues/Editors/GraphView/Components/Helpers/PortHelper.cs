using System;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using _Project._Scripts.Dialogues.Editors.GraphView.Utilities;
using UnityEditor.Experimental.GraphView;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class PortHelper
    {
        public static Port GeneratePort(BaseNode node, PortConfiguration portConfiguration,
            bool refreshNodeAfterProcess = true)
        {
            var generatedPort = node.InstantiatePort(
                portConfiguration.Orientation,
                portConfiguration.Direction,
                portConfiguration.Capacity,
                portConfiguration.Type
            );

            generatedPort.portName = portConfiguration.Name;

            switch (portConfiguration.Direction)
            {
                case Direction.Input:
                    node.inputContainer.Add(generatedPort);
                    break;
                case Direction.Output:
                    node.outputContainer.Add(generatedPort);
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (refreshNodeAfterProcess)
                NodeHelper.RefreshNode(node);

            return generatedPort;
        }

        public static void RemovePortFromNode(
            UnityEditor.Experimental.GraphView.GraphView targetGraphView,
            List<Edge> edges,
            BaseNode baseNode,
            Port generatedPort)
        {
            var targetedEdge = edges
                .Where(edge =>
                    edge.output.portName == generatedPort.portName
                    && edge.output.node == generatedPort.node)
                .ToList(); // TODO: ADDED TO LIST

            if (targetedEdge.Any())
            {
                var edge = targetedEdge.First();
                edge.input.Disconnect(edge);

                targetGraphView.RemoveElement(targetedEdge.First());
            }

            baseNode.outputContainer.Remove(generatedPort);

            NodeHelper.RefreshNode(baseNode);
        }
    }
}