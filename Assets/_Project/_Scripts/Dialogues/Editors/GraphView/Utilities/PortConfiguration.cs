using System;
using UnityEditor.Experimental.GraphView;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Utilities
{
    public class PortConfiguration
    {
        public string Name { get; set; } = "DefaultPortName";
        public Direction Direction { get; set; }
        public Port.Capacity Capacity { get; set; }
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
        public Type Type { get; set; } = typeof(short);
    }
}