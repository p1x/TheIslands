using NUnit.Framework;
using TheIslands.Procedural;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class MeshSourceTests {
        [Test]
        public void AfterCreation_ProducesEmptyMesh() {
            using (var meshSource = new MeshSource(0)) {
                AssertEmptyMesh(meshSource.Mesh);
            }
        }

        [Test]
        public void BuildingWithZeroMaxTriangles_ProducesEmptyMesh() {
            using (var meshSource = new MeshSource(0)) {
                using (var builder = meshSource.GetBuilder())
                    builder.AddTriangle(Vector3.zero, Vector3.forward, Vector3.one);
                
                AssertEmptyMesh(meshSource.Mesh);
            }
        }

        [Test]
        public void BuildingWithExcessiveMaxTriangles_ProducesValidMesh() {
            using (var meshSource = new MeshSource(24)) {
                using (var builder = meshSource.GetBuilder())
                    builder.AddTriangle(Vector3.zero, Vector3.forward, Vector3.one);

                AssertValidMesh(meshSource.Mesh, 3, 3, new Bounds(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one));
            }
        }

        [Test]
        public void BuildingWithInsufficientMaxTriangles_ProducesValidMeshWithLimitedSize() {
            using (var meshSource = new MeshSource(1)) {
                using (var builder = meshSource.GetBuilder()) {
                    builder.AddTriangle(Vector3.zero, Vector3.forward, Vector3.one);
                    builder.AddTriangle(Vector3.one, Vector3.forward + Vector3.one, Vector3.one + Vector3.one);
                }

                AssertValidMesh(meshSource.Mesh, 3, 3, new Bounds(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one));
            }
        }

        private static void AssertValidMesh(Mesh mesh, int vertexCount, int indexCount, Bounds bounds) {
            var subMesh = mesh.GetSubMesh(0);

            Assert.AreEqual(1, mesh.vertexBufferCount);
            Assert.AreEqual(vertexCount, subMesh.vertexCount);
            Assert.AreEqual(indexCount, subMesh.indexCount);
            Assert.AreEqual(bounds, subMesh.bounds);
        }

        private static void AssertEmptyMesh(Mesh mesh) {
            Assert.AreEqual(0, mesh.vertexCount);
            Assert.AreEqual(0, mesh.triangles.Length);
            Assert.AreEqual(0, mesh.vertices.Length);
            Assert.AreEqual(0, mesh.normals.Length);
            Assert.AreEqual(1, mesh.subMeshCount);
            Assert.AreEqual(new Bounds(), mesh.bounds);
            
            var subMesh = mesh.GetSubMesh(0);

            Assert.AreEqual(0, mesh.vertexBufferCount);
            Assert.AreEqual(0, subMesh.vertexCount);
            Assert.AreEqual(0, subMesh.indexCount);
            Assert.AreEqual(new Bounds(), subMesh.bounds);
        }
    }
}