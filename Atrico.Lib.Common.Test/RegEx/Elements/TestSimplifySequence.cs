using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.RegEx.Elements
{
    [TestFixture]
    public class TestSimplifySequence : RegExTestFixtureBase
    {
        [Test]
        public void TestMultipleSequences()
        {
            // Arrange
            var seq1 = RegExElement.CreateSequence(new TestElement('A'), new TestElement('B'));
            var seq2 = RegExElement.CreateSequence(new TestElement('C'), new TestElement('D'));
            var element = RegExElement.CreateSequence(seq1, new TestElement('E'), seq2);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"testAtestBtestEtestCtestD"), "Removed composite");
        }

        [Test]
        public void TestRepeatElement2()
        {
            // Arrange
            var element = RegExElement.CreateSequence(new TestElement('A'), new TestElement('A'), new TestElement('B'), new TestElement('C'), new TestElement('C'));

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"testAtestAtestBtestCtestC"), "No change");
        }

        [Test]
        public void TestRepeatElement3plus()
        {
            // Arrange
            var element = RegExElement.CreateSequence(new TestElement('A'), new TestElement('A'), new TestElement('A'), new TestElement('B'), new TestElement('C'), new TestElement('C'), new TestElement('C'), new TestElement('C'));

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"{testA}3testB{testC}4"), "Repeats");
        }
    }
}