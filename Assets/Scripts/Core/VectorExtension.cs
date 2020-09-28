using System.Collections.Generic;
using UnityEngine;

namespace TheIslands.Core
{
    public static class VectorExtension {
        public static Size3 AsSize(in this Vector3 vector) => new Size3(vector);
        public static Size3Int AsSize(in this Vector3Int vector) => new Size3Int(vector);

        public static Size3 ToSize(in this Vector3 vector) =>
            new Size3(Mathf.Max(vector.x, 0), Mathf.Max(vector.y, 0), Mathf.Max(vector.z, 0));
        public static Size3Int ToSize(in this Vector3Int vector) =>
            new Size3Int(Mathf.Max(vector.x, 0), Mathf.Max(vector.y, 0), Mathf.Max(vector.z, 0));
        
        /// <summary>
        /// Simple triangulations of points. Produce triangles constructed from the first point in input set
        /// and pairs of subsequent points.
        /// </summary>
        /// <param name="enumerable">Input points.</param>
        /// <returns>Triangles with a shared first point pairwise connected with each other.</returns>
        public static IEnumerable<Triangle> Triangulate(this IEnumerable<Vector3> enumerable) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    yield break;
                
                var p0 = enumerator.Current;
                
                if (!enumerator.MoveNext())
                    yield break;
                
                var p1 = enumerator.Current;
                while (enumerator.MoveNext()) {
                    var p2 = enumerator.Current;
                    yield return new Triangle(p0, p1, p2);
                    
                    p1 = enumerator.Current;
                }
            }
        }
    }
}