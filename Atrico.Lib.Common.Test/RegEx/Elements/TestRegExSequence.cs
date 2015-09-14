using System.Text.RegularExpressions;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.RegEx.Elements
{
    [TestFixture]
    public class TestRegExSequence : RegExTestFixtureBase
    {
        [Test]
        public void TestMultipleDigit([Values('0', '5', '9')] char digit1, [Values('2', '7')] char digit2)
        {
            // Arrange
            var regex = string.Format("{0}{1}", digit1, digit2);

            // Act
            var element = RegExElement.CreateSequence(RegExElement.Create(digit1), RegExElement.Create(digit2));

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string");
            var result1 = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit1.ToString());
            Assert.That(Value.Of(result1.Success).Is().False(), "Regex match (digit1)");
            var result2 = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit2.ToString());
            Assert.That(Value.Of(result2.Success).Is().False(), "Regex match (digit2)");
            var textBoth = string.Format("{0}{1}", digit1, digit2);
            var resultBoth = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(textBoth);
            Assert.That(Value.Of(resultBoth.Success).Is().True(), "Regex match (both)");
            Assert.That(Value.Of(resultBoth.Groups.Count).Is().EqualTo(1), "Single group (both)");
            Assert.That(Value.Of(resultBoth.Groups[0].Value).Is().EqualTo(textBoth), "Matches whole input (both)");
            var textBothWrongOrder = string.Format("{0}{1}", digit2, digit1);
            var resultBothWrongOrder = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(textBothWrongOrder);
            Assert.That(Value.Of(resultBothWrongOrder.Success).Is().False(), "Regex match ( wrong order)");
        }
    }
}