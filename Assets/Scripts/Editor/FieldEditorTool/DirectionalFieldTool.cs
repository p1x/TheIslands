using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldEditorTool {
    [Preserve]
    public sealed class DirectionalFieldTool : ScalarFieldTool<DirectionalField> {
        protected override void OnToolGUI(DirectionalField field) {
            Handles.DrawLine(field.Start, field.End);
            Handles.CubeHandleCap(0, field.Start, Quaternion.identity, HandleUtility.GetHandleSize(field.Start) * 0.1f, EventType.Repaint);
            Handles.ConeHandleCap(0, field.End, Quaternion.LookRotation(field.End - field.Start), HandleUtility.GetHandleSize(field.End) * 0.2f, EventType.Repaint);
            
            field.Start = Handles.PositionHandle(field.Start, Quaternion.identity);
            field.End = Handles.PositionHandle(field.End, Quaternion.identity);
        }
    }
}