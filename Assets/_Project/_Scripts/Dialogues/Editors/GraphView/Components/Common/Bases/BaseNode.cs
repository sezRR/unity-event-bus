using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases
{
    public abstract class BaseNode : Node
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public virtual Vector2 Size { get; set; } = new(150, 200);
        public virtual Vector2 Position { get; set; } = Vector2.zero;
        
        public sealed override string title
        {
            get => base.title;
            set => base.title = value;
        }

        public abstract BaseNode CreateNode();
    }
}