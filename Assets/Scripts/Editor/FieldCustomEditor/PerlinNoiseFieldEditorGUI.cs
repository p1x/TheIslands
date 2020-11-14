using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldCustomEditor {
    [Preserve]
    public class PerlinNoiseFieldEditorGUI : FieldEditorGUI<PerlinNoiseField> {
        public override void OnGUI(PerlinNoiseField field) {
            field.Offset = EditorGUILayout.Vector3Field(nameof(field.Offset), field.Offset);
            field.Scale = EditorGUILayout.FloatField(nameof(field.Scale), field.Scale);
            field.MaxValue = EditorGUILayout.FloatField(nameof(field.MaxValue), field.MaxValue);
            field.MinValue = EditorGUILayout.FloatField(nameof(field.MinValue), field.MinValue);
            field.Octaves = Mathf.Max(0, EditorGUILayout.IntField(nameof(field.Octaves), field.Octaves));
            field.Persistence = EditorGUILayout.Slider(nameof(field.Persistence), field.Persistence, 0f, 1f);
        }
    }
}