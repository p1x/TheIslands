using UnityEngine;

namespace TheIslands.Core {
    public interface IMeshBuilder {
        void AddTriangle(Triangle triangle);
        void AddTriangle(Vector3 p0, Vector3 p1, Vector3 p2);
    }
}