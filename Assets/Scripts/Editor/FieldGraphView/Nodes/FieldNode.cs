using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TheIslands.Core;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheIslands.Editor.FieldGraphView {
    public abstract class FieldNode : Node, IDisposable {
        public static readonly Vector2Int PreviewSize = new Vector2Int(128, 128);
        private readonly Dictionary<Port, PropertyInfo> _inputSetters = new Dictionary<Port, PropertyInfo>();

        private readonly Box _propertyBox;
        private Texture2D _previewTexture;
        
        protected FieldNode() {
            Output = new DataSampler<Vector2>(GetValue);
            var outputPort = InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Multi,
                typeof(IDataSampler<Vector2, float>)
            );
            outputPort.portName = nameof(Output);
            outputContainer.Add(outputPort);
            
            _propertyBox = new Box();
            extensionContainer.Add(_propertyBox);
        }

        public abstract float GetValue(Vector2 p);

        public void SetInputData(Port inputPort, object newData) {
            var propertyInfo = _inputSetters[inputPort];

            var oldData = propertyInfo.GetValue(this);
            if (oldData is IInvalidatableSampler oldSampler) 
                oldSampler.Invalidated -= SamplerInvalidated;

            propertyInfo.SetValue(this, newData);
            if (newData is IInvalidatableSampler newSampler)
                newSampler.Invalidated += SamplerInvalidated;
            
            UpdatePreview();
        }

        private void SamplerInvalidated(object sender, EventArgs eventArgs) => Update();

        protected void Update() {
            Output.Invalidate();
            UpdatePreview();
        }

        public void UpdatePreview() {
            if (_previewTexture == null)
                return;
            
            var texture = _previewTexture;
            for (var i = 0; i < PreviewSize.x; i++)
            for (var j = 0; j < PreviewSize.y; j++) {
                var value = GetValue(new Vector2(i, j));
                texture.SetPixel(i, j, new Color(value, value, value));
            }

            texture.Apply();
        }

        protected void AddInput<TNode, TInput>(Expression<Func<TNode, TInput>> expression) {
            var propertyInfo = Utils.GetPropertyInfo(expression);
            
            var inputPort = InstantiatePort(
                Orientation.Horizontal, 
                Direction.Input, 
                Port.Capacity.Single,
                typeof(TInput)
            );
            inputPort.portName = propertyInfo.Name;
            inputContainer.Add(inputPort);
            
            _inputSetters.Add(inputPort, propertyInfo);
        }

        protected void AddFloatField<TNode>(Expression<Func<TNode, float>> expression) where TNode : FieldNode => 
            AddTextField<TNode, FloatField, float>(expression);

        protected void AddIntegerField<TNode>(Expression<Func<TNode, int>> expression) where TNode : FieldNode => 
            AddTextField<TNode, IntegerField, int>(expression);

        private void AddTextField<TNode, TField, TValue>(Expression<Func<TNode, TValue>> expression) where TField : TextValueField<TValue>, new() {
            var propertyInfo = Utils.GetPropertyInfo(expression);
            var propertyName = propertyInfo.Name;
            var getValue = new Func<TValue>(() => (TValue)propertyInfo.GetValue(this));
            var setValue = new Action<TValue>(v => propertyInfo.SetValue(this, v));

            var field = new TField();
            field.labelElement.style.minWidth = new StyleLength(10);
            field.label = propertyName;
            field.value = getValue();
            field.RegisterValueChangedCallback(e => setValue(e.newValue));
            
            _propertyBox.Add(field);
        } 
        
        protected Texture2D AddPreview() {
            var image = new Image();
            image.scaleMode = ScaleMode.ScaleToFit;
            image.image = _previewTexture = new Texture2D(PreviewSize.x, PreviewSize.y);
            _propertyBox.Add(image);

            UpdatePreview();
            
            return _previewTexture;
        }

        public void Dispose() {
            foreach (var propertyInfo in _inputSetters.Values) {
                var value = propertyInfo.GetValue(this);
                if (value is IInvalidatableSampler sampler) 
                    sampler.Invalidated -= SamplerInvalidated;
            }
        }

        protected void RaisePropertyChanged() => Update();

        protected void SetIfChanged<T>(ref T field, in T value) where T : struct, IEquatable<T> =>
            Utils.RiseAndSetIfChanged(ref field, in value, RaisePropertyChanged);
        
        public IDataSampler<Vector2, float> Output { get; }
    }
}