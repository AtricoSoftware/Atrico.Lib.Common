using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Console;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Console
{
    [TestFixture]
    public class TestTable
    {
        [Test]
        public void TestAddRow()
        {
            // Arrange
            var table = new Table();

            // Act
            table.AppendRow(1, 2, 3);
            table.AppendRow(1, 2);
            table.AppendRow(1, 2, 3, 4);
            table.AppendRow(null, 2, null, 4);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(4), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(4), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(4), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("1 2 3  "), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("1 2    "), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("1 2 3 4"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo("  2   4"), "Tab row 3");
        }
    }
}
