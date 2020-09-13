using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TheIslands {
    [EditorTool("Show Field", typeof(MeshGenerator))]
    public class FieldEditor : EditorTool {
        public const float MinSamplingRate = 0.1f;
        
        private MeshEditorSettingsWindow _settingsWindow;
        private float _scale = 0.1f;
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
                    
                    for (float x = 0; x < meshGenerator.size.x; x += step)
                    for (float y = 0; y < meshGenerator.size.y; y += step)
                    for (float z = 0; z < meshGenerator.size.z; z += step) {
                        var position = new Vector3(x, y, z);
                        var value    = Mathf.Clamp01(field.GetValue(position));
                        var scale    = value * _scale;

                        GL.Color(Color.blue);
                        GL.Vertex(position + lt * scale);
                        GL.Vertex(position + rt * scale);
                        GL.Vertex(position + rb * scale);
                        GL.Vertex(position + lb * scale);
                    }
                    GL.End();
                    GL.PopMatrix();
                }
            }
        }

        private void OnPropertyChanged() => SceneView.RepaintAll();
        
        public float Scale {
            get => _scale;
            set => Utils.RiseAndSetIfChanged(ref _scale, value, OnPropertyChanged);
        }

        public float FieldSamplingRate {
            get => _fieldSamplingRate;
            set => Utils.RiseAndSetIfChanged(ref _fieldSamplingRate, value, OnPropertyChanged);
        }
    }
}
