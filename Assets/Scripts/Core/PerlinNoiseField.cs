using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class PerlinNoiseField : ScalarField {
        [SerializeField, HideInInspector]
        private float _scale = 1f;
        [SerializeField, HideInInspector]
        private float _maxValue = 1f;
        [SerializeField, HideInInspector]
        private float _minValue = 0;
        [SerializeField, HideInInspector]
        private Vector3 _offset = Vector3.zero;
        [SerializeField, HideInInspector]
        private int _octaves = 1;
        [SerializeField, HideInInspector]
        private float _persistence = 1;

        public override float GetValue(Vector3 position) {
            var total = 0f;
            var frequency = 1f/_scale;
            var amplitude = 1f;
            var maxValue = 0f;  // Used for normalizing result to 0.0 - 1.0
            position += _offset;
            for (var i = 0; i < _octaves; i++) {
                total += Noise(position * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= _persistence;
                frequency *= 2;
            }

            return Mathf.Clamp01(total / maxValue) * (_maxValue - _minValue) + _minValue;
        }

        private float Noise(Vector3 p) {
            var xy = Mathf.PerlinNoise(p.x, p.y);
            var yz = Mathf.PerlinNoise(p.y, p.z);
            var zx = Mathf.PerlinNoise(p.z, p.x);

            var yx = Mathf.PerlinNoise(p.y, p.x);
            var zy = Mathf.PerlinNoise(p.z, p.y);
            var xz = Mathf.PerlinNoise(p.x, p.z);

            return (xy + yz + zx + yx + zy + xz) / 6f;
        }

        public float Scale {
            get => _scale;
            set => _scale = value;
        }
        
        public float MaxValue {
            get => _maxValue;
            set => _maxValue = value;
        }

        public float MinValue {
            get => _minValue;
            set => _minValue = value;
        }

        public Vector3 Offset {
            get => _offset;
            set => _offset = value;
        }

        public int Octaves {
            get => _octaves;
            set => _octaves = value;
        }

        public float Persistence {
            get => _persistence;
            set => _persistence = value;
        }
    }
}