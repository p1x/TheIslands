using System.Collections.Generic;
using UnityEngine;

namespace TheIslands.Core {
    public class CompositeField : List<IScalarField>, IScalarField {
        public float GetValue(Vector3 position) {
            var result = 0f;
            for (var i = 0; i < Count; i++)
                result += this[i].GetValue(position);
            return result / Count;
        }
    }
}