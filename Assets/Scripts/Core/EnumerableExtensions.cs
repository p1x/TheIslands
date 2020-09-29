using System;
using System.Collections.Generic;
using System.Linq;

namespace TheIslands.Core {
    public static class EnumerableExtensions {
        public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> enumerable) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    yield break;

                var first = enumerator.Current;
                while (enumerator.MoveNext()) {
                    yield return (first, enumerator.Current);
                    first = enumerator.Current;
                }
            }
        }
        
        public static IEnumerable<(T, T)> PairwiseCycle<T>(this IEnumerable<T> enumerable) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    yield break;

                var theFirst = enumerator.Current;
                var first = enumerator.Current;
                while (enumerator.MoveNext()) {
                    var second = enumerator.Current;
                    yield return (first, second);
                    first = second;
                }

                yield return (first, theFirst);
            }
        }

        public static TSource WithMin<TSource, TComparable>(this IEnumerable<TSource> enumerable, Func<TSource, TComparable> comparerSelector) {
            var comparer = Comparer<TComparable>.Default;
            return enumerable.Aggregate((acc, x) => comparer.Compare(comparerSelector(acc), comparerSelector(x)) <= 0 ? acc : x);
        }
        
        public static TSource WithMax<TSource, TComparable>(this IEnumerable<TSource> enumerable, Func<TSource, TComparable> comparerSelector) {
            var comparer = Comparer<TComparable>.Default;
            return enumerable.Aggregate((acc, x) => comparer.Compare(comparerSelector(acc), comparerSelector(x)) >= 0 ? acc : x);
        }
    }
}