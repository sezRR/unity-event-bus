using _Project._Scripts.Dialogues.Editors.GraphView.Components.Common.Bases;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Helpers
{
    public static class StyleSheetHelper
    {
        public static void AddStyleSheetToVisualElement(BaseNode baseNode, StyleSheet styleSheet)
        {
            baseNode.styleSheets.Add(styleSheet);
        }
        
        public static void AddStyleSheetToVisualElementFromResources(BaseNode baseNode, string path)
        {
            var styleSheet = Resources.Load<StyleSheet>(path);
            AddStyleSheetToVisualElement(baseNode, styleSheet);
        }
    }
}