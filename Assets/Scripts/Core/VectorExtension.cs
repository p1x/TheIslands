using UnityEngine;

namespace TheIslands.Core
{
    public static class VectorExtension {
        public static Size3 AsSize(in this Vector3 vector) => new Size3(vector);
        public static Size3Int AsSize(in this Vector3Int vector) => new Size3Int(vector);

        public static Size3 ToSize(in this Vector3 vector) =>
            new Size3(Mathf.Max(vector.x, 0), Mathf.Max(vector.y, 0), Mathf.Max(vector.z, 0));
        public static Size3Int ToSize(in this Vector3Int vector) =>
            new Size3Int(Mathf.Max(vector.x, 0), Mathf.Max(vector.y, 0), Mathf.Max(vector.z, 0));
    }
}