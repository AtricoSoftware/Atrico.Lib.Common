using System.Diagnostics;
using System.Text.RegularExpressions;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.RegEx
{
    [TestFixture]
    public class TestNumberMatcher : TestFixtureBase
    {
        [Test]
        public void TestSingleDigitValid([Range(3, 8)] int value)
        {
            // Arrange
            var text = value.ToString("D");
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(3, 8);
            var regex = new Regex(number.ToString());

            // Act
            var result = regex.Match(text);

            // Assert
            Debug.WriteLine(number.ToString());
            Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
            Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
            Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
        }

        [Test]
        public void TestSingleDigitInvalid([Values("", "0", "1", "2", "9")] string text)
        {
            // Arrange
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(3, 8);
            var regex = new Regex(number.ToString());

            // Act
            var result = regex.Match(text);

            // Assert
            Debug.WriteLine(number.ToString());
            Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
        }

        [Test]
        public void TestDoubleDigitValid()
        {
            // Arrange
            var text = 63.ToString("D");
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(63, 81);
            var regex = new Regex(number.ToString());

            // Act
            var result = regex.Match(text);

            // Assert
            Debug.WriteLine(number.ToString());
            Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
            Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
            Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
        }

        [Test]
        public void TestDoubleDigitInvalid([Values("", "0", "62", "82", "100")] string text)
        {
            // Arrange
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(63, 81);
            var regex = new Regex(number.ToString());

            // Act
            var result = regex.Match(text);

            // Assert
            Debug.WriteLine(number.ToString());
            Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
        }
    }
}