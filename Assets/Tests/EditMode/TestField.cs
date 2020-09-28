using System.Linq;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class TestField : IScalarField {
        private readonly (Vector3 position, float value)[] _data = {
            (new Vector3(0, 0, 0), 1f),
            (new Vector3(0, 0, 1), 1f),
            (new Vector3(1, 0, 1), 1f),
            (new Vector3(1, 0, 0), 1f),
            (new Vector3(0, 1, 0), 0f),
            (new Vector3(0, 1, 1), 0f),
            (new Vector3(1, 1, 1), 0f),
            (new Vector3(1, 1, 0), 0f),
        };
        
        public float GetValue(Vector3 position) {
            var (_, value) = _data
                .Select(x => (distance: Vector3.Distance(x.position, position), x.value))
                .Aggregate((a, x) => x.distance <= a.distance ? x : a);
            return value;
        }
    }
}