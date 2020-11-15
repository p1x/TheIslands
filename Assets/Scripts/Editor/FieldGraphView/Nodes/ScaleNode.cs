using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Editor.FieldGraphView {
    public class ScaleNode : FieldNode {
        private float _multiplier = 0.5f;

        private ScaleNode() { }
        
        public static void AddTo(FieldGraphView graphView) {
            var node = new ScaleNode { title = nameof(ScaleNode) };

            node.AddInput<ScaleNode, IDataSampler<Vector2, float>>(x => x.Input);
            node.AddFloatField<ScaleNode>(x => x.Multiplier);

            node.AddPreview();
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            graphView.AddElement(node);
        }

        public override float GetValue(Vector2 p) {
            if (Input == null)
                return 0;

            return Input.GetValue(p) * Multiplier;
        }

        public IDataSampler<Vector2, float> Input { get; set; }

        public float Multiplier {
            get => _multiplier;
            set => Utils.RiseAndSetIfChanged(ref _multiplier, in value, RaisePropertyChanged);
        }
    }
}