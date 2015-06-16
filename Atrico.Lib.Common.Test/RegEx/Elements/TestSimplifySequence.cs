using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.RegEx.Elements
{
    [TestFixture]
    public class TestSimplifySequence : RegExTestFixtureBase
    {
        [Test]
        public void TestMultipleSequences()
        {
            // Arrange
            var seq12 = RegExElement.CreateSequence(new TestElement(1), new TestElement(2));
            var seq34 = RegExElement.CreateSequence(new TestElement(3), new TestElement(4));
            var element = RegExElement.CreateSequence(seq12, new TestElement(5), seq34);

            // Act
            var simpleElement = element.Simplify();

            // Assert
            DisplayElement(element);
            DisplayElement(simpleElement);
            Assert.That(Value.Of(simpleElement).Is().Not().ReferenceEqualTo(element), "New element");
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"test1test2test5test3test4"), "Removed composite");
        }
    }
}