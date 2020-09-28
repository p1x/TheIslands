using System;
using System.Collections.Generic;
using System.Linq;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public static class TestGeometryUtility {
        /// <summary>
        /// Check if triangles form a surface without holes with specified boundary. 
        /// </summary>
        /// <param name="triangles">Triangles</param>
        /// <param name="boundary">Boundary represented by ordered points.</param>
        /// <returns><value>true</value> - if no holes found, otherwise - <value>false</value></returns>
        [Obsolete("Do not use until fixed")] public static bool SurfaceHasNoHoles(IEnumerable<Triangle> triangles, IEnumerable<Vector3> boundary) {
            // To simplify solution and remove edge cases (i.e. boundary-triangle edges)
            // we construct additional triangles and form supposedly closed mesh.

            // We need an additional point to not overlap with existed triangles and avoid false-positives.
            // Bug: this fixes the issue in most cases, but not all of them
            var externalPoint = new Vector3(-1000, -1000, -1000);   
            var boundaryList = boundary.ToList();
            var outlineTriangles = boundaryList.Append(boundaryList[0]).Prepend(externalPoint).Triangulate();

            var allTriangles = triangles.Concat(outlineTriangles).ToList();

            return IsTrianglesFormClosedMesh(allTriangles);
        }
        
        /// <summary>
        /// Check if list of triangles forms a closed mesh (has no holes).
        /// </summary>
        public static unsafe bool IsTrianglesFormClosedMesh(IReadOnlyList<Triangle> triangles) {
            // Mesh is closed if each edge is shared by exactly 2 triangles.
            // So we iterate over all triangles and count shared edges for each triangle.

            var sharedEdgesCount = stackalloc int[3];

            void CountSharedEdges(Triangle t1, Triangle t2) {
                for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    if (t1.GetEdge(i) == t2.GetEdge(j))
                        sharedEdgesCount[i] += 1;
            }
            bool ValidateEdges() => sharedEdgesCount[0] == 1 && sharedEdgesCount[1] == 1 && sharedEdgesCount[2] == 1;

            // Iterate over triangles, count shared edges and assert.
            for (var i = 0; i < triangles.Count; i++) {
                for (var k = 0; k < 3; k++) 
                    sharedEdgesCount[k] = 0;
                
                for (var j = 0; j < triangles.Count; j++)
                    if (j != i)
                        CountSharedEdges(triangles[i], triangles[j]);

                if (!ValidateEdges())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if triangles completely fill boundary polygon.
        /// </summary>
        /// <param name="triangles">List of triangles to check</param>
        /// <param name="boundary">List of boundary edges</param>
        public static bool IsTrianglesFillBoundary(IList<Triangle> triangles, IEnumerable<Vector3> boundary) {
            var boundaryEdges = boundary.PairwiseCycle().Select(x => new Edge(x.Item1, x.Item2)).ToList();
            return IsTrianglesFillBoundary(triangles, boundaryEdges);
        }

        /// <summary>
        /// Check if triangles completely fill boundary polygon.
        /// </summary>
        /// <param name="triangles">List of triangles to check</param>
        /// <param name="boundary">List of boundary edges</param>
        public static bool IsTrianglesFillBoundary(IList<Triangle> triangles, IList<Edge> boundary) {
            // Triangles completely fill polygon if all edges are shared exactly by 2 triangles or by a triangle and a boundary edge
            
            int CountSharedEdges(Edge edge, in Triangle triangle) => triangle.GetEdges().Count(e => e == edge);
            
            // Check if all triangle edges are shared with exactly one other triangle or boundary 
            foreach (var triangle in triangles)
            foreach (var edge in triangle.GetEdges()) {
                // Count shared edges between triangles
                var triangleSharedCount =
                    triangles
                        .Where(x => x != triangle)
                        .Sum(t => CountSharedEdges(edge, t));
                
                // Count shared edges between triangle and boundary
                var boundarySharedCount = boundary.Sum(t => edge == t ? 1 : 0);

                // It should be exactly one
                if (triangleSharedCount + boundarySharedCount != 1)
                    return false;
            }

            // Check if only one triangle shares an edge with each boundary edge.
            return boundary.All(b => triangles.Sum(t => CountSharedEdges(b, t)) == 1);
        }
    }
}