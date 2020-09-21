using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor {
    [Preserve]
    public class CompositeFieldEditorGUI : FieldEditorGUI<CompositeField> {
        public override void OnGUI(CompositeField field) {
            field.items.RemoveAll(x => {
                bool remove;
                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField(x.GetType().Name);
                    remove = GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_close"), GUILayout.ExpandWidth(false));
                }
                
                using (new EditorGUI.IndentLevelScope())
                    InvokeGUI(x);

                return remove;
            });

            var fieldNameIndex = EditorGUILayout.Popup("Add", -1, ScalarFieldFactory.FieldNames);
            if (fieldNameIndex >= 0 && fieldNameIndex < ScalarFieldFactory.FieldNames.Length)
                field.items.Add(ScalarFieldFactory.Create(fieldNameIndex));
        }
    }
}