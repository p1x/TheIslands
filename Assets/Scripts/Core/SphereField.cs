using UnityEngine;

namespace TheIslands.Core {
    public class SphereField : IScalarField {
        public Vector3 Center { get; set; } = Vector3.zero;
        public float HalfValueRadius { get; set; } = 10;

        public float GetValue(Vector3 position) {
            var distance = Vector3.Distance(Center, position);
            var radius   = HalfValueRadius * 2;
            var value    = Mathf.Max(1 - distance / radius, 0);
            return value;
        }
    }
}