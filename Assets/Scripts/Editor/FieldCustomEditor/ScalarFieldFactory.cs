using System;
using System.Collections.ObjectModel;
using System.Linq;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Editor.FieldCustomEditor {
    public static class ScalarFieldFactory {
        private static ReadOnlyCollection<Type> _fieldTypes = new ReadOnlyCollection<Type>(Array.Empty<Type>());

        private static Lazy<string[]> _fieldNamesLazy = new Lazy<string[]>(CreateFieldTypeNames);

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnRecompile() {
            _fieldTypes = typeof(ScalarField)
                .GetAllSubclasses()
                .WithDefaultConstructor()
                .ToList()
                .AsReadOnly();
            _fieldNamesLazy = new Lazy<string[]>(CreateFieldTypeNames);
        }

        public static string[] FieldNames => _fieldNamesLazy.Value;
        private static string[] CreateFieldTypeNames() => _fieldTypes.Select(x => x.Name).ToArray();

        public static ScalarField Create(int nameIndex) => (ScalarField)ScriptableObject.CreateInstance(_fieldTypes[nameIndex]);
    }
}