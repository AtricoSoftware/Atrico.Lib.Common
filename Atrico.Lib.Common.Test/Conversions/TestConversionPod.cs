using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.Conversions
{
    [TestFixture(typeof (bool))]
    [TestFixture(typeof (char))]
    [TestFixture(typeof (byte))]
    [TestFixture(typeof (sbyte))]
    [TestFixture(typeof (short))]
    [TestFixture(typeof (ushort))]
    [TestFixture(typeof (int))]
    [TestFixture(typeof (uint))]
    [TestFixture(typeof (long))]
    [TestFixture(typeof (ulong))]
    [TestFixture(typeof (float))]
    [TestFixture(typeof (double))]
    public class TestConversionPod<T> : TestFixtureBase
    {
        [Test]
        public void TestIdentity()
        {
            // Arrange
            var value = RandomValues.Value<T>();

            // Act
            var converted = Conversion.Convert<T>(value);

            // Assert
            Assert.That(Value.Of(converted).Is().TypeOf(typeof (T)), "Type");
            Assert.That(Value.Of(converted).Is().EqualTo(value), "Value");
        }

        [Test]
        public void TestNull()
        {
            // Arrange

            // Act
            var ex = Catch.Exception(() => Conversion.Convert<T>(null));

            // Assert
            Assert.That(Value.Of(ex).Is().Not().Null());
            Debug.WriteLine(ex.Message);
        }
    }
}
