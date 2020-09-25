using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;
using AssertionException = UnityEngine.Assertions.AssertionException;

namespace TheIslands.Tests.EditMode {
    public class VectorExtensionTests {
        [Test]
        public void AsSize_PositiveVector_ReturnsSizeWithSameValues() {
            var vector = new Vector3(1, 2, 3);

            var actual = vector.AsSize();
            
            Assert.AreEqual(vector.x, actual.X);
            Assert.AreEqual(vector.y, actual.Y);
            Assert.AreEqual(vector.z, actual.Z);
        }
        
        [Test]
        public void AsSize_VectorWithNegativeValues_ThrowsException() {
            Assert.Throws<AssertionException>(() => new Vector3(-1, 2, 3).AsSize());
            Assert.Throws<AssertionException>(() => new Vector3(1, -2, 3).AsSize());
            Assert.Throws<AssertionException>(() => new Vector3(1, 2, -3).AsSize());
        }
        
        [Test]
        public void ToSize_PositiveVector_ReturnsSizeWithSameValues() {
            var vector = new Vector3(1, 2, 3);

            var actual = vector.ToSize();
            
            Assert.AreEqual(vector.x, actual.X);
            Assert.AreEqual(vector.y, actual.Y);
            Assert.AreEqual(vector.z, actual.Z);
        }

        [Test]
        public void ToSize_VectorWithNegativeValues_CreateSizeWithSameOrZeroValues() {
            void TestVector(Vector3 input, Vector3 expected) {
                var actual = input.ToSize();

                Assert.AreEqual(expected.x, actual.X);
                Assert.AreEqual(expected.y, actual.Y);
                Assert.AreEqual(expected.z, actual.Z);
            }

            TestVector(new Vector3(-1, 2, 3), new Vector3(0, 2, 3));
            TestVector(new Vector3(1, -2, 3), new Vector3(1, 0, 3));
            TestVector(new Vector3(1, 2, -3), new Vector3(1, 2, 0));
        }
        
        [Test]
        public void AsSize_PositiveVectorInt_ReturnsSizeWithSameValues() {
            var vector = new Vector3Int(1, 2, 3);

            var actual = vector.AsSize();
            
            Assert.AreEqual(vector.x, actual.X);
            Assert.AreEqual(vector.y, actual.Y);
            Assert.AreEqual(vector.z, actual.Z);
        }
        
        [Test]
        public void AsSize_VectorIntWithNegativeValues_ThrowsException() {
            Assert.Throws<AssertionException>(() => new Vector3Int(-1, 2, 3).AsSize());
            Assert.Throws<AssertionException>(() => new Vector3Int(1, -2, 3).AsSize());
            Assert.Throws<AssertionException>(() => new Vector3Int(1, 2, -3).AsSize());
        }
        
        [Test]
        public void ToSize_PositiveVectorInt_ReturnsSizeWithSameValues() {
            var vector = new Vector3Int(1, 2, 3);

            var actual = vector.ToSize();
            
            Assert.AreEqual(vector.x, actual.X);
            Assert.AreEqual(vector.y, actual.Y);
            Assert.AreEqual(vector.z, actual.Z);
        }

        [Test]
        public void ToSize_VectorIntWithNegativeValues_CreateSizeWithSameOrZeroValues() {
            void TestVector(Vector3Int input, Vector3Int expected) {
                var actual = input.ToSize();

                Assert.AreEqual(expected.x, actual.X);
                Assert.AreEqual(expected.y, actual.Y);
                Assert.AreEqual(expected.z, actual.Z);
            }

            TestVector(new Vector3Int(-1, 2, 3), new Vector3Int(0, 2, 3));
            TestVector(new Vector3Int(1, -2, 3), new Vector3Int(1, 0, 3));
            TestVector(new Vector3Int(1, 2, -3), new Vector3Int(1, 2, 0));
        }
    }
}