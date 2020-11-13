using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class MultiplicativeField : CompositeFieldBase {
        public override float GetValue(Vector3 position) {
            if (_items == null || _items.Count == 0) // Empty and not initialized
                return 0;

            var x = _items[0].GetValue(position);
            for (var i = 1; i < _items.Count; i++) 
                x *= _items[i].GetValue(position);

            return x;
        }
    }
}