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
            _editor.FieldSamplingRate = EditorGUILayout.FloatField("Sampling Rate:", _editor.FieldSamplingRate);
            _editor.MinSize           = EditorGUILayout.Slider("Point Min Size:", _editor.MinSize,  0,                _editor.MaxSize);
            _editor.MaxSize           = EditorGUILayout.Slider("Point Max Size:", _editor.MaxSize,  _editor.MinSize,  1);
            _editor.MinValue          = EditorGUILayout.Slider("Min Value:",      _editor.MinValue, 0,                _editor.MaxValue);
            _editor.MaxValue          = EditorGUILayout.Slider("Max Value:",      _editor.MaxValue, _editor.MinValue, 1);
            _editor.Color             = EditorGUILayout.ColorField("Point Color:", _editor.Color);
        }
    }
}