using System;
using System.Collections.Generic;
using TheIslands.Core;
using TheIslands.Procedural;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TheIslands.Editor.FieldEditorTool {
    [EditorTool("Show Fields")]
    public class FieldEditorTool : EditorTool {

        private MeshEditorSettingsWindow _settingsWindow;
        
        private void OnEnable() => EditorTools.activeToolChanged += ActiveToolDidChange;

        private void OnDisable() => EditorTools.activeToolChanged -= ActiveToolDidChange;

        private void ActiveToolDidChange() {
            if (!EditorTools.IsActiveTool(this))
                MeshEditorSettingsWindow.Get().Close();
            else
                MeshEditorSettingsWindow.Get(this).ShowUtility();
        }

        public override void OnToolGUI(EditorWindow window) {
            var sceneView = window as SceneView;
            if (sceneView == null)
                return;
            
            var evt = Event.current;
            if (target is GameObject go && 
                go.GetComponent<MeshGenerator>() is MeshGenerator meshGenerator &&
                meshGenerator.field is IScalarField field) {
                if (evt.type == EventType.Repaint)
                    FieldRenderer.Render(field, meshGenerator.size, meshGenerator.transform.localToWorldMatrix);

                //EditorGUI.BeginChangeCheck();

                if (FieldTools.TryGetValue(field.GetType(), out var fieldEditor))
                    fieldEditor.OnToolGUI(field);

                //EditorGUI.EndChangeCheck();
            }
        }
        
        public FieldRenderer FieldRenderer { get; } = new FieldRenderer();

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnRecompile() {
            FieldTools = TypeUtility.GetImplementationsOfOpenGeneric<ScalarFieldTool>(typeof(ScalarFieldTool<>));
            
        }

        public static Dictionary<Type, ScalarFieldTool> FieldTools;
    }
}
