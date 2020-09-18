using UnityEngine;

namespace TheIslands.Core {
    public interface IScalarField {
        float GetValue(Vector3 position);
    }
}