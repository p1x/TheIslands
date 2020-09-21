using TheIslands.Core;
using UnityEditor;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldCustomEditor {
    [Preserve]
    public class SphereFieldEditorGUI : FieldEditorGUI<SphereField> {
        public override void OnGUI(SphereField field) {
            field.Center          = EditorGUILayout.Vector3Field(nameof(field.Center), field.Center);
            field.MaxValue        = EditorGUILayout.FloatField(nameof(field.MaxValue),        field.MaxValue);
            field.HalfValueRadius = EditorGUILayout.FloatField(nameof(field.HalfValueRadius), field.HalfValueRadius);
        }
    }
}