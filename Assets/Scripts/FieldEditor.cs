using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TheIslands {
    [EditorTool("Show Field", typeof(MeshGenerator))]
    public class FieldEditor : EditorTool {

        private MeshEditorSettingsWindow _settingsWindow;
        
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
            
            var evt = Event.current;
            if (evt.type == EventType.Repaint) {
                if (target is MeshGenerator meshGenerator && meshGenerator.Field is IScalarField field) {
                    FieldRenderer.Render(field, meshGenerator.size);
                }
            }
        }
        
        public FieldRenderer FieldRenderer { get; } = new FieldRenderer();
    }
}
