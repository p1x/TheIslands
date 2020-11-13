using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldEditorTool {
    [Preserve]
    public sealed class DirectionalFieldTool : ScalarFieldTool<DirectionalField> {
        protected override void OnToolGUI(DirectionalField field) {
            field.Start = Handles.PositionHandle(field.Start, Quaternion.identity);
            field.End = Handles.PositionHandle(field.End, Quaternion.identity);
            
            Handles.DrawLine(field.Start, field.End);
        }
    }
}