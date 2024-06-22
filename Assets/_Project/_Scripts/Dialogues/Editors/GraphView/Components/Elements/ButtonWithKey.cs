using System;
using UnityEngine.UIElements;

namespace _Project._Scripts.Dialogues.Editors.GraphView.Components.Elements
{
    public class ButtonWithKey : Button
    {
        public string Key { get; set; }

        public sealed override string text
        {
            get => base.text;
            set => base.text = value;
        }
        
        public ButtonWithKey(string key, string ttext, Action action) : base(action)
        {
            Key = key;
            text = ttext;
        }

        public ButtonWithKey(string key, string ttext)
        {
            Key = key;
            text = ttext;
        }

        public ButtonWithKey(string key)
        {
            Key = key;
        }

        public void RegisterClickEvent(Action @event)
        {
            base.clickable = new Clickable(@event);
        } 
    }
}