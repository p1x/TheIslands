using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class FieldsTests {
        // We use cubic interpolation between R-F and R+F distances, and max value of 1. 
        [TestCase(0f,  0f,  0f,  1f)]
        [TestCase(5f,  0f,  0f,  0.84375f)]
        [TestCase(10f, 0f,  0f,  0.5f)]
        [TestCase(15f, 0f,  0f,  0.15625f)]
        [TestCase(20f, 0f,  0f,  0.0f)]
        [TestCase(0f,  15f, 0f,  0.15625f)]
        [TestCase(0f,  0f,  15f, 0.15625f)]
        public void SphereFieldTests(float x, float y, float z, float expectedResult) {
            var field = new SphereField();
            field.Radius = 10;
            field.Falloff = 10;
            field.Center          = Vector3.zero;

            var actualResult = field.GetValue(new Vector3(x, y, z));

            Assert.AreEqual(expectedResult, actualResult, 0.00001);
        }

        [TestCase(0f,  0f, 0f, 10f, 1f)]
        [TestCase(10f, 0f, 0f, 5f,  0.15625f)]
        [TestCase(10f, 0f, 0f, 10f, 0.5f)]
        [TestCase(10f, 0f, 0f, 15f, 0.84375f)]
        public void CustomPropertiesSphereFieldTests(float x, float y, float z, float r, float expectedResult) {
            var field = new SphereField();
            field.Radius = r;
            field.Falloff = 10;
            field.Center          = new Vector3(x, y, z);

            var actualResult = field.GetValue(Vector3.zero);

            Assert.AreEqual(expectedResult, actualResult, 0.00001);
        }
        
        /*
         *  We use this to compose fields:
         *  v1 + v2
         *  ----------- = vx 
         *  1 + v1 * v2
         */
        [TestCase(0f,  0f, 0f, 1f)]
        [TestCase(5f,  0f, 0f, 0.88352f)]
        [TestCase(10f, 0f, 0f, 0.8f)]
        [TestCase(15f, 0f, 0f, 0.88352f)]
        [TestCase(20f, 0f, 0f, 1f)]
        public void TwoSphereCompositeFieldTests(float x, float y, float z, float expectedResult) {
            var field   = new CompositeField {
                new SphereField { Radius = 10, Falloff = 10, Center = new Vector3(0,  0, 0) },
                new SphereField { Radius = 10, Falloff = 10, Center = new Vector3(20, 0, 0) }
            };

            var actualResult = field.GetValue(new Vector3(x, y, z));

            Assert.AreEqual(expectedResult, actualResult, 0.00001);
        }
    }
}