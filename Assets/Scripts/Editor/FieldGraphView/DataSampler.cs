using System;

namespace TheIslands.Editor.FieldGraphView {
    public class DataSampler<T> : IDataSampler<T, float> {
        private readonly Func<T, float> _getValue;
        
        public DataSampler(Func<T, float> getValue) => _getValue = getValue;

        public float GetValue(T parameter) => _getValue(parameter);
        
        public void Invalidate() => OnInvalidated();

        public event EventHandler Invalidated;
        private void OnInvalidated() => Invalidated?.Invoke(this, EventArgs.Empty);
    }
}