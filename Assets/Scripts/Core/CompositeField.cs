using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public class CompositeField : ScalarField, IEnumerable {
        [SerializeReference]
        private List<ScalarField> _items; 
        public List<ScalarField> Items => _items ?? (_items = new List<ScalarField>());

        public override float GetValue(Vector3 position) {

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

        public void Add(ScalarField field) => Items.Add(field);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}