using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.PropertyContainer
{
    [TestFixture]
    public class TestPropertyContainerEquality : TestFixtureBase
    {
        [Test]
        public void TestEmptyEquals()
        {
            // Arrange
            var props1 = new Common.PropertyContainer.PropertyContainer(this);
            var props2 = new Common.PropertyContainer.PropertyContainer(this);

            // Act
            var hash1 = props1.GetHashCode();
            var hash2 = props2.GetHashCode();
            var equals12 = props1.Equals(props2);
            var equals21 = props2.Equals(props1);

            // Assert
            Assert.That(Value.Of(hash1).Is().EqualTo(hash2), "Hash codes equal");
            Assert.That(Value.Of(equals12).Is().True(), "Equals 1-2");
            Assert.That(Value.Of(equals21).Is().True(), "Equals 2-1");
        }

        [Test]
        public void TestSameElementsEquals()
        {
            // Arrange
            var name1 = RandomValues.String();
            var value1 = RandomValues.Integer();
            var name2 = RandomValues.String();
            var value2 = RandomValues.String();
            var name3 = RandomValues.String();
            var value3 = RandomValues.Value<bool>();
            var props1 = new Common.PropertyContainer.PropertyContainer(this);
            props1.Set(value1, name1);
            props1.Set(value2, name2);
            props1.Set(value3, name3);
            var props2 = new Common.PropertyContainer.PropertyContainer(this);
            props2.Set(value1, name1);
            props2.Set(value2, name2);
            props2.Set(value3, name3);

            // Act
            var hash1 = props1.GetHashCode();
            var hash2 = props2.GetHashCode();
            var equals12 = props1.Equals(props2);
            var equals21 = props2.Equals(props1);

            // Assert
            Assert.That(Value.Of(hash1).Is().EqualTo(hash2), "Hash codes equal");
            Assert.That(Value.Of(equals12).Is().True(), "Equals 1-2");
            Assert.That(Value.Of(equals21).Is().True(), "Equals 2-1");
        }

        [Test]
        public void TestDefaultElementsEquals()
        {
            // Arrange
            var name1 = RandomValues.String();
            const int value1 = default(int);
            var name2 = RandomValues.String();
            const string value2 = null;
            var name3 = RandomValues.String();
            const double value3 = default(double);
            var props1 = new Common.PropertyContainer.PropertyContainer(this);
            props1.Set(value1, name1);
            props1.Set(value2, name2);
            props1.Set(value3, name3);
            var props2 = new Common.PropertyContainer.PropertyContainer(this);

            // Act
            var hash1 = props1.GetHashCode();
            var hash2 = props2.GetHashCode();
            var equals12 = props1.Equals(props2);
            var equals21 = props2.Equals(props1);

            // Assert
            Assert.That(Value.Of(hash1).Is().EqualTo(hash2), "Hash codes equal");
            Assert.That(Value.Of(equals12).Is().True(), "Equals 1-2");
            Assert.That(Value.Of(equals21).Is().True(), "Equals 2-1");
        }

        [Test]
        public void TestDifferentElementsNotEquals()
        {
            // Arrange
            var name1 = RandomValues.String();
            var value1a = RandomValues.Integer();
            var value1b = value1a + 1;
            var name2 = RandomValues.String();
            var value2 = RandomValues.String();
            var name3 = RandomValues.String();
            var value3 = RandomValues.Value<bool>();
            var props1 = new Common.PropertyContainer.PropertyContainer(this);
            props1.Set(value1a, name1);
            props1.Set(value2, name2);
            props1.Set(value3, name3);
            var props2 = new Common.PropertyContainer.PropertyContainer(this);
            props2.Set(value1b, name1);
            props2.Set(value2, name2);
            props2.Set(value3, name3);

            // Act
            var equals12 = props1.Equals(props2);
            var equals21 = props2.Equals(props1);

            // Assert
            Assert.That(Value.Of(equals12).Is().False(), "Equals 1-2");
            Assert.That(Value.Of(equals21).Is().False(), "Equals 2-1");
        }

        [Test]
        public void TestExtraElementNotEquals()
        {
            // Arrange
            var name1 = RandomValues.String();
            var value1 = RandomValues.Integer();
            var name2 = RandomValues.String();
            var value2 = RandomValues.String();
            var name3 = RandomValues.String();
            var value3 = RandomValues.Value<double>();
            var props1 = new Common.PropertyContainer.PropertyContainer(this);
            props1.Set(value1, name1);
            props1.Set(value2, name2);
            props1.Set(value3, name3);
            var props2 = new Common.PropertyContainer.PropertyContainer(this);
            props2.Set(value1, name1);
            props2.Set(value2, name2);

            // Act
            var equals12 = props1.Equals(props2);
            var equals21 = props2.Equals(props1);

            // Assert
            Assert.That(Value.Of(equals12).Is().False(), "Equals 1-2");
            Assert.That(Value.Of(equals21).Is().False(), "Equals 2-1");
        }
    }
}