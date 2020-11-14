using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class MarchingCubesTests {
        [Test]
        public void PolygonizeYZFlatField_ProduceSquareSurface() =>
            TestPolygonizeFlatField(new Plane(Vector3.right, Vector3.one / 2), new[] {
                new Vector3(0.5f, 0, 0),
                new Vector3(0.5f, 0, 1),
                new Vector3(0.5f, 1, 1),
                new Vector3(0.5f, 1, 0),
            }, 1f);

        [Test]
        public void PolygonizeXZFlatField_ProduceSquareSurface() =>
            TestPolygonizeFlatField(new Plane(Vector3.up, Vector3.one / 2), new[] {
                new Vector3(0, 0.5f, 0),
                new Vector3(0, 0.5f, 1),
                new Vector3(1, 0.5f, 1),
                new Vector3(1, 0.5f, 0),
            }, 1f);

        [Test]
        public void PolygonizeXYFlatField_ProduceSquareSurface() =>
            TestPolygonizeFlatField(new Plane(Vector3.forward, Vector3.one / 2), new[] {
                new Vector3(0, 0, 0.5f),
                new Vector3(0, 1, 0.5f),
                new Vector3(1, 1, 0.5f),
                new Vector3(1, 0, 0.5f),
            }, 1f);

        [Test]
        public void PolygonizeCornerFlatField_ProduceTriangleSurface() {
            void Test(IReadOnlyList<Vector3> v) => TestPolygonizeFlatField(new Plane(v[0], v[1], v[2]), v, 0.216506347f);

            Test(new[] { new Vector3(0.5f, 0, 0), new Vector3(0, 0.5f, 0), new Vector3(0, 0, 0.5f) });
            Test(new[] { new Vector3(0, 0, 0.5f), new Vector3(0, 0.5f, 1), new Vector3(0.5f, 0, 1) });
            Test(new[] { new Vector3(0.5f, 0, 1), new Vector3(1, 0.5f, 1), new Vector3(1, 0, 0.5f) });
            Test(new[] { new Vector3(1, 0, 0.5f), new Vector3(1, 0.5f, 0), new Vector3(0.5f, 0, 0) });

            Test(new[] { new Vector3(0, 1, 0.5f), new Vector3(0, 0.5f, 0), new Vector3(0.5f, 1, 0) });
            Test(new[] { new Vector3(0.5f, 1, 1), new Vector3(0, 0.5f, 1), new Vector3(0, 1, 0.5f) });
            Test(new[] { new Vector3(1, 1, 0.5f), new Vector3(1, 0.5f, 1), new Vector3(0.5f, 1, 1) });
            Test(new[] { new Vector3(0.5f, 1, 0), new Vector3(1, 0.5f, 0), new Vector3(1, 1, 0.5f) });
        }

        private static void TestPolygonizeFlatField(Plane isoPlane, IEnumerable<Vector3> boundary, float area) {
            var marchingCubes = new MarchingCubes();
            var testField = new TestField(isoPlane);
            var meshBuilder = new TestMeshBuilder();

            marchingCubes.Polygonize(testField, 0.5f, new Size3(1, 1, 1), new Size3Int(1, 1, 1), meshBuilder, CancellationToken.None);

            // Simple and fast asserts
            AssertInPlane(meshBuilder.Triangles, isoPlane);
            AssertContainedInCube(meshBuilder.Triangles);
            AssertArea(meshBuilder.Triangles, area);
            Assert.IsTrue(TestGeometryUtility.IsTrianglesFillBoundary(meshBuilder.Triangles, boundary));
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