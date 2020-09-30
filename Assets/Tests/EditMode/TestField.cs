using System.Linq;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class TestField : IScalarField {
        private static readonly Vector3[] Corners = {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
        };
        
        private readonly float[] _data;

        public TestField(Plane plane) {
            _data = new float[8];

            for (var i = 0; i < 8; i++) {
                var cornerPoint = Corners[i];
                var distance = plane.GetDistanceToPoint(cornerPoint);
                var value = (Mathf.Sign(distance) + 1) / 2;
                _data[i] = value;
            }
        }
        
        public float GetValue(Vector3 position) {
            var (_, index) = Corners
                .Select((c, i) => (c, i))
                .WithMin(t => Vector3.Distance(t.c, position));
            return _data[index];
        }
    }
}