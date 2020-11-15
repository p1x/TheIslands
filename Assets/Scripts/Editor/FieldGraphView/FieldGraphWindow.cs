using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheIslands.Editor.FieldGraphView {
    public class FieldGraphWindow : EditorWindow {
        private FieldGraphView _graphView;
        private Toolbar _toolbar;

        [MenuItem("Graph/Field Graph")]
        public static void OpenWindow() {
            var window = GetWindow<FieldGraphWindow>();
            window.titleContent = new GUIContent("Field Graph");
        }

        private void OnEnable() {
            _graphView = new FieldGraphView {
                name = "Field Graph"
            };
            _graphView.StretchToParentSize();
            _graphView.graphViewChanged = GraphViewChanged;
            rootVisualElement.Add(_graphView);
        
            _toolbar = new Toolbar();
            _toolbar.Add(new ToolbarButton(() => _graphView.ClearNodes()) { text = "Clear Graph" });

            var addMenu = new ToolbarMenu { text = "Add" };
            addMenu.menu.AppendAction(nameof(ConstantNode), a => ConstantNode.AddTo(_graphView));
            addMenu.menu.AppendAction(nameof(ScaleNode), a => ScaleNode.AddTo(_graphView));
            addMenu.menu.AppendAction(nameof(PerlinNoiseNode), a => PerlinNoiseNode.AddTo(_graphView));
            addMenu.menu.AppendAction(nameof(MultiplierNode), a => MultiplierNode.AddTo(_graphView));
            addMenu.menu.AppendAction(nameof(BorderFieldNode), a => BorderFieldNode.AddTo(_graphView));
            _toolbar.Add(addMenu);
            
            rootVisualElement.Add(_toolbar);
        }

        private GraphViewChange GraphViewChanged(GraphViewChange change) {
            if (change.edgesToCreate != null)
                foreach (var edge in change.edgesToCreate)
                    OnAddEdge(edge);

            if (change.elementsToRemove != null) {
                foreach (var edge in change.elementsToRemove.OfType<Edge>())
                    OnRemoveEdge(edge);

                foreach (var disposable in change.elementsToRemove.OfType<IDisposable>())
                    disposable.Dispose();
            }

            return change;
        }

        private void OnAddEdge(Edge edge) {
            var inputNode = (FieldNode)edge.input.node;
            var outputNode = (FieldNode)edge.output.node;

            inputNode.SetInputData(edge.input, outputNode.Output);
        }

        private void OnRemoveEdge(Edge edge) {
            var inputNode = (FieldNode)edge.input.node;
            inputNode.SetInputData(edge.input, null);
        }

        private void OnDisable() {
            if (_graphView != null)
                rootVisualElement.Remove(_graphView);
        }
    }
}