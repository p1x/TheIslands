using System;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class CompositeField : CompositeFieldBase {
        public override float GetValue(Vector3 position) {
            if (_items == null) // Empty and not initialized
                return 0;
            
            /*
             *  v1 + v2
             *  ----------- = vx 
             *  1 + v1 * v2
             */
            
            var x0 = 0f;
            for (var i = 0; i < _items.Count; i++) {
                var x1 = _items[i].GetValue(position);
                x0 = (x0 + x1) / (1 + x0 * x1);
            }
            return x0;
        }
    }
}