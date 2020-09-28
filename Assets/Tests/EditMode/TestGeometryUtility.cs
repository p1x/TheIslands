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
        public static bool SurfaceHasNoHoles(IEnumerable<Triangle> triangles, IEnumerable<Vector3> boundary) {
            // To simplify solution and remove edge cases (i.e. boundary-triangle edges)
            // we construct additional triangles and form supposedly closed mesh

            var outlineTriangles = boundary.Triangulate();
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
    }
}