using TheIslands.Core;
using UnityEditor;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldCustomEditor {
    [Preserve]
    public class SphereFieldEditorGUI : FieldEditorGUI<SphereField> {
        public override void OnGUI(SphereField field) {
            field.Center = EditorGUILayout.Vector3Field(nameof(field.Center), field.Center);
            field.Radius = EditorGUILayout.FloatField(nameof(field.Radius), field.Radius);
            field.Falloff = EditorGUILayout.FloatField(nameof(field.Falloff), field.Falloff);
        }
    }
}