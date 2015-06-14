using Atrico.Lib.Assertions;
using Atrico.Lib.Common.RegEx;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.RegEx
{
    [TestFixture]
    public class TestRegExChar
    {
        [Test]
        public void TestSingleDigit([Values('0', '5', '9')]char digit)
        {
            // Arrange

            // Act
            var element = RegExElement.Create(digit);

            // Assert
            Assert.That(Value.Of(TODO).Is().Null(), "TODO");
        }
    }
}
