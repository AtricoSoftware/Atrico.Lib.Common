using System;
using System.Text.RegularExpressions;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.TestAttributes;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.RegEx.Elements
{
    [TestFixture]
    public class TestRegExChar : RegExTestFixtureBase
    {
        [Test]
        public void TestSingleDigit([Values('0', '5', '9')] char digit)
        {
            // Arrange
            var regex = digit.ToString();

            // Act
            var element = RegExElement.Create(digit);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string");
            var result = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit.ToString());
            Assert.That(Value.Of(result.Success).Is().True(), "Regex match");
            Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group");
            Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(digit.ToString()), "Matches whole input");
        }

        [Test]
        public void TestMultipleDigit([Values('0', '5', '9')] char digit1, [Values('2', '7')] char digit2)
        {
            // Arrange
            var regex = string.Format("[{0},{1}]", (char) Math.Min(digit1, digit2), (char) Math.Max(digit1, digit2));

            // Act
            var element = RegExElement.Create(digit1, digit2);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string");
            var result1 = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit1.ToString());
            Assert.That(Value.Of(result1.Success).Is().True(), "Regex match (digit1)");
            Assert.That(Value.Of(result1.Groups.Count).Is().EqualTo(1), "Single group (digit1)");
            Assert.That(Value.Of(result1.Groups[0].Value).Is().EqualTo(digit1.ToString()), "Matches whole input (digit1)");
            var result2 = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit2.ToString());
            Assert.That(Value.Of(result2.Success).Is().True(), "Regex match (digit2)");
            Assert.That(Value.Of(result2.Groups.Count).Is().EqualTo(1), "Single group (digit2)");
            Assert.That(Value.Of(result2.Groups[0].Value).Is().EqualTo(digit2.ToString()), "Matches whole input (digit2)");
        }

        [Test]
        public void TestRangeOfDigits([Range('0', '4')] char digit)
        {
            // Arrange
            const string regex = "[1-3]";

            // Act
            var element = RegExElement.Create('1', '2', '3');

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string");
            var result = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit.ToString());
            if ('1' <= digit && digit <= '3')
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", digit);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", digit);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(digit.ToString()), "Matches whole input ({0})", digit);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex match ({0})", digit);
            }
        }

        [Test]
        public void TestAllDigits([Range('0', '9')] char digit)
        {
            // Arrange
            const string regex = @"\d";

            // Act
            var element = RegExElement.Create('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string");
            var result = new Regex(element.ToString(), RegexOptions.ExplicitCapture).Match(digit.ToString());
            Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", digit);
            Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", digit);
            Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(digit.ToString()), "Matches whole input ({0})", digit);
        }
    }
}