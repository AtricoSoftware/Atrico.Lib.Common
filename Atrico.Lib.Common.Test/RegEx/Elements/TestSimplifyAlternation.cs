using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.RegEx.Elements
{
    [TestFixture]
    public class TestSimplifyAlternation : RegExTestFixtureBase
    {
        [Test]
        public void TestMultipleAlternations()
        {
            // Arrange
            var alt12 = RegExElement.CreateAlternation(new TestElement(1), new TestElement(2));
            var alt34 = RegExElement.CreateAlternation(new TestElement(3), new TestElement(4));
            var element = RegExElement.CreateAlternation(alt12, new TestElement(5), alt34);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"(test1|test2|test3|test4|test5)"), "Removed composite");
        }

        [Test]
        public void TestMultipleChars()
        {
            // Arrange
            var char12 = RegExElement.CreateAlternation(RegExElement.Create('1'), RegExElement.Create('2'));
            var char34 = RegExElement.CreateAlternation(RegExElement.Create('3'), RegExElement.Create('4'));
            var element = RegExElement.CreateAlternation(char12, new TestElement(5), char34);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"([1-4]|test5)"), "Merged chars");
        }

        [Test]
        public void TestFactoriseAnds1()
        {
            // (A & B) | (A & C) => A & (B | C)

            // Arrange
            var seq12 = RegExElement.CreateSequence(new TestElement(1), new TestElement(2));
            var seq13 = RegExElement.CreateSequence(new TestElement(1), new TestElement(3));
            var element = RegExElement.CreateAlternation(seq12, seq13);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"test1(test2|test3)"), "And Or inversion");
        }

        // TODO different orders (A & B) | (C & B) => (B | C) & A
        // TODO different numbers of elements (A & B & X) | (C & B & X) => (B | C) & A & X
    }
}