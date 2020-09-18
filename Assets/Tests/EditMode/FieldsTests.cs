using NUnit.Framework;
using TheIslands.Core;
using UnityEngine;

namespace TheIslands.Tests.EditMode {
    public class FieldsTests {
        [TestCase(0f, 0f, 0f, ExpectedResult = 1f)]
        [TestCase(1f, 0f, 0f, ExpectedResult = 0.5f)]
        [TestCase(0f, 1f, 0f, ExpectedResult = 0.5f)]
        [TestCase(0f, 0f, 1f, ExpectedResult = 0.5f)]
        [TestCase(2f, 0f, 0f, ExpectedResult = 0f)]
        [TestCase(0f, 2f, 0f, ExpectedResult = 0f)]
        [TestCase(0f, 0f, 2f, ExpectedResult = 0f)]
        public float SphereFieldTests(float x, float y, float z) {
            var field = new SphereField {
                HalfValueRadius = 1,
                Center          = new Vector3(0, 0, 0)
            };

            return field.GetValue(new Vector3(x, y, z));
        }

        [TestCase(0f, 0f, 0f, ExpectedResult = 0.5f)]
        [TestCase(2f, 0f, 0f, ExpectedResult = 0.5f)]
        [TestCase(1f, 0f, 0f, ExpectedResult = 0.5f)]
        public float TwoSphereCompositeFieldTests(float x, float y, float z) {
            var field = new CompositeField {
                new SphereField {
                    HalfValueRadius = 1,
                    Center          = new Vector3(0, 0, 0)
                },
                new SphereField {
                    HalfValueRadius = 1,
                    Center          = new Vector3(2, 0, 0)
                }
            };

            return field.GetValue(new Vector3(x, y, z));
        }
    }
}