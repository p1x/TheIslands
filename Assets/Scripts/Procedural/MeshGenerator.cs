using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Procedural {
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshGenerator : MonoBehaviour {
        private MeshSource _meshSource;

        private void OnEnable() {
            _meshSource?.Dispose();
            _meshSource = new MeshSource(8192 * 3);

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
                var actualSize = new Vector3(size.x * step.x, size.y * step.y, size.z * step.z);
                meshBuilder.Bounds = new Bounds(actualSize / 2, actualSize);
                
                var marchingCubes = new MarchingCubes();
                marchingCubes.Polygonize(Field, 0.5f, step.ToSize(), size.ToSize(), meshBuilder);
            }
        }

        public Vector3Int size = new Vector3Int(10, 10, 10); 
        public Vector3 step = new Vector3(1, 1, 1);
        
        [SerializeReference]
        public CompositeField Field = new CompositeField();
    }
}
