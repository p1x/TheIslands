using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Procedural {
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshGenerator : MonoBehaviour {
        private MeshSource _meshSource;

        public void Generate() {
            if (_meshSource == null) {
                _meshSource = new MeshSource(65535);
                GetComponent<MeshFilter>().sharedMesh = _meshSource.Mesh;
            }

            using (var meshBuilder = _meshSource.GetBuilder()) {
                var s = size.ToSize(); // todo: add size editors and change field type to Size3Int
                meshBuilder.AddTriangle(new Vector3(0, 0, 0), new Vector3(s.X, 0, 0), new Vector3(s.X, 0, s.Z));
                meshBuilder.AddTriangle(new Vector3(0, 0, 0), new Vector3(s.X, 0, s.Z), new Vector3(0, 0, s.Z));

                meshBuilder.AddTriangle(new Vector3(0, 0, 0), new Vector3(0, s.Y, 0), new Vector3(s.X, s.Y, 0));
                meshBuilder.AddTriangle(new Vector3(0, 0, 0), new Vector3(s.X, s.Y, 0), new Vector3(s.X, 0, 0));
                
                meshBuilder.AddTriangle(new Vector3(0, 0, 0), new Vector3(0, s.Y, s.Z), new Vector3(0, s.Y, 0));
                meshBuilder.AddTriangle(new Vector3(0, 0, 0), new Vector3(0, 0, s.Z), new Vector3(0, s.Y, s.Z));
                
                meshBuilder.AddTriangle(new Vector3(0, s.Y, 0), new Vector3(s.X, s.Y, s.Z), new Vector3(s.X, s.Y, 0));
                meshBuilder.AddTriangle(new Vector3(0, s.Y, 0), new Vector3(0, s.Y, s.Z), new Vector3(s.X, s.Y, s.Z));
                
                meshBuilder.AddTriangle(new Vector3(0, 0, s.Z), new Vector3(s.X, s.Y, s.Z), new Vector3(0, s.Y, s.Z));
                meshBuilder.AddTriangle(new Vector3(0, 0, s.Z), new Vector3(s.X, 0, s.Z), new Vector3(s.X, s.Y, s.Z));
                
                meshBuilder.AddTriangle(new Vector3(s.X, 0, 0), new Vector3(s.X, s.Y, 0), new Vector3(s.X, s.Y, s.Z));
                meshBuilder.AddTriangle(new Vector3(s.X, 0, 0), new Vector3(s.X, s.Y, s.Z), new Vector3(s.X, 0, s.Z));
            }
        }
        
        private void OnDestroy() {
            _meshSource?.Dispose();
        }

        public Vector3 size = new Vector3(10, 10, 10);

        [SerializeReference]
        public CompositeField Field = new CompositeField();
    }
}
