using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing.NUnitAttributes;

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
            Assert.That(Value.Of(simpleElement.ToString()).Is().EqualTo(@"(?:test1|test2|test3|test4|test5)"), "Removed composite");
        }
    }
}