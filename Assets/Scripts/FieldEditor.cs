using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TheIslands {
    [EditorTool("Show Field", typeof(MeshGenerator))]
    public class FieldEditor : EditorTool {
        public const float MinSamplingRate = 0.1f;
        
        private MeshEditorSettingsWindow _settingsWindow;
        private float _minSize = 0.1f;
        private float _maxSize = 1f;

        private float _minValue = 0.4f;
        private float _maxValue = 1f;
        
        private Color _color = Color.blue;
        
        private float _fieldSamplingRate = 1f;
        
        private void OnEnable() => EditorTools.activeToolChanged += ActiveToolDidChange;

        private void OnDisable() => EditorTools.activeToolChanged -= ActiveToolDidChange;

        private void ActiveToolDidChange() {
            if (!EditorTools.IsActiveTool(this))
                MeshEditorSettingsWindow.Get().Close();
            else
                MeshEditorSettingsWindow.Get(this).ShowUtility();
        }

        private static Material _material;

        [SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
        private static Material Material {
            get {
                if (_material != null)
                    return _material;

                var shader = Shader.Find("Hidden/Internal-Colored");
                _material = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
                _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                _material.SetInt("_Cull",     (int)UnityEngine.Rendering.CullMode.Off);
                _material.SetInt("_ZWrite",   0);

                return _material;
            }
        }

        public override void OnToolGUI(EditorWindow window) {
            var sceneView = window as SceneView;
            if (sceneView == null)
                return;
            
            var evt = Event.current;
            if (evt.type == EventType.Repaint) {
                if (target is MeshGenerator meshGenerator && meshGenerator.Field is IScalarField field) {
                    RenderField(field, meshGenerator.size);
                }
            }
        }

        private void RenderField(IScalarField field, Vector3 size) {
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

            Material.SetPass(0);
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
