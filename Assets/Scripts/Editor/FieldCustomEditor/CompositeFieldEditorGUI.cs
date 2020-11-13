using TheIslands.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace TheIslands.Editor.FieldCustomEditor {
    public abstract class CompositeFieldBaseEditorGUI<T> : FieldEditorGUI<T> where T : CompositeFieldBase {
        public override void OnGUI(T field) {
            field.Items.RemoveAll(x => {
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
                field.Items.Add(ScalarFieldFactory.Create(fieldNameIndex));
        }
    }

    [Preserve]
    public class CompositeFieldEditorGUI : CompositeFieldBaseEditorGUI<CompositeField> { }
    
    [Preserve]
    public class MultiplicativeFieldEditorGUI : CompositeFieldBaseEditorGUI<MultiplicativeField> { }
}