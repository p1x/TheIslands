using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheIslands.Procedural {
    public class MeshSource : IDisposable {
        public const MeshUpdateFlags UpdateFlags = MeshUpdateFlags.DontRecalculateBounds |
                                                   MeshUpdateFlags.DontValidateIndices |
                                                   MeshUpdateFlags.DontNotifyMeshUsers |
                                                   MeshUpdateFlags.DontResetBoneBounds;

        private readonly Mesh _mesh;
        
        private readonly int _maxTriangleCount;
        private NativeArray<Vertex> _vertices;
        private NativeArray<uint> _indices;

        public MeshSource(int maxTriangleCount) {
            _maxTriangleCount = maxTriangleCount;
            
            _mesh = new Mesh();
            _mesh.MarkDynamic();

            _vertices = new NativeArray<Vertex>(_maxTriangleCount * 3, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            _indices  = new NativeArray<uint>(_maxTriangleCount * 3, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            
            _mesh.SetVertexBufferParams(_vertices.Length, Vertex.Attributes);
            _mesh.SetVertexBufferData(_vertices, 0, 0, _vertices.Length, 0, UpdateFlags);
            _mesh.SetIndexBufferParams(_indices.Length, IndexFormat.UInt32);
            _mesh.SetIndexBufferData(_indices, 0, 0, _indices.Length, UpdateFlags);
            _mesh.SetSubMesh(0, new SubMeshDescriptor(), UpdateFlags); // everything to zero
        }
        
        public Mesh Mesh => _mesh;

        public MeshBuilder GetBuilder() => new MeshBuilder(_maxTriangleCount, _mesh, _vertices, _indices);

        public void Dispose() {
            _vertices.Dispose();
            _indices.Dispose();
        }
    }
}