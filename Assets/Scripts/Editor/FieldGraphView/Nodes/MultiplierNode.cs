using UnityEngine;

namespace TheIslands.Editor.FieldGraphView {
    public class MultiplierNode : FieldNode {
        private MultiplierNode() { }
        
        public static void AddTo(FieldGraphView graphView) {
            var node = new MultiplierNode { title = nameof(MultiplierNode) };

            node.AddInput<MultiplierNode, IDataSampler<Vector2, float>>(x => x.Input1);
            node.AddInput<MultiplierNode, IDataSampler<Vector2, float>>(x => x.Input2);

            node.AddPreview();
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            graphView.AddElement(node);
        }
        
        public override float GetValue(Vector2 p) => 
            (Input1?.GetValue(p) ?? 1f) * (Input2?.GetValue(p) ?? 1f);

        public IDataSampler<Vector2, float> Input1 { get; set; }
        public IDataSampler<Vector2, float> Input2 { get; set; }
    }
}