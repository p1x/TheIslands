using TheIslands.Core;
using TheIslands.Editor.FieldCustomEditor;
using TheIslands.Procedural;
using UnityEditor;

namespace TheIslands.Editor {
    [CustomEditor(typeof(MeshGenerator))]
    public class MeshGeneratorEditor : UnityEditor.Editor {
        private static bool _isFieldsExpanded;
        
        public override void OnInspectorGUI() {
            if (target is MeshGenerator meshGenerator && meshGenerator.Field is ScalarField scalarField) {
                using (var changeCheckScope = new EditorGUI.ChangeCheckScope()) {
                    var size = EditorGUILayout.Vector3IntField("Size", meshGenerator.size).ToSize();
                    var step = EditorGUILayout.Vector3Field("Step", meshGenerator.step).ToSize();
                    
                    if (changeCheckScope.changed) {
                        meshGenerator.Generate();
                        
                        Undo.RecordObject(target, "Change Fields");
                        meshGenerator.size = size;
                        meshGenerator.step = step;
                    }
                }

                using (var changeCheckScope = new EditorGUI.ChangeCheckScope()) {
                    _isFieldsExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_isFieldsExpanded, "Fields");
                    if (_isFieldsExpanded)
                        FieldEditorGUI.InvokeGUI(scalarField);

                    EditorGUILayout.EndFoldoutHeaderGroup();
                    
                    if (changeCheckScope.changed)
                        meshGenerator.Generate();
                }
            }
        }
    }
}