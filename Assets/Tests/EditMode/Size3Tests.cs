using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;
using AssertionException = UnityEngine.Assertions.AssertionException;

namespace TheIslands.Tests.EditMode {
    public class Size3Tests {
        [Test]
        public void Constructor_PositiveValues_CreatesSizeWithSameValues() {
            var x = 1f;
            var y = 2f;
            var z = 3f;
            
            var actual = new Size3(x, y, z);

            Assert.AreEqual(x, actual.X);
            Assert.AreEqual(y, actual.Y);
            Assert.AreEqual(z, actual.Z);
        }
        
        [Test]
        public void Constructor_PositiveVector_CreatesSizeWithSameValues() {
            var vector = new Vector3(1, 2, 3);
            
            var actual = new Size3(vector);
            
            Assert.AreEqual(vector.x, actual.X);
            Assert.AreEqual(vector.y, actual.Y);
            Assert.AreEqual(vector.z, actual.Z);
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Constructor_NegativeValues_ThrowsException() {
            Assert.Throws<AssertionException>(() => new Size3(-1, 2, 3));
            Assert.Throws<AssertionException>(() => new Size3(1, -2, 3));
            Assert.Throws<AssertionException>(() => new Size3(1, 2, -3));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Constructor_VectorWithNegativeValues_ThrowsException() {
            Assert.Throws<AssertionException>(() => new Size3(new Vector3(-1, 2, 3)));
            Assert.Throws<AssertionException>(() => new Size3(new Vector3(1, -2, 3)));
            Assert.Throws<AssertionException>(() => new Size3(new Vector3(1, 2, -3)));
        }
    }
}