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
            var r = _editor.FieldRenderer;
            r.FieldSamplingRate = EditorGUILayout.FloatField("Sampling Rate:", r.FieldSamplingRate);
            r.MinSize           = EditorGUILayout.Slider("Point Min Size:", r.MinSize,  0,          r.MaxSize);
            r.MaxSize           = EditorGUILayout.Slider("Point Max Size:", r.MaxSize,  r.MinSize,  1);
            r.MinValue          = EditorGUILayout.Slider("Min Value:",      r.MinValue, 0,          r.MaxValue);
            r.MaxValue          = EditorGUILayout.Slider("Max Value:",      r.MaxValue, r.MinValue, 1);
            r.Gradient          = EditorGUILayout.GradientField("Color Map:", r.Gradient);
        }
    }
}