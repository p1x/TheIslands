using UnityEngine;

namespace TheIslands.Core {
    public abstract class ScalarField : ScriptableObject, IScalarField {
        public abstract float GetValue(Vector3 position);
    }
}