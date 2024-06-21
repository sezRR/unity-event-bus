using System;
using UnityEditor.Experimental.GraphView;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases
{
    public abstract class BaseNode : Node
    {
        public string Guid { get; set; } = new Guid().ToString();

        public abstract BaseNode CreateNode();
    }
}