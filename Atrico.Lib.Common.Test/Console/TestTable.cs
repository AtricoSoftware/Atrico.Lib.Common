using System.Diagnostics;
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
        private static Table Test3x3Table
        {
            get
            {
                return new Table()
                    .AppendRow(1, 2, 3)
                    .AppendRow(1, 2, 3)
                    .AppendRow(1, 2, 3)
                    .SetBorder(Table.Border.Vertical);
            }
        }

        private static void DisplayTable(Table tab)
        {
            foreach (var row in tab.Tabulate())
            {
                Debug.WriteLine(row);
            }
        }

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
            DisplayTable(table);
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

        [Test]
        public void TestBordersNone()
        {
            // Arrange

            // Act
            var table = Test3x3Table;
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(3), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("123"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("123"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("123"), "Tab row 2");
        }

        [Test]
        public void TestBordersUpDown()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Up, 'u')
                .SetBorder(Table.Border.Down, 'd');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(5), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("uuu"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("123"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("123"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo("123"), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("ddd"), "Tab row 4");
        }

        [Test]
        public void TestBordersLeftRight()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Left, 'l')
                .SetBorder(Table.Border.Right, 'r');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(3), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("l123r"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("l123r"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("l123r"), "Tab row 2");
        }

        [Test]
        public void TestBordersExternal()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Up, 'u')
                .SetBorder(Table.Border.Down, 'd')
                .SetBorder(Table.Border.Left, 'l')
                .SetBorder(Table.Border.Right, 'r');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(5), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("uuuuu"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("l123r"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("l123r"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo("l123r"), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("ddddd"), "Tab row 4");
        }

        [Test]
        public void TestBordersVertical()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Vertical, 'v');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(3), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("1v2v3"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("1v2v3"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("1v2v3"), "Tab row 2");
        }

        [Test]
        public void TestBordersHorizontal()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Horizontal, 'h');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(5), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("123"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("hhh"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("123"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo("hhh"), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("123"), "Tab row 4");
        }

        [Test]
        public void TestBordersInternal()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Vertical, 'v')
                .SetBorder(Table.Border.Horizontal, 'h');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(5), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("1v2v3"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("hhhhh"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("1v2v3"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo("hhhhh"), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("1v2v3"), "Tab row 4");
        }

        [Test]
        public void TestBordersAll()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.Up, 'u')
                .SetBorder(Table.Border.Down, 'd')
                .SetBorder(Table.Border.Left, 'l')
                .SetBorder(Table.Border.Right, 'r')
                .SetBorder(Table.Border.Vertical, 'v')
                .SetBorder(Table.Border.Horizontal, 'h');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(7), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("uuuuuuu"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo("l1v2v3r"), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("hhhhhhh"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo("l1v2v3r"), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("hhhhhhh"), "Tab row 4");
            Assert.That(Value.Of(tab[5]).Is().EqualTo("l1v2v3r"), "Tab row 5");
            Assert.That(Value.Of(tab[6]).Is().EqualTo("ddddddd"), "Tab row 6");
        }

        [Test]
        public void TestBordersOutsideCorners()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.TopLeftCorner, 'A')
                .SetBorder(Table.Border.TopRightCorner, 'C')
                .SetBorder(Table.Border.BottomLeftCorner, 'G')
                .SetBorder(Table.Border.BottomRightCorner, 'I');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(5), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("A   C"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo(" 123 "), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo(" 123 "), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo(" 123 "), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("G   I"), "Tab row 4");
        }

        [Test]
        public void TestBordersInsideCorners()
        {
            // Arrange

            // Act
            var table = Test3x3Table
                .SetBorder(Table.Border.TopLeftCorner, 'A')
                .SetBorder(Table.Border.TopMiddleCorner, 'B')
                .SetBorder(Table.Border.TopRightCorner, 'C')
                .SetBorder(Table.Border.MiddleLeftCorner, 'D')
                .SetBorder(Table.Border.InternalCorner, 'E')
                .SetBorder(Table.Border.MiddleRightCorner, 'F')
                .SetBorder(Table.Border.BottomLeftCorner, 'G')
                .SetBorder(Table.Border.BottomMiddleCorner, 'H')
                .SetBorder(Table.Border.BottomRightCorner, 'I');
            DisplayTable(table);
            var tab = table.Tabulate().ToArray();

            // Assert
            Assert.That(Value.Of(table.Rows).Is().EqualTo(3), "Rows");
            Assert.That(Value.Of(table.Columns).Is().EqualTo(3), "Columns");
            Assert.That(Value.Of(tab).Count().Is().EqualTo(7), "Tab rows");
            Assert.That(Value.Of(tab[0]).Is().EqualTo("A B B C"), "Tab row 0");
            Assert.That(Value.Of(tab[1]).Is().EqualTo(" 1 2 3 "), "Tab row 1");
            Assert.That(Value.Of(tab[2]).Is().EqualTo("D E E F"), "Tab row 2");
            Assert.That(Value.Of(tab[3]).Is().EqualTo(" 1 2 3 "), "Tab row 3");
            Assert.That(Value.Of(tab[4]).Is().EqualTo("D E E F"), "Tab row 4");
            Assert.That(Value.Of(tab[5]).Is().EqualTo(" 1 2 3 "), "Tab row 5");
            Assert.That(Value.Of(tab[6]).Is().EqualTo("G H H I"), "Tab row 6");
        }
    }
}
