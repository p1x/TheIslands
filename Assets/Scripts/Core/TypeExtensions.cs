using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TheIslands.Core {
    public static class TypeExtensions {
        public static IEnumerable<Type> GetAllSubclasses(this Type type) {
            Assert.IsNotNull(type);
            
            if (!type.IsGenericTypeDefinition)
                return GetAllSubclassesSimple(type);
            
            var baseType = type.EnumerateBase().FirstOrDefault(x => !x.IsGenericTypeDefinition);
            if (baseType == null)
                return Enumerable.Empty<Type>();

            return GetAllSubclassesSimple(baseType).Where(x => IsSubclassOfOpenGeneric(x, type));
        }

        private static IEnumerable<Type> GetAllSubclassesSimple(Type type) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsSubclassOf(type));

        private static bool IsSubclassOfOpenGeneric(Type type,Type openGeneric) {
            Assert.IsNotNull(type);
            Assert.IsNotNull(openGeneric);
            Assert.IsTrue(openGeneric.IsGenericTypeDefinition, "type.IsGenericTypeDefinition");
            return type.EnumerateBase().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == openGeneric);
        }

        public static IEnumerable<Type> WithDefaultConstructor(this IEnumerable<Type> types) =>
            types.Where(x => x.GetConstructor(Array.Empty<Type>()) != null);

        public static IEnumerable<Type> EnumerateBase(this Type type) {
            Assert.IsNotNull(type);
            while (true) {
                if (type.BaseType == null ||
                    type.BaseType == type || 
                    type.BaseType == typeof(object) ||
                    type.BaseType == typeof(UnityEngine.Object))
                    yield break;
                
                yield return type.BaseType;
                
                type = type.BaseType;
            }
        }
        
        public static Type FindBaseType(this Type type, Type baseType) {
            Assert.IsNotNull(type);
            Assert.IsNotNull(baseType);
            while (true) {
                if (type.BaseType == null ||
                    type.BaseType == typeof(object) || 
                    type.BaseType == typeof(UnityEngine.Object))
                    return null;
                
                if (type.BaseType == baseType)
                    return baseType;

                if (baseType.IsGenericTypeDefinition && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == baseType)
                    return type.BaseType;

                type = type.BaseType;
            }
        }
    }
}