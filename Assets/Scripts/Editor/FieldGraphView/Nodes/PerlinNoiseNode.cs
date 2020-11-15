using UnityEngine;

namespace TheIslands.Editor.FieldGraphView {
    public class PerlinNoiseNode : FieldNode {
        private float _scale = 64f;
        private int _octaves = 4;
        private float _persistence = 0.25f;
        private PerlinNoiseNode() { }
        
        public static void AddTo(FieldGraphView graphView) {
            var node = new PerlinNoiseNode { title = nameof(PerlinNoiseNode) };

            node.AddFloatField<PerlinNoiseNode>(x => x.Scale);
            node.AddIntegerField<PerlinNoiseNode>(x => x.Octaves);
            node.AddFloatField<PerlinNoiseNode>(x => x.Persistence);

            node.AddPreview();
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            graphView.AddElement(node);
        }

        public override float GetValue(Vector2 p) {
            var total = 0f;
            var frequency = 1f/_scale;
            var amplitude = 1f;
            var maxValue = 0f;  // Used for normalizing result to 0.0 - 1.0
            //position += _offset;
            for (var i = 0; i < _octaves; i++) {
                total += Mathf.PerlinNoise(p.x * frequency, p.y * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= _persistence;
                frequency *= 2;
            }

            return Mathf.Clamp01(total / maxValue);
        }

        public float Scale {
            get => _scale;
            set => SetIfChanged(ref _scale, in value);
        }

        public int Octaves {
            get => _octaves;
            set => SetIfChanged(ref _octaves, in value);
        }

        public float Persistence {
            get => _persistence;
            set => SetIfChanged(ref _persistence, in value);
        }
    }
}