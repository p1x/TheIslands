using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldEditorTool {
    [Preserve]
    public sealed class SphereFieldTool : ScalarFieldTool<SphereField> {
        protected override void OnToolGUI(SphereField field) {
            field.Center = Handles.PositionHandle(field.Center, Quaternion.identity);

            var rMin = field.Radius - field.Falloff;
            var rMax = field.Radius + field.Falloff;

            rMin = Mathf.Min(rMax, Handles.RadiusHandle(Quaternion.identity, field.Center, rMin));
            rMax = Mathf.Max(rMin, Handles.RadiusHandle(Quaternion.identity, field.Center, rMax));
            
            field.Radius = (rMax + rMin) / 2f;
            field.Falloff = (rMax - rMin) / 2f;
        }
    }
}