using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Editor {
    public abstract class FieldEditorGUI<T> : FieldEditorGUI where T : ScalarField {
        public override void OnGUI(ScalarField field) => OnGUI((T)field);
        public abstract void OnGUI(T field);
    }

    public abstract class FieldEditorGUI {
        public static ReadOnlyDictionary<Type, FieldEditorGUI> Editors { get; private set; }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnRecompile() { 
            var editorTypes = typeof(FieldEditorGUI<>).GetAllSubclasses().WithDefaultConstructor();
            var dictionary  = new Dictionary<Type, FieldEditorGUI>();
            foreach (var type in editorTypes) {
                var fieldType = type.FindBaseType(typeof(FieldEditorGUI<>)).GetGenericArguments()[0];
                dictionary.Add(fieldType, (FieldEditorGUI)Activator.CreateInstance(type));
            }
            Editors = new ReadOnlyDictionary<Type, FieldEditorGUI>(dictionary);
        }

        public static void InvokeGUI(ScalarField field) {
            if (Editors.TryGetValue(field.GetType(), out var editorGUI))
                editorGUI.OnGUI(field);
            else
                Debug.LogError($"Editor for type '{field}' not found.");
        }

        public abstract void OnGUI(ScalarField field);
    }
}