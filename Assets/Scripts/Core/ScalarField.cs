using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public abstract class ScalarField : IScalarField { 
        public abstract float GetValue(Vector3 position);
    }
}