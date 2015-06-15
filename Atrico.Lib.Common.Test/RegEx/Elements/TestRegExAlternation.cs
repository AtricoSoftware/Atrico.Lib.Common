using System.Diagnostics;
using System.Text.RegularExpressions;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.RegEx.Elements
{
    [TestFixture]
    public class TestRegExAlternation
    {
        [Test]
        public void TestMultipleDigit([Values('0', '5', '9')] char digit1, [Values('2', '7')] char digit2)
        {
            // Arrange
            var regex = string.Format("(?:{0}|{1})", digit1, digit2);

            // Act
            var element = RegExElement.CreateOr(RegExElement.Create(digit1), RegExElement.Create(digit2));

            // Assert
            Debug.WriteLine(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string");
            var result1 = new Regex(element.ToString()).Match(digit1.ToString());
            Assert.That(Value.Of(result1.Success).Is().True(), "Regex match (digit1)");
            Assert.That(Value.Of(result1.Groups.Count).Is().EqualTo(1), "Single group (digit1)");
            Assert.That(Value.Of(result1.Groups[0].Value).Is().EqualTo(digit1.ToString()), "Matches whole input (digit1)");
            var result2 = new Regex(element.ToString()).Match(digit2.ToString());
            Assert.That(Value.Of(result2.Success).Is().True(), "Regex match (digit2)");
            Assert.That(Value.Of(result2.Groups.Count).Is().EqualTo(1), "Single group (digit2)");
            Assert.That(Value.Of(result2.Groups[0].Value).Is().EqualTo(digit2.ToString()), "Matches whole input (digit2)");
        }
    }
}