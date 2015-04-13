using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

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
			Assert.That(converted, Is.TypeOf<T>(), "Type");
			Assert.That(converted, Is.EqualTo(value), "Value");
		}

		[Test]
		public void TestNull()
		{
			// Arrange

			// Act
			var ex = Catch.Exception(() => Conversion.Convert<T>(null));

			// Assert
			Assert.That(ex, Is.Not.Null);
			Debug.WriteLine(ex.Message);
		}
	}
}
