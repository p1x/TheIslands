using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace TheIslands.Core {
    public static class TypeUtility {
        public static Dictionary<Type, Type> GetSubclassesOfOpenGeneric(Type openGeneric) {
            Assert.IsNotNull(openGeneric);
            Assert.IsTrue(openGeneric.IsGenericTypeDefinition, "openGeneric.IsGenericTypeDefinition");
            var editorTypes = openGeneric.GetAllSubclasses().WithDefaultConstructor();
            var dictionary  = new Dictionary<Type, Type>();
            foreach (var type in editorTypes) {
                var fieldType = type.FindBaseType(openGeneric).GetGenericArguments()[0];
                dictionary.Add(fieldType, type);
            }
            return dictionary;
        }

        public static Dictionary<Type, T> GetImplementationsOfOpenGeneric<T>(Type openGeneric) {
            Assert.IsNotNull(openGeneric);
            Assert.IsTrue(openGeneric.IsGenericTypeDefinition, "openGeneric.IsGenericTypeDefinition"); 
            Assert.IsTrue(openGeneric.IsSubclassOf(typeof(T)), "openGeneric.IsSubclassOf(typeof(T))");
            return GetSubclassesOfOpenGeneric(openGeneric)
                .ToDictionary(
                    x => x.Key,
                    x => (T)Activator.CreateInstance(x.Value)
                );
        }
    }
}