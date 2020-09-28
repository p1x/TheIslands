using System;
using UnityEngine;

namespace TheIslands.Core {
    public readonly struct Triangle {
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
    }
}