using System.Text.RegularExpressions;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;
using Range = NUnit.Framework.RangeAttribute;

namespace Atrico.Lib.Common.Test.RegEx
{
    [TestFixture]
    public class TestNumberRange : TestFixtureBase
    {
        [Test]
        public void TestSingleDigitValid([Range(3, 8)] int value)
        {
            // Arrange
            var text = value.ToString("D");

            // Act
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(3, 8);
            var regex = new Regex(number.ToString());
            var result = regex.Match(text);

            // Assert
            Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
            Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
            Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
        }

        //[Test]
        //public void TestSingleDigitInvalid([Values("", "0", "1", "2", "9")] string text)
        //{
        //    // Arrange

        //    // Act
        //    var regex = new Regex(RegExHelpers.NumberRange(3, 8));
        //    var result = regex.Match(text);

        //    // Assert
        //    Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
        //}

        //[Test]
        //public void TestDoubleDigitValid([Range(63, 81)] int value)
        //{
        //    // Arrange
        //    var text = value.ToString("D");

        //    // Act
        //    var regex = new Regex(RegExHelpers.NumberRange(63, 81));
        //    var result = regex.Match(text);

        //    // Assert
        //    Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
        //    Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
        //    Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
        //}

        //[Test]
        //public void TestDoubleDigitInvalid([Values("", "0", "62", "82", "100")] string text)
        //{
        //    // Arrange

        //    // Act
        //    var regex = new Regex(RegExHelpers.NumberRange(63, 81));
        //    var result = regex.Match(text);

        //    // Assert
        //    Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
        //}

        //[Test, Ignore]
        //public void TestInvalid([Values(0, 3, 11, 124, 200)] int value)
        //{
        //    // Arrange
        //    var text1 = value.ToString("D");
        //    var text2 = value.ToString("000");

        //    // Act
        //    var regex = new Regex(RegExHelpers.NumberRange(12, 123));
        //    var result1 = regex.Match(text1);
        //    var result2 = regex.Match(text2);

        //    // Assert
        //    Assert.That(Value.Of(result1.Success).Is().False(), string.Format("{0} - Failure", text1));
        //    Assert.That(Value.Of(result2.Success).Is().False(), string.Format("{0} - Failure", text2));
        //}
    }
}