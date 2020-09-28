using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class MarchingCubesTests {
        [Test]
        public void PolygonizeFlatField() {
            var marchingCubes = new MarchingCubes();
            var testField = new TestField();
            var meshBuilder = new TestMeshBuilder();

            marchingCubes.Polygonize(testField, 0.5f, new Size3(1, 1, 1), new Size3Int(1,1,1),  meshBuilder);

            var plane = new Plane(Vector3.up, new Vector3(0, 0.5f, 0));

            // Simple and fast asserts
            AssertInPlane(meshBuilder.Triangles, plane);
            AssertContainedInCube(meshBuilder.Triangles);
            AssertArea(meshBuilder.Triangles, 1f);

            Assert.IsTrue(
                TestGeometryUtility.IsTrianglesFillBoundary(meshBuilder.Triangles, new[] {
                    new Vector3(0, 0.5f, 0),
                    new Vector3(0, 0.5f, 1),
                    new Vector3(1, 0.5f, 1),
                    new Vector3(1, 0.5f, 0),
                })
            );
        }

        private static void AssertInPlane(IEnumerable<Triangle> triangles, Plane plane) {
            void AssertTriangleInPlane(Triangle t, Plane p) {
                Assert.AreEqual(0, p.GetDistanceToPoint(t.P0));
                Assert.AreEqual(0, p.GetDistanceToPoint(t.P1));
                Assert.AreEqual(0, p.GetDistanceToPoint(t.P2));
            }

            foreach (var triangle in triangles)
                AssertTriangleInPlane(triangle, plane);
        }

        private static void AssertArea(IEnumerable<Triangle> triangles, float area) {
            float GetArea(Triangle t) => Vector3.Cross(t.P0 - t.P1, t.P0 - t.P2).magnitude * 0.5f;
            Assert.AreEqual(area, triangles.Sum(GetArea));
        }

        private static void AssertContainedInCube(IEnumerable<Triangle> triangles) {
            void AssertVectorInCube(Vector3 v) {
                Assert.GreaterOrEqual(v.x, 0);
                Assert.GreaterOrEqual(v.y, 0);
                Assert.GreaterOrEqual(v.z, 0);
                Assert.LessOrEqual(v.x, 1);
                Assert.LessOrEqual(v.y, 1);
                Assert.LessOrEqual(v.z, 1);
            }
            void AssertTriangleInCube(Triangle t) {
                AssertVectorInCube(t.P0);
                AssertVectorInCube(t.P1);
                AssertVectorInCube(t.P2);
            }

            foreach (var triangle in triangles) 
                AssertTriangleInCube(triangle);
        }
    }
}