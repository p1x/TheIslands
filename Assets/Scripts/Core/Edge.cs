using System;
using UnityEngine;

namespace TheIslands.Core {
    public readonly struct Edge : IEquatable<Edge> {
        public Edge(in Vector3 p0, in Vector3 p1) {
            P0 = p0;
            P1 = p1;
        }

        public bool Equals(Edge other) =>
            P0.Equals(other.P0) && P1.Equals(other.P1) ||
            P0.Equals(other.P1) && P1.Equals(other.P0);
        public override bool Equals(object obj) => obj is Edge other && Equals(other);
        public override int GetHashCode() {
            // Simple xor to represent independence of order. Against all best practices.
            return P0.GetHashCode() ^ P1.GetHashCode();
        }

        public static bool operator ==(Edge left, Edge right) => left.Equals(right);
        public static bool operator !=(Edge left, Edge right) => !left.Equals(right);

        public Vector3 P0 { get; }
        public Vector3 P1 { get; }
    }
}