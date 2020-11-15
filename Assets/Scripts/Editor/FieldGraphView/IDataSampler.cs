using System;

namespace TheIslands.Editor.FieldGraphView {
    public interface IDataSampler<in TParameter, out TResult> : IInvalidatableSampler {
        TResult GetValue(TParameter parameter);
    }

    public interface IInvalidatableSampler {
        void Invalidate();

        event EventHandler Invalidated;
    }
}