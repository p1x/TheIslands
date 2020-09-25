using UnityEngine;
using UnityEngine.Assertions;

namespace TheIslands.Core
{
    public readonly struct Size3Int {
        private readonly Vector3Int _vector;

        public Size3Int(in int x, in int y, in int z) : this(new Vector3Int(x, y, z)) { }
        public Size3Int(in Vector3Int vector) {
            Assert.IsTrue(vector.x >= 0 && vector.y >= 0 && vector.z >= 0, "vector.x >= 0 && vector.y >= 0 && vector.z >= 0");
            _vector = vector;
        }
        
        public static implicit operator Vector3Int(in Size3Int a) => a._vector;
        
        public int X => _vector.x;
        public int Y => _vector.y;
        public int Z => _vector.z;
    }
}