using UnityEngine;
using UnityEngine.Rendering;

namespace TheIslands.Procedural {
    public readonly struct Vertex { 
        public static readonly VertexAttributeDescriptor[] Attributes = {
            new VertexAttributeDescriptor(VertexAttribute.Position),
            new VertexAttributeDescriptor(VertexAttribute.Normal),
        };
        
        private readonly Vector3 _position;
        private readonly Vector3 _normal;
        public Vertex(Vector3 position, Vector3 normal) {
            _position = position;
            _normal   = normal;
        }

        public override string ToString() => $"{_position}";
    }
}