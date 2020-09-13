using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace TheIslands {
    public class FieldRenderer {
        public const float MinSamplingRate = 0.1f;

        private static readonly Lazy<Material> Material = new Lazy<Material>(GetMaterial);

        private float _minSize = 0.1f;
        private float _maxSize = 1f;

        private float _minValue = 0.4f;
        private float _maxValue = 1f;
        
        private Color _color = Color.blue;
        
        private float _fieldSamplingRate = 1f;

        public void Render(IScalarField field, Vector3 size) {
            if (_fieldSamplingRate < MinSamplingRate)
                return;

            var step = 1 / _fieldSamplingRate;

            var transform = Camera.current.transform;
            var right     = transform.right;
            var up        = transform.up;
            var lt        = up - right;
            var rt        = up + right;
            var rb        = -up + right;
            var lb        = -up - right;

            Material.Value.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.QUADS);
            GL.Color(_color);

            for (float x = 0; x < size.x; x += step)
            for (float y = 0; y < size.y; y += step)
            for (float z = 0; z < size.z; z += step) {
                var position    = new Vector3(x, y, z);
                var rawValue    = field.GetValue(position);
                var scaledValue = Mathf.InverseLerp(_minValue, _maxValue, rawValue);
                if (scaledValue > 0 && scaledValue < 1) {
                    var value = Mathf.Clamp01(scaledValue);
                    var scale = Mathf.Lerp(_minSize, _maxSize, value);

                    GL.Vertex(position + lt * scale);
                    GL.Vertex(position + rt * scale);
                    GL.Vertex(position + rb * scale);
                    GL.Vertex(position + lb * scale);
                }
            }

            GL.End();
            GL.PopMatrix();
        }

        [SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
        private static Material GetMaterial() {
            var shader   = Shader.Find("Hidden/Internal-Colored");
            var material = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_Cull",     (int)UnityEngine.Rendering.CullMode.Off);
            material.SetInt("_ZWrite",   0);
            return material;
        }

        private void OnPropertyChanged() => SceneView.RepaintAll();

        public float MinSize {
            get => _minSize;
            set => Utils.RiseAndSetIfChanged(ref _minSize, value, OnPropertyChanged);
        }
        
        public float MaxSize {
            get => _maxSize;
            set => Utils.RiseAndSetIfChanged(ref _maxSize, value, OnPropertyChanged);
        }
        
        public float MinValue {
            get => _minValue;
            set => Utils.RiseAndSetIfChanged(ref _minValue, value, OnPropertyChanged);
        }

        public float MaxValue {
            get => _maxValue;
            set => Utils.RiseAndSetIfChanged(ref _maxValue, value, OnPropertyChanged);
        }

        public Color Color {
            get => _color;
            set => Utils.RiseAndSetIfChanged(ref _color, value, OnPropertyChanged);
        }

        public float FieldSamplingRate {
            get => _fieldSamplingRate;
            set => Utils.RiseAndSetIfChanged(ref _fieldSamplingRate, value, OnPropertyChanged);
        }
    }
}