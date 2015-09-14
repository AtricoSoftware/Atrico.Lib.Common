using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.PropertyContainer
{
    [TestFixture]
    public class TestPropertyContainerGetSet<T> : TestPODTypes<T>
    {
        [Test]
        public void TestGetDefault()
        {
            var name = RandomValues.String();
            var expected = default(T);

            // Arrange
            var props = new Common.PropertyContainer.PropertyContainer(null);

            // Act
            var value = props.Get<T>(name);

            // Assert
            if (typeof(T) != typeof(string))
            {
                Assert.That(Value.Of(value).Is().EqualTo(expected), "Get default value");
            }
            else
            {
                Assert.That(Value.Of(value as string).Is().Null(), "Get default value (string)");
            }
        }

        [Test]
        public void TestSetAndGet()
        {
            var name = RandomValues.String();
            var expected = RandomValues.Value<T>();

            // Arrange
            var props = new Common.PropertyContainer.PropertyContainer(null);
            props.Set(expected, name);

            // Act
            var value = props.Get<T>(name);

            // Assert
            Assert.That(Value.Of(value).Is().EqualTo(expected), "Get value");
        }

        [Test]
        public void TestOverwriteAndGet()
        {
            var name = RandomValues.String();
            var expected = RandomValues.Value<T>();

            // Arrange
            var props = new Common.PropertyContainer.PropertyContainer(null);
            props.Set(RandomValues.Value<T>(), name);
            props.Set(expected, name);

            // Act
            var value = props.Get<T>(name);

            // Assert
            Assert.That(Value.Of(value).Is().EqualTo(expected), "Get value");
        }
    }
}