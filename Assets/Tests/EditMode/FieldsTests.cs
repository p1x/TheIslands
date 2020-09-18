using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class FieldsTests {
        /*
         * we want something like 1/x, but with max value
         * so we go for a/(x + b)
         *  values:
         *  x  y 
         *  0  h
         *  r  0.5
         *
         *  some math:
         *  a / (b + x) = y
         * 
         *  a / (b + 0)  = h
         *  a / (b + r)  = 0.5
         *
         *  a = h * b
         *  h * b = 0.5 * b + 0.5 * r
         *  (h - 0.5) * b = 0.5 * r
         *  b = 0.5 * r / (h - 0.5)
         *  b = r / (2 * h - 1)
         *  a = h * r / (2 * h - 1)
         *
         *  h * b / (b + x) = y
         *  h / (1 + x / b) = y
         * 
         *  h / (1 + x * (2 * h - 1) / r) = y
         * 
         *  for h = 10 and r = 10:
         *  10 / (1 + x * 1.9) = y
         *  
         */
        [TestCase(0f,  0f,  0f,  10f)]
        [TestCase(5f,  0f,  0f,  0.95238f)]
        [TestCase(10f, 0f,  0f,  0.5f)]
        [TestCase(15f, 0f,  0f,  0.33898f)]
        [TestCase(20f, 0f,  0f,  0.25641f)]
        [TestCase(0f,  15f, 0f,  0.33898f)]
        [TestCase(0f,  0f,  15f, 0.33898f)]
        public void SphereFieldTests(float x, float y, float z, float expectedResult) {
            var field = new SphereField();

            var actualResult = field.GetValue(new Vector3(x, y, z));

            Assert.AreEqual(expectedResult, actualResult, 0.00001);
        }

        [TestCase(0f,  0f, 0f, 10f, 10f)]
        [TestCase(10f, 0f, 0f, 5f,  0.25641f)]
        [TestCase(10f, 0f, 0f, 10f, 0.5f)]
        [TestCase(10f, 0f, 0f, 15f, 0.73170f)]
        public void CustomPropertiesSphereFieldTests(float x, float y, float z, float r, float expectedResult) {
            var field = new SphereField {
                HalfValueRadius = r,
                Center = new Vector3(x, y, z)
            };

            var actualResult = field.GetValue(Vector3.zero);

            Assert.AreEqual(expectedResult, actualResult, 0.00001);
        }
        
        [TestCase(0f,  0f, 0f, 5.12821f)]
        [TestCase(5f,  0f, 0f, 0.64568f)]
        [TestCase(10f, 0f, 0f, 0.5f)]
        [TestCase(15f, 0f, 0f, 0.64568f)]
        [TestCase(20f, 0f, 0f, 5.12821f)]
        public void TwoSphereCompositeFieldTests(float x, float y, float z, float expectedResult) {
            var field = new CompositeField {
                new SphereField { Center = new Vector3(0,  0, 0) },
                new SphereField { Center = new Vector3(20, 0, 0) }
            };

            var actualResult = field.GetValue(new Vector3(x, y, z));

            Assert.AreEqual(expectedResult, actualResult, 0.00001);
        }
    }
}