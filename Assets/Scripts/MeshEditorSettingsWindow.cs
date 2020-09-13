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
            _editor.FieldRenderer.FieldSamplingRate = EditorGUILayout.FloatField("Sampling Rate:", _editor.FieldRenderer.FieldSamplingRate);
            _editor.FieldRenderer.MinSize           = EditorGUILayout.Slider("Point Min Size:", _editor.FieldRenderer.MinSize,  0,                              _editor.FieldRenderer.MaxSize);
            _editor.FieldRenderer.MaxSize           = EditorGUILayout.Slider("Point Max Size:", _editor.FieldRenderer.MaxSize,  _editor.FieldRenderer.MinSize,  1);
            _editor.FieldRenderer.MinValue          = EditorGUILayout.Slider("Min Value:",      _editor.FieldRenderer.MinValue, 0,                              _editor.FieldRenderer.MaxValue);
            _editor.FieldRenderer.MaxValue          = EditorGUILayout.Slider("Max Value:",      _editor.FieldRenderer.MaxValue, _editor.FieldRenderer.MinValue, 1);
            _editor.FieldRenderer.Color             = EditorGUILayout.ColorField("Point Color:", _editor.FieldRenderer.Color);
        }
    }
}