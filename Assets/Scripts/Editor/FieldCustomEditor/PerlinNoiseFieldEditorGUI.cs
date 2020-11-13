using TheIslands.Core;
using UnityEditor;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldCustomEditor {
    [Preserve]
    public class PerlinNoiseFieldEditorGUI : FieldEditorGUI<PerlinNoiseField> {
        public override void OnGUI(PerlinNoiseField field) {
            field.Offset = EditorGUILayout.Vector3Field(nameof(field.Offset), field.Offset);
            field.Scale = EditorGUILayout.FloatField(nameof(field.Scale), field.Scale);
            field.Amplitude = EditorGUILayout.Slider(nameof(field.Amplitude), field.Amplitude, 0f, 1f);
        }
    }
}