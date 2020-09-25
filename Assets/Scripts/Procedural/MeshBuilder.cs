using System;
using TheIslands.Core;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheIslands.Procedural {
    public class MeshBuilder : IMeshBuilder, IDisposable {
        private readonly Mesh _mesh;
        private readonly int _maxTriangleCount;

        private int _triangleCount;
        private NativeArray<Vertex> _vertices;
        private NativeArray<uint> _indices;

        private Bounds _bounds;
        
        public MeshBuilder(int maxTriangleCount, Mesh mesh, NativeArray<Vertex> vertices, NativeArray<uint> indices) {
            _maxTriangleCount = maxTriangleCount;
            _mesh             = mesh;
            _vertices         = vertices;
            _indices          = indices;
            _bounds           = new Bounds(); 
        }

        public void AddTriangle(Triangle triangle) => AddTriangle(triangle.P0, triangle.P1, triangle.P2);
        public void AddTriangle(Vector3 p0, Vector3 p1, Vector3 p2) {
            if (_triangleCount >= _maxTriangleCount)
                return;

            _vertices[_triangleCount * 3 + 0] = new Vertex(p0, Vector3.one);
            _vertices[_triangleCount * 3 + 1] = new Vertex(p1, Vector3.one);
            _vertices[_triangleCount * 3 + 2] = new Vertex(p2, Vector3.one);

            _indices[_triangleCount * 3 + 0] = (uint)_triangleCount * 3 + 0;
            _indices[_triangleCount * 3 + 1] = (uint)_triangleCount * 3 + 1;
            _indices[_triangleCount * 3 + 2] = (uint)_triangleCount * 3 + 2;

            _triangleCount += 1;
            
            // todo: probably should take bounds from outside
            _bounds.Encapsulate(p0);
            _bounds.Encapsulate(p1);
            _bounds.Encapsulate(p2);
        }
        
        public void Dispose() {
            var count = _triangleCount * 3; // same for indices and vertices
            
            _mesh.SetVertexBufferData(_vertices, 0, 0, count, 0, MeshSource.UpdateFlags);
            _mesh.SetIndexBufferData(_indices, 0, 0, count, MeshSource.UpdateFlags);

            _mesh.SetSubMesh(0, new SubMeshDescriptor(0, count) {
                vertexCount = count,
                bounds = _bounds,
            }, MeshSource.UpdateFlags);
        }
    }
}