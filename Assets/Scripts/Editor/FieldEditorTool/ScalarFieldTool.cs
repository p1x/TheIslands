using TheIslands.Core;

namespace TheIslands.Editor.FieldEditorTool {
    public abstract class ScalarFieldTool<T> : ScalarFieldTool where T : IScalarField {
        public override void OnToolGUI(IScalarField field) => OnToolGUI((T)field);
        protected abstract void OnToolGUI(T field);
    }

    public abstract class ScalarFieldTool {
        public abstract void OnToolGUI(IScalarField field);
    }
}