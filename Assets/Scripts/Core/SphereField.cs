using UnityEngine;

namespace TheIslands.Core {
    public class SphereField : IScalarField {
        private float _halfValueRadius = 10;
        private float _maxValue = 10;

        private float _a;
        private float _b;

        public SphereField() => UpdateCache();

        public float GetValue(Vector3 position) => _b / (_a + Vector3.Distance(Center, position));

        private void UpdateCache() {
            _a = _halfValueRadius / (2 * _maxValue - 1);
            _b = _maxValue * _a;
        }

        public Vector3 Center { get; set; } = Vector3.zero;

        public float HalfValueRadius {
            get => _halfValueRadius;
            set {
                _halfValueRadius = value;
                UpdateCache();
            }
        }

        public float MaxValue {
            get => _maxValue;
            set {
                _maxValue = value;
                UpdateCache();
            }
        }
    }
}