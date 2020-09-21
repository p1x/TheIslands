using System.Collections.Generic;
using UnityEngine;

namespace TheIslands.Core {
    public class CompositeField : ScalarField {
        public List<ScalarField> items = new List<ScalarField>();
        
        public override float GetValue(Vector3 position) {

            /*
             *  v1 + v2
             *  ----------- = vx
             *  1 + v1 * v2
             */
            
            var x0 = 0f;
            for (var i = 0; i < items.Count; i++) {
                var x1 = items[i].GetValue(position);
                x0 = (x0 + x1) / (1 + x0 * x1);
            }
            return x0;
        }
    }
}