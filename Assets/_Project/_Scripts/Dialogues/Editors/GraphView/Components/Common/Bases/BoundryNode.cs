using UnityEngine;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases
{
    public abstract class BoundryNode : BaseNode
    {
        public override Vector2 Size { get; set; } = new(100, 150);
    }
}
