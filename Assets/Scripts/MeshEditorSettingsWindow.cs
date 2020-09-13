using UnityEditor;

namespace TheIslands {
    public class MeshEditorSettingsWindow : EditorWindow {
        private const string Title = "Field Editor Settings";
        private FieldEditor _editor;
        
        public static MeshEditorSettingsWindow Get() => GetWindow<MeshEditorSettingsWindow>(true, Title, false);

        public static MeshEditorSettingsWindow Get(FieldEditor editor) {
            var window = Get();
            window._editor = editor;
            return window;
        }

        private void OnGUI() {
            _editor.Scale             = EditorGUILayout.Slider("Point Size:", _editor.Scale, 0, 1);
            _editor.FieldSamplingRate = EditorGUILayout.FloatField("Sampling Rate:", _editor.FieldSamplingRate);
        }
    }
}