using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheIslands.Core {
    [Serializable]
    public abstract class CompositeFieldBase : ScalarField, IEnumerable {
        [SerializeReference]
        protected List<ScalarField> _items; 
        public List<ScalarField> Items => _items ?? (_items = new List<ScalarField>());
        
        public void Add(ScalarField field) => Items.Add(field);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}