using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class SphereField : ScalarField {
        [SerializeField, HideInInspector]
        private float _halfValueRadius = 2;
        [SerializeField, HideInInspector]
        private float _maxValue = 1;

        [SerializeField, HideInInspector]
        private float _a;
        [SerializeField, HideInInspector]
        private float _b;

        [SerializeField, HideInInspector]
        private Vector3 _center = Vector3.zero;

        public SphereField() => UpdateCache();

        public override float GetValue(Vector3 position) => _b / (_a + Vector3.Distance(Center, position));

        private void UpdateCache() {
            _a = _halfValueRadius / (2 * _maxValue - 1);
            _b = _maxValue * _a;
        }

        public Vector3 Center {
            get => _center;
            set => _center = value;
        }

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