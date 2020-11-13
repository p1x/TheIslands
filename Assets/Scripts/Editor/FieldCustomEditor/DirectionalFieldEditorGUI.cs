using TheIslands.Core;
using UnityEditor;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldCustomEditor {
    [Preserve]
    public class DirectionalFieldEditorGUI : FieldEditorGUI<DirectionalField> {
        public override void OnGUI(DirectionalField field) {
            field.Start =  EditorGUILayout.Vector3Field(nameof(field.Start), field.Start);
            field.End =  EditorGUILayout.Vector3Field(nameof(field.End), field.End);
            field.Curve = EditorGUILayout.CurveField(nameof(field.Curve), field.Curve);
        }
    }
}