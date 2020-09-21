using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldEditorTool {
    [Preserve]
    public sealed class SphereFieldTool : ScalarFieldTool<SphereField> {
        protected override void OnToolGUI(SphereField field) {
            field.Center          = Handles.PositionHandle(field.Center, Quaternion.identity);
            field.HalfValueRadius = Handles.RadiusHandle(Quaternion.identity, field.Center, field.HalfValueRadius);
        }
    }
}