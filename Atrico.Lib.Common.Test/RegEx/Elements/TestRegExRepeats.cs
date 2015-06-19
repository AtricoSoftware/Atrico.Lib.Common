using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.TestAttributes;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.RegEx.Elements
{
    [TestFixture]
    public class TestRegExRepeats : RegExTestFixtureBase
    {
        [Test]
        public void Test1([Range(0, 3)] int len)
        {
            // Arrange
            const int repeats = 1;
            const string regex = "A";
            var expected = len == repeats;
            var text = new string('A', len);

            // Act
            var element = RegExElement.CreateRepeat(RegExElement.Create('A'), repeats);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string ({0})", text);
            var result = CreateRegex(element).Match(text);
            if (expected)
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", text);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", text);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), "Matches whole input ({0})", text);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex fail ({0})", text);
            }
        }

        [Test]
        public void Test2([Range(0, 3)] int len)
        {
            // Arrange
            const int repeats = 2;
            const string regex = "AA";
            var expected = len == repeats;
            var text = new string('A', len);

            // Act
            var element = RegExElement.CreateRepeat(RegExElement.Create('A'), repeats);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string ({0})", text);
            var result = CreateRegex(element).Match(text);
            if (expected)
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", text);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", text);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), "Matches whole input ({0})", text);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex fail ({0})", text);
            }
        }

        [Test]
        public void Test3([Range(0, 4)] int len)
        {
            // Arrange
            const int repeats = 3;
            const string regex = "AAA";
            var expected = len == repeats;
            var text = new string('A', len);

            // Act
            var element = RegExElement.CreateRepeat(RegExElement.Create('A'), repeats);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string ({0})", text);
            var result = CreateRegex(element).Match(text);
            if (expected)
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", text);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", text);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), "Matches whole input ({0})", text);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex fail ({0})", text);
            }
        }

        [Test]
        public void Test4([Range(0, 5)] int len)
        {
            // Arrange
            const int repeats = 4;
            const string regex = "A{4}";
            var expected = len == repeats;
            var text = new string('A', len);

            // Act
            var element = RegExElement.CreateRepeat(RegExElement.Create('A'), repeats);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string ({0})", text);
            var result = CreateRegex(element).Match(text);
            if (expected)
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", text);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", text);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), "Matches whole input ({0})", text);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex fail ({0})", text);
            }
        }

        [Test]
        public void Test1to3([Range(0, 4)] int len)
        {
            // Arrange
            const int min = 1;
            const int max = 3;
            const string regex = "A{1,3}";
            var expected = min <= len && len <= max;
            var text = new string('A', len);

            // Act
            var element = RegExElement.CreateRepeat(RegExElement.Create('A'), min, max);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string ({0})", text);
            var result = CreateRegex(element).Match(text);
            if (expected)
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", text);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", text);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), "Matches whole input ({0})", text);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex fail ({0})", text);
            }
        }

        [Test]
        public void Test2toX([Range(0, 6)] int len)
        {
            // Arrange
            const int min = 2;
            const string regex = "A{2,}";
            var expected = min <= len;
            var text = new string('A', len);

            // Act
            var element = RegExElement.CreateRepeat(RegExElement.Create('A'), min, null);

            // Assert
            DisplayElement(element);
            Assert.That(Value.Of(element.ToString()).Is().EqualTo(regex), "Regex string ({0})", text);
            var result = CreateRegex(element).Match(text);
            if (expected)
            {
                Assert.That(Value.Of(result.Success).Is().True(), "Regex match ({0})", text);
                Assert.That(Value.Of(result.Groups.Count).Is().EqualTo(1), "Single group ({0})", text);
                Assert.That(Value.Of(result.Groups[0].Value).Is().EqualTo(text), "Matches whole input ({0})", text);
            }
            else
            {
                Assert.That(Value.Of(result.Success).Is().False(), "Regex fail ({0})", text);
            }
        }
    }
}