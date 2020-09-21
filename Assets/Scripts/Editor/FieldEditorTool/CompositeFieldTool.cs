using TheIslands.Core;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldEditorTool {
    [Preserve]
    public sealed class CompositeFieldTool : ScalarFieldTool<CompositeField> {
        protected override void OnToolGUI(CompositeField field) {
            foreach (var child in field.items)
                if (FieldEditorTool.FieldTools.TryGetValue(child.GetType(), out var editor))
                    editor.OnToolGUI(child);
        }
    }
}