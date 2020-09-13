using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TheIslands {
    [EditorTool("Show Field", typeof(MeshGenerator))]
    public class FieldEditor : EditorTool {

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
            if (target is MeshGenerator meshGenerator && meshGenerator.Field is IScalarField field) {
                if (evt.type == EventType.Repaint)
                    FieldRenderer.Render(field, meshGenerator.size);

                //EditorGUI.BeginChangeCheck();

                if (FieldEditors.TryGetValue(field.GetType(), out var fieldEditor))
                    fieldEditor.OnToolGUI(field);

                //EditorGUI.EndChangeCheck();
            }
        }
        
        public FieldRenderer FieldRenderer { get; } = new FieldRenderer();

        public static readonly Dictionary<Type, IScalarFieldEditor> FieldEditors = new Dictionary<Type, IScalarFieldEditor>() {
            { typeof(SphereField), new SphereFieldEditor() },
            { typeof(CompositeField), new CompositeFieldEditor() }
        };
    }
    
    public interface IScalarFieldEditor {
        void OnToolGUI(IScalarField field);
    }

    public abstract class ScalarFieldEditor<T> : IScalarFieldEditor where T : IScalarField {
        public void OnToolGUI(IScalarField field) => OnToolGUI((T)field);
        protected abstract void OnToolGUI(T field);
    }

    public sealed class SphereFieldEditor : ScalarFieldEditor<SphereField> {
        protected override void OnToolGUI(SphereField field) {
            field.Center          = Handles.PositionHandle(field.Center, Quaternion.identity);
            field.HalfValueRadius = Handles.RadiusHandle(Quaternion.identity, field.Center, field.HalfValueRadius);
        }
    }

    public sealed class CompositeFieldEditor : ScalarFieldEditor<CompositeField> {
        protected override void OnToolGUI(CompositeField field) {
            foreach (var child in field)
                if (FieldEditor.FieldEditors.TryGetValue(child.GetType(), out var editor))
                    editor.OnToolGUI(child);
        }
    }
}
