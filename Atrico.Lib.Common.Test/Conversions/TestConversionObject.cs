using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.Conversions
{
	[TestFixture]
	public class TestConversionObject : TestFixtureBase
	{
		[Test]
		public void TestIdentity()
		{
			// Arrange
			var value = RandomValues.String();

			// Act
			var converted = Conversion.Convert<string>(value);

			// Assert
			Assert.That(Value.Of(converted).Is().TypeOf(typeof(string)), "Type");
			Assert.That(Value.Of(converted).Is().ReferenceEqualTo(value), "Value");
		}

		[Test]
		public void TestNull()
		{
			// Arrange

			// Act
			var converted = Conversion.Convert<string>(null);

			// Assert
			Assert.That(Value.Of(converted).Is().Null(), "Value");
		}
	}
}
