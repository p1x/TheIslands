using System.Threading;
using System.Threading.Tasks;
using TheIslands.Core;
using UnityEditor;
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

        private Task _generationTask;
        private CancellationTokenSource _generationCancellation = new CancellationTokenSource();

        public void Update() {
            if (!_isInvalid) 
                return;
            
            if (_generationTask == null || _generationTask.IsCanceled || _generationTask.IsCompleted) {
                StartNewTask();
                _isInvalid = false;
            } else {
                _generationCancellation.Cancel();
            }
        }

        private void OnDrawGizmosSelected() {
            var actualSize = new Vector3(size.x * step.x, size.y * step.y, size.z * step.z);
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(actualSize / 2f, actualSize);
        }

        private bool _isInvalid = true;

        private void StartNewTask() {
            var meshBuilder = _meshSource.GetBuilder();
            var actualSize = new Vector3(size.x * step.x, size.y * step.y, size.z * step.z);
            meshBuilder.Bounds = new Bounds(actualSize / 2f, actualSize);
            
            _generationCancellation = new CancellationTokenSource();
            _generationTask = Task.Factory.StartNew(
                    () => {
                        var marchingCubes = new MarchingCubes();
                        marchingCubes.Polygonize(Field, isoLevel, step.ToSize(), size.ToSize(), meshBuilder,
                            _generationCancellation.Token);
                    },
                    _generationCancellation.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Current
                )
                .ContinueWith(
                    t => meshBuilder.Dispose(),
                    CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
        
        public void Generate() {
            if (_meshSource == null)
                return;

            _isInvalid = true;
            EditorUtility.SetDirty(this);
        }

        public Vector3Int size = new Vector3Int(10, 10, 10); 
        public Vector3 step = new Vector3(1, 1, 1);
        [Range(0, 1)]
        public float isoLevel = 0.5f;
        
        [SerializeReference]
        public CompositeField Field = new CompositeField();
    }
}
