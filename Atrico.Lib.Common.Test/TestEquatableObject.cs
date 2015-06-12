using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test
{
    [TestFixture]
    public class TestEquatableObject : TestFixtureBase
    {
        private const int _pivot = 10;

        private class TestObject : EquatableObject<TestObject>
        {
            private readonly int _value;

            public TestObject(int value)
            {
                _value = value;
            }

            protected override int GetHashCodeImpl()
            {
                return _value.GetHashCode();
            }

            protected override bool EqualsImpl(TestObject other)
            {
                return _value.Equals(other._value);
            }
        }

        private class TestObjectDerived : TestObject
        {
            public TestObjectDerived(int value)
                : base(value)
            {
            }
        }

        [Test]
        public void TestEquals([Values(1, 10, 100)] int val)
        {
            // Arrange
            var obj1 = new TestObject(_pivot);
            var obj2 = new TestObject(val);
            var expected = _pivot.Equals(val);
            Debug.WriteLine("{0} equals {1} = {2}", _pivot, val, expected);

            // Act
            var result = obj1.Equals(obj2);

            // Assert
            Assert.That(Value.Of(result).Is().EqualTo(expected));
        }

        [Test]
        public void TestEqualsWithNull()
        {
            // Arrange
            var val = RandomValues.Integer();
            var obj1 = new TestObject(val);
            const bool expected = false; // Null never equals
            Debug.WriteLine("{0} equals NULL = {1}", val, expected);

            // Act
            var result = obj1.Equals(null);

            // Assert
            Assert.That(Value.Of(result).Is().EqualTo(expected));
        }

        [Test]
        public void TestEqualsWithOtherType([Values(1, 10, 100)] int val)
        {
            // Arrange
            var obj1 = new TestObject(_pivot);
            var obj2 = new TestObjectDerived(val);
            const bool expected = false; // Different type never equals
            Debug.WriteLine("{0} equals {1} = {2}", obj1, obj2, expected);

            // Act
            var result = obj1.Equals(obj2);

            // Assert
            Assert.That(Value.Of(result).Is().EqualTo(expected));
        }
    }
}