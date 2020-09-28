using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheIslands.Core {
    public readonly struct Triangle : IEquatable<Triangle> {
        public Triangle(in Vector3 p0, in Vector3 p1, in Vector3 p2) {
            P0 = p0;
            P1 = p1;
            P2 = p2;
        }
        
        public Vector3 P0 { get; }
        public Vector3 P1 { get; }
        public Vector3 P2 { get; }

        public Edge GetEdge(int i) {
            switch (i) {
                case 0: return new Edge(P0, P1);
                case 1: return new Edge(P1, P2);
                case 2: return new Edge(P2, P0);
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public IEnumerable<Edge> GetEdges() {
            yield return new Edge(P0, P1);
            yield return new Edge(P1, P2);
            yield return new Edge(P2, P0);
        }
        
        public bool Equals(Triangle other) => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);
        public override bool Equals(object obj) => obj is Triangle other && Equals(other);

        public override int GetHashCode() {
            unchecked {
                var hashCode = P0.GetHashCode();
                hashCode = (hashCode * 397) ^ P1.GetHashCode();
                hashCode = (hashCode * 397) ^ P2.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Triangle left, Triangle right) => left.Equals(right);
        public static bool operator !=(Triangle left, Triangle right) => !left.Equals(right);
    }
}