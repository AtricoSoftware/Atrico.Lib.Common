using Atrico.Lib.Assertions;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Conversions
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
			Assert.That(converted, Is.TypeOf<string>(), "Type");
			Assert.That(converted, Is.ReferenceEqualTo(value), "Value");
		}

		[Test]
		public void TestNull()
		{
			// Arrange

			// Act
			var converted = Conversion.Convert<string>(null);

			// Assert
			Assert.That(converted, Is.Null, "Value");
		}
	}
}
