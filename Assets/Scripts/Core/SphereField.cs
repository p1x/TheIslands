using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class SphereField : ScalarField {
        [SerializeField, HideInInspector]
        private Vector3 _center = Vector3.zero;
        [SerializeField, HideInInspector]
        private float _radius = 10;
        [SerializeField, HideInInspector]
        private float _falloff = 10;

        private float _minRadiusSqr;
        private float _maxRadiusSqr;
        private float _multiplier;
        private float _offset;
        
        public SphereField() => UpdateCache();

        private void UpdateCache() {
            var rMin = _radius - _falloff;
            var rMax = _radius + _falloff;
            _minRadiusSqr = rMin > 0 ? rMin * rMin : 0;
            _maxRadiusSqr = rMax > 0 ? rMax * rMax : 0;
            _multiplier = 0.5f / _falloff;
            _offset = rMin;
        }

        public override float GetValue(Vector3 position) {
            var dSqr = Vector3.SqrMagnitude(position - _center);
            if (dSqr > _maxRadiusSqr)
                return 0;
            if (dSqr < _minRadiusSqr)
                return 1;

            // cubic interpolation
            return Mathf.SmoothStep(1, 0, (Mathf.Sqrt(dSqr) - _offset) * _multiplier);
        }

        public Vector3 Center {
            get => _center;
            set => _center = value;
        }

        public float Radius {
            get => _radius;
            set => Utils.RiseAndSetIfChanged(ref _radius, value, UpdateCache);
        }

        public float Falloff {
            get => _falloff;
            set => Utils.RiseAndSetIfChanged(ref _falloff, value, UpdateCache);
        }
    }
}