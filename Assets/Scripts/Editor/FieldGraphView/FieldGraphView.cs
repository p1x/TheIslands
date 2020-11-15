using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheIslands.Editor.FieldGraphView {
    public class FieldGraphView : GraphView {
        private Blackboard _blackboard;
        
        public FieldGraphView() {
            styleSheets.Add(Resources.Load<StyleSheet>("FieldGraphView"));
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var background = new GridBackground();
            background.StretchToParentSize();
            Insert(0, background);

            _blackboard = new Blackboard(this);
            
            ClearNodes();
        }

        public override Blackboard GetBlackboard() {
            return _blackboard;
        }

        public void ClearNodes() {
            DeleteElements(graphElements.ToList());
            
            //AddElement(CreateEntryPointNode());
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            var result = new List<Port>();
            ports.ForEach(p => {
                if (p == startPort || p.direction == startPort.direction || p.node == startPort.node)
                    return;

                var inputPort = p.direction == Direction.Input ? p : startPort;
                var outputPort = p.direction == Direction.Input ? startPort : p;

                if (inputPort.portType.IsAssignableFrom(outputPort.portType)) result.Add(p);
            });
            return result;
        }
    }
}