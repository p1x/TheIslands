using UnityEngine;
using UnityEngine.Assertions;

namespace TheIslands.Core {
    public readonly struct Size3 {
        private readonly Vector3 _vector;

        public Size3(in float x, in float y, in float z) : this(new Vector3(x, y, z)) { }
        public Size3(in Vector3 vector) {
            Assert.IsTrue(vector.x >= 0 && vector.y >= 0 && vector.z >= 0, "vector.x >= 0 && vector.y >= 0 && vector.z >= 0");
            _vector = vector;
        }

        public static implicit operator Vector3(in Size3 a) => a._vector;

        public float X => _vector.x;
        public float Y => _vector.y;
        public float Z => _vector.z;
    }
}