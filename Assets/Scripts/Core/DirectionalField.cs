using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class DirectionalField : ScalarField {
        [SerializeField, HideInInspector] 
        private AnimationCurve _curve = AnimationCurve.Constant(0, 1, 0);

        [SerializeField, HideInInspector]
        private Vector3 _start = Vector3.up;
        
        [SerializeField, HideInInspector]
        private Vector3 _end = Vector3.zero;

        private Vector3 _direction;
        private float _lengthSqr;
        
        public DirectionalField() => UpdateCache();

        private void UpdateCache() {
            _direction = _end - _start;
            _lengthSqr = _direction.sqrMagnitude;
        }

        public override float GetValue(Vector3 position) {
            if (_lengthSqr == 0f)
                return 0;
            
            var x = Vector3.Dot(_direction, position - _start) / _lengthSqr;
            return _curve.Evaluate(x);
        }

        public Vector3 Start {
            get => _start;
            set => Utils.RiseAndSetIfChanged(ref _start, value, UpdateCache);
        }
        public Vector3 End {
            get => _end;
            set => Utils.RiseAndSetIfChanged(ref _end, value, UpdateCache);
        }
        public AnimationCurve Curve {
            get => _curve;
            set => Utils.RiseAndSetIfChanged(ref _curve, value, UpdateCache);
        }
    }
}