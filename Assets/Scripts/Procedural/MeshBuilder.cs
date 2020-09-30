using System;
using TheIslands.Core;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheIslands.Procedural {
    public class MeshBuilder : IMeshBuilder, IDisposable {
        private readonly Mesh _mesh;
        private readonly int _maxTriangleCount;

        private int _triangleCount;
        private NativeArray<Vertex> _vertices;
        private NativeArray<uint> _indices;
        private unsafe void* _verticesPtr;
        private unsafe void* _indicesPtr;

        public Bounds Bounds { get; set; }

        public unsafe MeshBuilder(int maxTriangleCount, Mesh mesh, NativeArray<Vertex> vertices, NativeArray<uint> indices) {
            _maxTriangleCount = maxTriangleCount;
            _mesh             = mesh;
            _vertices         = vertices;
            _indices          = indices;
            Bounds           = new Bounds(); 
            
            _verticesPtr = _vertices.GetUnsafePtr();
            _indicesPtr = _indices.GetUnsafePtr();
        }

        public void AddTriangle(Triangle triangle) => AddTriangle(triangle.P0, triangle.P1, triangle.P2);
        public unsafe void AddTriangle(Vector3 p0, Vector3 p1, Vector3 p2) {
            if (_triangleCount >= _maxTriangleCount)
                return;

            var normal = Vector3.Cross(p1 - p0, p2 - p1).normalized;

            UnsafeUtility.WriteArrayElement(_verticesPtr, _triangleCount * 3 + 0, new Vertex(p0, normal));
            UnsafeUtility.WriteArrayElement(_verticesPtr, _triangleCount * 3 + 1, new Vertex(p1, normal));
            UnsafeUtility.WriteArrayElement(_verticesPtr, _triangleCount * 3 + 2, new Vertex(p2, normal));

            UnsafeUtility.WriteArrayElement(_indicesPtr, _triangleCount * 3 + 0, (uint)_triangleCount * 3 + 0);
            UnsafeUtility.WriteArrayElement(_indicesPtr, _triangleCount * 3 + 1, (uint)_triangleCount * 3 + 1);
            UnsafeUtility.WriteArrayElement(_indicesPtr, _triangleCount * 3 + 2, (uint)_triangleCount * 3 + 2);

            _triangleCount += 1;
        }
        
        public void Dispose() {
            var count = _triangleCount * 3; // same for indices and vertices
            
            _mesh.SetVertexBufferData(_vertices, 0, 0, count, 0, MeshSource.UpdateFlags);
            _mesh.SetIndexBufferData(_indices, 0, 0, count, MeshSource.UpdateFlags);

            _mesh.SetSubMesh(0, new SubMeshDescriptor(0, count) {
                vertexCount = count,
                bounds = Bounds,
            }, MeshSource.UpdateFlags);

            _mesh.bounds = Bounds;
        }
    }
}