using UnityEngine;

namespace TheIslands.Editor.FieldGraphView {
    public class BorderFieldNode : FieldNode {
        private float _borderSize;
        private BorderFieldNode() { }
        
        public static void AddTo(FieldGraphView graphView) {
            var node = new BorderFieldNode { title = nameof(BorderFieldNode) };

            node.AddFloatField<BorderFieldNode>(x => x.BorderSize);
            
            node.AddPreview();
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            graphView.AddElement(node);
        }
        
        public override float GetValue(Vector2 p) {
            var size = BorderSize;
            var xDistance = Mathf.Min(p.x, PreviewSize.x - p.x) / size;
            var yDistance = Mathf.Min(p.y, PreviewSize.y - p.y) / size;
            var distance = Mathf.Min(xDistance, yDistance);
            return Mathf.Clamp01(distance);
        }
        
        public IDataSampler<Vector2, float> Input { get; set; }

        public float BorderSize {
            get => _borderSize;
            set => SetIfChanged(ref _borderSize, in value);
        }
    }
}