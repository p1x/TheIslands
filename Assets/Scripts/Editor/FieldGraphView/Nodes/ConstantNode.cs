using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheIslands.Editor.FieldGraphView {
    public class ConstantNode : FieldNode {
        private float _value;
        private ConstantNode() { }

        public static void AddTo(FieldGraphView graphView) {
            var node = new ConstantNode { title = nameof(ConstantNode) };

            node.AddFloatField<ConstantNode>(x => x.Value);
            
            node.RefreshExpandedState();
            node.RefreshPorts();

            graphView.AddElement(node);
        }
        
        public override float GetValue(Vector2 p) => Value;

        public float Value {
            get => _value;
            set => Utils.RiseAndSetIfChanged(ref _value, in value, RaisePropertyChanged);
        }
    }
}