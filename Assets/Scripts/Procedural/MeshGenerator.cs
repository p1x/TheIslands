using TheIslands.Core;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheIslands.Procedural {
    [RequireComponent(typeof(MeshFilter))]
    public class MeshGenerator : MonoBehaviour {
        private static readonly VertexAttributeDescriptor[] VertexAttributes = {
            new VertexAttributeDescriptor(VertexAttribute.Position),
            new VertexAttributeDescriptor(VertexAttribute.Normal),
        };

        private int _generatedMeshId = -1;
        
        private void OnValidate() {
            var meshFilter = GetComponent<MeshFilter>();

            var mesh = meshFilter.sharedMesh;
            if (mesh == null || mesh.GetInstanceID() != _generatedMeshId)
                mesh = meshFilter.sharedMesh = new Mesh();
            _generatedMeshId = mesh.GetInstanceID();
            
            /*  ^ y
             * 4|            7
             *  *-----------*
             *  | -         | -
             *  |   -           -
             *  |     -     |     -
             *  |     5 *-----------* 6
             *  * - - - | - *       |-----> z
             * 0  -     |   3       |
             *      -   |       _   |
             *        - |           |
             *          *-----------* 
             *         1  -          2
             *              _ x
             */

            var meshUpdateFlags = MeshUpdateFlags.Default; // TODO flags

            void SetVertex(NativeArray<Vertex> target, int index, float x, float y, float z) {
                var p = new Vector3(x * size.x, y * size.y, z * size.z);
                var n = (p * 2 - Vector3.one).normalized;
                target[index] = new Vertex(p, n);
            }

            var vertices = new NativeArray<Vertex>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory); 
            SetVertex(vertices, 0, 0, 0, 0);
            SetVertex(vertices, 1, 1, 0, 0);
            SetVertex(vertices, 2, 1, 0, 1);
            SetVertex(vertices, 3, 0, 0, 1);
            SetVertex(vertices, 4, 0, 1, 0);
            SetVertex(vertices, 5, 1, 1, 0);
            SetVertex(vertices, 6, 1, 1, 1);
            SetVertex(vertices, 7, 0, 1, 1);

            mesh.SetVertexBufferParams(vertices.Length, VertexAttributes);
            mesh.SetVertexBufferData(vertices, 0, 0, vertices.Length);

            void SetSquareIndices(NativeArray<uint> target, int face, uint i1, uint i2, uint i3, uint i4) {
                var offset = face * 2 * 3;
                target[offset + 0] = i1;
                target[offset + 1] = i2;
                target[offset + 2] = i3;
                
                target[offset + 3] = i1;
                target[offset + 4] = i3;
                target[offset + 5] = i4;
            }
            
            var indices = new NativeArray<uint>(6 * 2 * 3, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            SetSquareIndices(indices, 0, 0, 1, 2, 3);
            SetSquareIndices(indices, 1, 1, 5, 6, 2);
            SetSquareIndices(indices, 2, 4, 7, 6, 5);
            SetSquareIndices(indices, 3, 0, 3, 7, 4);
            SetSquareIndices(indices, 4, 0, 4, 5, 1);
            SetSquareIndices(indices, 5, 2, 6, 7, 3);
            mesh.SetIndexBufferParams(indices.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(indices, 0, 0, indices.Length, meshUpdateFlags);
            
            var subMeshDescriptor = new SubMeshDescriptor(0, indices.Length);
            mesh.SetSubMesh(0, subMeshDescriptor);
            
            //mesh.RecalculateNormals();
            //mesh.MarkModified();
            //mesh.UploadMeshData(true);
            mesh.bounds = new Bounds(size / 2, size);
        }
        
        private readonly struct Vertex {
            private readonly Vector3 _position;
            private readonly Vector3 _normal;
            public Vertex(Vector3 position, Vector3 normal) {
                _position    = position;
                _normal = normal;
            }
        }

        public Vector3 size;

        public CompositeField Field { get; } = new CompositeField {
            new SphereField { Center = Vector3.one },
            new SphereField { Center = Vector3.zero }
        };
    }
}
