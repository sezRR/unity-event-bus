using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new();
    public List<DialogueNodeData> DialogueNodeData = new();
    public List<ExposedProperty> ExposedProperties = new();
    public List<CommentBlockData> CommentBlockData = new List<CommentBlockData>();
}
