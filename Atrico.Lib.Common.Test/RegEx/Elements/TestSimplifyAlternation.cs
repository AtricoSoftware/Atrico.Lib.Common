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
        public void TestFactoriseAnds1of2()
        {
            // (X & A) | (X & B) => X & (A | B)

            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement('X'), new TestElement('A'));
            var seq2 = RegExElement.CreateSequence(new TestElement('X'), new TestElement('B'));
            var element = RegExElement.CreateAlternation(seq1, seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"testX(testA|testB)"), "And Or inversion");
        }

        [Test]
        public void TestFactoriseAnds2of1()
        {
            // (A & X) | (B & X) => (A | B) & X

            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement('A'), new TestElement('X'));
            var seq2 = RegExElement.CreateSequence(new TestElement('B'), new TestElement('X'));
            var element = RegExElement.CreateAlternation(seq1, seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"(testA|testB)testX"), "And Or inversion");
        }

        [Test]
        public void TestFactoriseAnds1and2of3()
        {
            // (X & Y & A) | (X & Y & B) => X & Y & (A | B)

            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement('X'), new TestElement('Y'), new TestElement('A'));
            var seq2 = RegExElement.CreateSequence(new TestElement('X'), new TestElement('Y'), new TestElement('B'));
            var element = RegExElement.CreateAlternation(seq1, seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"testXtestY(testA|testB)"), "And Or inversion");
        }

        [Test]
        public void TestFactoriseAnds2and3of3()
        {
            // (A & X & Y) | (B & X & Y) => (A | B) & X & Y

            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement('A'), new TestElement('X'), new TestElement('Y'));
            var seq2 = RegExElement.CreateSequence(new TestElement('B'), new TestElement('X'), new TestElement('Y'));
            var element = RegExElement.CreateAlternation(seq1, seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"(testA|testB)testXtestY"), "And Or inversion");
        }

        [Test]
        public void TestFactoriseAnds2of3()
        {
            // (X & A & Y) | (X & B & Y) => X & (A | B) & Y

            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement('X'), new TestElement('A'), new TestElement('Y'));
            var seq2 = RegExElement.CreateSequence(new TestElement('X'), new TestElement('B'), new TestElement('Y'));
            var element = RegExElement.CreateAlternation(seq1, seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"testX(testA|testB)testY"), "And Or inversion");
        }

        // TODO mid optional
        // optimize, best match
        // multiple matches - 
    }
}