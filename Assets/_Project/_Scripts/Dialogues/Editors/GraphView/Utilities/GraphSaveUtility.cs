using System;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Dialogues.Editors.GraphView.Components.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Utilities
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _container;

        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

        private List<Group> CommentBlocks =>
            _targetGraphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            if (!SaveNodes(dialogueContainer)) return;
            SaveExposedProperties(dialogueContainer);
            SaveCommentBlocks(dialogueContainer);

            if (!AssetDatabase.IsValidFolder("Assets/Resources/Dialogues"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
                AssetDatabase.CreateFolder("Assets/Resources", "Dialogues");
            }

            UnityEngine.Object loadedAsset =
                AssetDatabase.LoadAssetAtPath($"Assets/Resources/Dialogues/{fileName}.asset", typeof(DialogueContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset))
            {
                AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/Dialogues/{fileName}.asset");
            }
            else
            {
                DialogueContainer container = loadedAsset as DialogueContainer;
                container.NodeLinks = dialogueContainer.NodeLinks;
                container.DialogueNodeData = dialogueContainer.DialogueNodeData;
                container.ExposedProperties = dialogueContainer.ExposedProperties;
                container.CommentBlockData = dialogueContainer.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private void SaveCommentBlocks(DialogueContainer dialogueContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>()
                    .Select(x => x.Guid)
                    .ToList();

                dialogueContainer.CommentBlockData.Add(new CommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer)
        {
            dialogueContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
        }

        private bool SaveNodes(DialogueContainer dialogueContainer)
        {
            if (!Edges.Any()) return false;

            var connectedPorts = Edges.Where(edge => edge.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as DialogueNode;
                var inputNode = connectedPorts[i].input.node as DialogueNode;

                if (outputNode == null || inputNode == null) continue;

                dialogueContainer.NodeLinks.Add(new()
                {
                    BaseNodeGuid = outputNode.Guid,
                    PortName = connectedPorts[i].output.portName,
                    TargetNodeGuid = inputNode.Guid,
                });
            }

            foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
            {
                dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
                {
                    Guid = dialogueNode.Guid,
                    DialogueText = dialogueNode.Text,
                    Position = dialogueNode.GetPosition().position
                });
            }

            return true;
        }

        public void LoadGraph(string fileName)
        {
            _container = Resources.Load<DialogueContainer>($"Dialogues/{fileName}");
            if (_container == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exists!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
            CreateExposedProperties();
            GenerateCommentBlocks();
        }

        private void CreateExposedProperties()
        {
            _targetGraphView.ClearBlackBoardAndExposedProperties();
            foreach (var containerExposedProperty in _container.ExposedProperties)
            {
                _targetGraphView.AddPropertyToBlackBoard(containerExposedProperty);
            }
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var connections = _container.NodeLinks.Where(nodeLink => nodeLink.BaseNodeGuid == Nodes[i].Guid)
                    .ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(node => node.Guid == targetNodeGuid);

                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        _container.DialogueNodeData.First(node => node.Guid == targetNodeGuid).Position,
                        _targetGraphView.DefaultNodeSize
                    ));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);

            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _container.DialogueNodeData)
            {
                var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, Vector2.zero);
                tempNode.Guid = nodeData.Guid;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _container.NodeLinks.Where(nodeLink => nodeLink.BaseNodeGuid == nodeData.Guid).ToList();
                nodePorts.ForEach(nodeLink => _targetGraphView.AddChoicePort(tempNode, nodeLink.PortName));
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(node => node.EntryPoint).Guid = _container.NodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint) continue;

                Edges.Where((edge) => edge.input.node == node).ToList()
                    .ForEach(edge => _targetGraphView.RemoveElement(edge));
                _targetGraphView.RemoveElement(node);
            }
        }

        private void GenerateCommentBlocks()
        {
            foreach (var commentBlock in CommentBlocks)
            {
                _targetGraphView.RemoveElement(commentBlock);
            }

            foreach (var commentBlockData in _container.CommentBlockData)
            {
                var block = _targetGraphView.CreateCommentBlock(
                    new Rect(commentBlockData.Position, _targetGraphView.DefaultCommentBlockSize),
                    commentBlockData);
                block.AddElements(Nodes.Where(x => commentBlockData.ChildNodes.Contains(x.Guid)));
            }
        }
    }
}