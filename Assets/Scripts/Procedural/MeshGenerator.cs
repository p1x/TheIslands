using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Procedural {
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshGenerator : MonoBehaviour {
        private MeshSource _meshSource;

        private void OnEnable() {
            _meshSource?.Dispose();
            _meshSource = new MeshSource(12 * 3);

            if (!TryGetComponent(out MeshFilter meshFilter))
                meshFilter = gameObject.AddComponent<MeshFilter>();
            
            meshFilter.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            meshFilter.mesh = _meshSource.Mesh;
            
            Generate();
        }

        private void OnDisable() {
            _meshSource?.Dispose();
            _meshSource = null;
            if (TryGetComponent(out MeshFilter meshFilter)) 
                meshFilter.mesh = null;
        }

        public void Generate() {
            if (_meshSource == null)
                return;

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

        public Vector3Int size = new Vector3Int(10, 10, 10);

        [SerializeReference]
        public CompositeField Field = new CompositeField();
    }
}
