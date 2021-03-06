﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx;
using Atrico.Lib.Testing.TestAttributes;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.RegEx
{
    [TestFixture]
    public class TestNumberMatcher : RegExTestFixtureBase
    {
        [Test]
        public void TestSingleDigitValid([Range(3, 8)] int value)
        {
            // Arrange
            var text = value.ToString("D");
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(3, 8);
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
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
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
            Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
        }

        [Test]
        public void TestDoubleDigitValid([Values("63", "75", "81")] string text)
        {
            // Arrange
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(63, 81);
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
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
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
            Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
        }

        [Test]
        public void TestMultipleRanges1([Range('0', '9')] char value)
        {
            // Arrange
            var text = value.ToString();
            var expectedMatch = ('1' <= value && value <= '3') || ('6' <= value && value <= '7');
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(1, 3)
                .AddRange(6, 7);
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
            if (expectedMatch)
            {
                Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure", text));
            }
        }

        [Test]
        public void TestMultipleRanges2([Range(0, 102)] int value)
        {
            // Arrange
            var text = value.ToString();
            var expectedMatch = (12 <= value && value <= 20) || (99 <= value && value <= 101);
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(12, 20)
                .AddRange(99, 101);
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
            if (expectedMatch)
            {
                Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure ({1})", text, result));
            }
        }

        private static IEnumerable<int> _testMultipleRanges3Values
        {
            get
            {
                var values = new List<int>();
                for (var i = 122; i <= 323; ++i) values.Add(i);
                for (var i = 9999; i <= 10002; ++i) values.Add(i);
                return values;
            }
        }

        [Test]
        public void TestMultipleRanges3([ValueSource("_testMultipleRanges3Values")] int value)
        {
            // Arrange
            var text = value.ToString();
            var expectedMatch = (123 <= value && value <= 321) || (10000 <= value && value <= 10001);
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(123, 321)
                .AddRange(10000, 10001);
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayElement(number.Element);
            if (expectedMatch)
            {
                Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure ({1})", text, result));
            }
        }

        private static IEnumerable<int> _testRepeatsValues
        {
            get
            {
                var values = new List<int>();
                for (var i = 99; i <= 102; ++i) values.Add(i);
                for (var i = 999; i <= 1002; ++i) values.Add(i);
                for (var i = 9999; i <= 10002; ++i) values.Add(i);
                for (var i = 99999; i <= 100002; ++i) values.Add(i);
                return values;
            }
        }

        [Test]
        public void TestRepeats([ValueSource("_testRepeatsValues")] int value)
        {
            // Arrange
            var text = value.ToString();
            var expectedMatch = 
                (100 <= value && value <= 101) || 
                (1000 <= value && value <= 1001) || 
                (10000 <= value && value <= 10001) ||
                (100000 <= value && value <= 100001);
            var number = new RegExHelpers.NumberMatcher()
                .AddRange(100, 101)
                .AddRange(1000, 1001)
                .AddRange(10000, 10001)
                .AddRange(100000, 100001);
            var regex = new Regex(number.ToString(), RegexOptions.ExplicitCapture);

            // Act
            var result = regex.Match(text);

            // Assert
            DisplayTree(number.CharacterTree);
            DisplayElement(number.Element);
            if (expectedMatch)
            {
                Assert.That(Value.Of(result.Success).Is().True(), string.Format("{0} - Success", text));
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), string.Format("{0} - Group count", text));
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), string.Format("{0} - Full match", text));
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), string.Format("{0} - Failure ({1})", text, result));
            }
        }
    }
}