using System;
using System.Diagnostics.CodeAnalysis;
using TheIslands.Core;
using UnityEditor;
using UnityEngine;

namespace TheIslands.Editor {
    public class FieldRenderer {
        public const float MinSamplingRate = 0.1f;

        private static readonly Lazy<Material> Material = new Lazy<Material>(GetMaterial);

        private float _minSize = 0f;
        private float _maxSize = 1f;

        private float _minValue = 0f;
        private float _maxValue = 1f;
        
        private Gradient _gradient;

        private float _fieldSamplingRate = 1f;
        private bool _showField;

        public FieldRenderer() {
            _gradient = new Gradient();
            var colorKey = new GradientColorKey[5];
            colorKey[0].color = Color.blue;
            colorKey[0].time  = 0.0f;
            colorKey[1].color = Color.cyan;
            colorKey[1].time  = 0.25f;
            colorKey[2].color = Color.green;
            colorKey[2].time  = 0.5f;
            colorKey[3].color = Color.yellow;
            colorKey[3].time  = 0.75f;
            colorKey[4].color = Color.red;
            colorKey[4].time  = 1f;
            var alphaKey = new GradientAlphaKey[3];
            alphaKey[0].alpha = 2 / 255f;
            alphaKey[0].time  = 0.0f;
            alphaKey[1].alpha = 4 / 255f;
            alphaKey[1].time  = 0.75f;
            alphaKey[2].alpha = 1.0f;
            alphaKey[2].time  = 1.0f;
            _gradient.SetKeys(colorKey, alphaKey);
        }

        public void Render(IScalarField field, Vector3 size, Matrix4x4 transformation) {
            if (!ShowField || _fieldSamplingRate < MinSamplingRate)
                return;

            var step = 1 / _fieldSamplingRate;

            var transform = Camera.current.transform;
            var right     = transform.right * step;
            var up        = transform.up * step;
            var lt        = up - right;
            var rt        = up + right;
            var rb        = -up + right;
            var lb        = -up - right;

            Material.Value.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(transformation);
            GL.Begin(GL.QUADS);
            GL.Color(Color.black);

            for (float x = 0; x < size.x; x += step)
            for (float y = 0; y < size.y; y += step)
            for (float z = 0; z < size.z; z += step) {
                var position    = new Vector3(x, y, z);
                var rawValue    = field.GetValue(position);
                var scaledValue = Mathf.InverseLerp(_minValue, _maxValue, rawValue);
                if (scaledValue > 0 && scaledValue < 1) {
                    var value = Mathf.Clamp01(scaledValue);
                    var scale = Mathf.Lerp(_minSize, _maxSize, value);
                    var color = _gradient.Evaluate(scale);
                    
                    GL.Color(color);
                    
                    GL.Vertex(position + lt);
                    GL.Vertex(position + rt);
                    GL.Vertex(position + rb);
                    GL.Vertex(position + lb);
                }
            }

            GL.End();
            GL.PopMatrix();
        }

        private static Material GetMaterial() {
            var shader   = Shader.Find("Editor/FieldShader");
            var material = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
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

        public Gradient Gradient {
            get => _gradient;
            set => Utils.RiseAndSetIfChanged(ref _gradient, value, OnPropertyChanged);
        }

        public float FieldSamplingRate {
            get => _fieldSamplingRate;
            set => Utils.RiseAndSetIfChanged(ref _fieldSamplingRate, value, OnPropertyChanged);
        }

        public bool ShowField {
            get => _showField;
            set => Utils.RiseAndSetIfChanged(ref _showField, value, OnPropertyChanged);
        }
    }
}