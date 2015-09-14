using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.RegEx.Elements
{
    [TestFixture]
    public class TestSimplifySequence : RegExTestFixtureBase
    {
        [Test]
        public void TestMultipleSequences()
        {
            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement("A"), new TestElement("B"));
            var seq2 = RegExElement.CreateSequence(new TestElement("C"), new TestElement("D"));
            var element = RegExElement.CreateSequence(seq1, new TestElement("E"), seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"ABECD"), "Removed composite");
        }

        [Test]
        public void TestRepeatElement1char()
        {
            // Arrange
            var element = RegExElement.CreateSequence(
                new TestElement("A"),
                new TestElement("B"), new TestElement("B"),
                new TestElement("C"), new TestElement("C"), new TestElement("C"),
                new TestElement("D"), new TestElement("D"), new TestElement("D"), new TestElement("D"));

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"ABBCCCD{4}"), "Repeats");
        }

        [Test]
        public void TestRepeatElement2chars()
        {
            // Arrange
            var element = RegExElement.CreateSequence(
                new TestElement("Aa"),
                new TestElement("Bb"), new TestElement("Bb"),
                new TestElement("Cc"), new TestElement("Cc"), new TestElement("Cc"),
                new TestElement("Dd"), new TestElement("Dd"), new TestElement("Dd"), new TestElement("Dd"));

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"AaBbBbCc{3}Dd{4}"), "Repeats");
        }

        [Test]
        public void TestRepeatElement3chars()
        {
            // Arrange
            var element = RegExElement.CreateSequence(
                new TestElement("Aaa"),
                new TestElement("Bbb"), new TestElement("Bbb"),
                new TestElement("Ccc"), new TestElement("Ccc"), new TestElement("Ccc"),
                new TestElement("Ddd"), new TestElement("Ddd"), new TestElement("Ddd"), new TestElement("Ddd"));

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"AaaBbb{2}Ccc{3}Ddd{4}"), "Repeats");
        }
    }
}