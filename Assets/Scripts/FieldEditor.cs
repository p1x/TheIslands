using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TheIslands {
    [EditorTool("Show Field", typeof(MeshGenerator))]
    public class FieldEditor : EditorTool {
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
        
        public override void OnToolGUI(EditorWindow window) {
            var sceneView = window as SceneView;
            if (sceneView == null)
                return;

            var meshGenerator = target as MeshGenerator;
            if (meshGenerator == null)
                return;
            
            var evt = Event.current;
            if (evt.type == EventType.Repaint) {
                var cameraRotation = Camera.current.transform.localToWorldMatrix.rotation;
                var cameraDirection = Matrix4x4.Rotate(cameraRotation).MultiplyVector(Vector3.forward);
                using (new Handles.DrawingScope(Color.blue)) {
                    Handles.DrawSolidDisc(Vector3.one, cameraDirection, Scale);
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
