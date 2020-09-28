using System.Collections.Generic;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class TestMeshBuilder : IMeshBuilder {
        public List<Triangle> Triangles { get; } = new List<Triangle>();

        public void AddTriangle(Triangle triangle) => Triangles.Add(triangle);

        public void AddTriangle(Vector3 p0, Vector3 p1, Vector3 p2) => AddTriangle(new Triangle(p0, p1, p2));
    }
}