using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class PerlinNoiseField : ScalarField {
        [SerializeField, HideInInspector]
        private float _scale = 1f;
        [SerializeField, HideInInspector]
        private float _amplitude = 1f;
        [SerializeField, HideInInspector]
        private Vector3 _offset = Vector3.zero;

        public override float GetValue(Vector3 position) {
            return Mathf.Clamp01(Noise((position + _offset) * _scale) * _amplitude);
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
        
        public float Amplitude {
            get => _amplitude;
            set => _amplitude = value;
        }
        
        public Vector3 Offset {
            get => _offset;
            set => _offset = value;
        }
    }
}