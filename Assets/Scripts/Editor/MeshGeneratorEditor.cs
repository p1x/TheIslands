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
                    var meshGeneratorSize = EditorGUILayout.Vector3Field("Size", meshGenerator.size).ToSize();
                    
                    if (changeCheckScope.changed) {
                        meshGenerator.Generate();
                        
                        Undo.RecordObject(target, "Change Fields");
                        meshGenerator.size = meshGeneratorSize;
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