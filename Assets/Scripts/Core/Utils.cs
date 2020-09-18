using System;

namespace TheIslands.Core {
    public static class Utils {
        public static void RiseAndSetIfChanged<T>(ref T field, in T value, Action action) where T : struct, IEquatable<T> {
            if (field.Equals(value))
                return;

            field = value;
            action();
        }
        
        public static void RiseAndSetIfChanged<T>(ref T field, T value, Action action) where T : class {
            if (ReferenceEquals(field, value))
                return;

            field = value;
            action();
        }
    }
}