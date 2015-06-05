using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Console
{
    /// <summary>
    ///     Write a table to the console
    /// </summary>
    public class Table
    {
        private readonly List<List<object>> _rows = new List<List<object>>();
        private readonly char?[] _border;

        public Table()
        {
            _border = new char?[Enum.GetNames(typeof (Border)).Count()];
            // Default vertical space
            SetBorder(Border.Vertical, ' ');
        }

        public int Rows
        {
            get { return _rows.Count; }
        }

        public int Columns
        {
            get { return _rows.Count == 0 ? 0 : _rows[0].Count; }
        }

        public enum Border
        {
            Up,
            Down,
            Left,
            Right,
            Horizontal,
            Vertical,
            TopLeftCorner,
            TopMiddleCorner,
            TopRightCorner,
            MiddleLeftCorner,
            InternalCorner,
            MiddleRightCorner,
            BottomLeftCorner,
            BottomMiddleCorner,
            BottomRightCorner,
        }

        public Table SetBorder(Border border, char? ch = null)
        {
            _border[(int) border] = ch;
            return this;
        }

        /// <summary>
        ///     Appends the row at the bottom of table
        /// </summary>
        /// <param name="row">The items in the row</param>
        public Table AppendRow(params object[] row)
        {
            // Ensure there are enough columns
            if (Columns < row.Length)
            {
                var nulls = new object[row.Length - Columns];
                _rows.ForEach(r => r.AddRange(nulls));
            }
            var newRow = new List<object>(row);
            // Ensure new row has enough columns
            if (row.Length < Columns)
            {
                newRow.AddRange(new object[Columns - row.Length]);
            }

            _rows.Add(newRow);
            return this;
        }

        /// <summary>
        ///     Tabulates this instance as a list of lines of text
        /// </summary>
        /// <returns>multiple lines of text</returns>
        public IEnumerable<string> Tabulate()
        {
            // Calculate max width of each column
            var columnWidth = new int[Columns];
            foreach (var row in _rows)
            {
                for (var column = 0; column < Columns; ++column)
                {
                    var len = (ReferenceEquals(row[column], null) ? "" : row[column].ToString()).Length;
                    if (len > columnWidth[column])
                    {
                        columnWidth[column] = len;
                    }
                }
            }
            // Borders
            var up = PadColumns(GetBorder(Border.Up), columnWidth);
            var horizontal = PadColumns(GetBorder(Border.Horizontal), columnWidth);
            var down = PadColumns(GetBorder(Border.Down), columnWidth);
            var hasLeft = HasBorder(Border.Left, Border.TopLeftCorner, Border.MiddleLeftCorner, Border.BottomLeftCorner);
            var hasMiddle = HasBorder(Border.Vertical, Border.TopMiddleCorner, Border.InternalCorner, Border.BottomMiddleCorner);
            var hasRight = HasBorder(Border.Right, Border.TopRightCorner, Border.MiddleRightCorner, Border.BottomRightCorner);
            // Write each row
            var lines = new List<string>();
            if (up != null)
            {
                var left = hasLeft ? GetBorder(Border.TopLeftCorner) ?? GetBorder(Border.Up) : null;
                var middle = hasMiddle ? GetBorder(Border.TopMiddleCorner) ?? GetBorder(Border.Up) : null;
                var right = hasRight ? GetBorder(Border.TopRightCorner) ?? GetBorder(Border.Up) : null;
                lines.Add(CreateRow(up, left, middle, right));
            }
            var firstItem = true;
            foreach (var row in _rows)
            {
                if (!firstItem && horizontal != null)
                {
                    var left = hasLeft ? GetBorder(Border.MiddleLeftCorner) ?? GetBorder(Border.Horizontal) : null;
                    var middle = hasMiddle ? GetBorder(Border.InternalCorner) ?? GetBorder(Border.Horizontal) : null;
                    var right = hasRight ? GetBorder(Border.MiddleRightCorner) ?? GetBorder(Border.Horizontal) : null;
                    lines.Add(CreateRow(horizontal, left, middle, right));                   
                }
                else firstItem = false;
                var padded = PadColumns(row, columnWidth);
                lines.Add(CreateRow(padded, GetBorder(Border.Left), GetBorder(Border.Vertical), GetBorder(Border.Right)));
            }
            if (down != null)
            {
                var left = hasLeft ? GetBorder(Border.BottomLeftCorner) ?? GetBorder(Border.Down) : null;
                var middle = hasMiddle ? GetBorder(Border.BottomMiddleCorner) ?? GetBorder(Border.Down) : null;
                var right = hasRight ? GetBorder(Border.BottomRightCorner) ?? GetBorder(Border.Down) : null;
                lines.Add(CreateRow(down, left, middle, right));
            }

            return lines;

            #region Old
            //var horizontal = CreateHorizontalBorder(GetBorder(Border.Horizontal), GetBorder(Border.MiddleLeftCorner), GetBorder(Border.InternalCorner), GetBorder(Border.MiddleRightCorner), columnWidth);

            //var hasLeft = HasBorder(Border.Left, Border.TopLeftCorner, Border.MiddleLeftCorner, Border.BottomLeftCorner);
            //var hasRight = HasBorder(Border.Right, Border.TopRightCorner, Border.MiddleRightCorner, Border.BottomRightCorner);
            //// Output lines
            //if (up != null)
            //{
            //    lines.Add(up);
            //}
            //var firstRow = true;
            //foreach (var row in _rows)
            //{
            //    var line = new StringBuilder();
            //    if (hasLeft)
            //    {
            //        line.Append(GetBorder(Border.Left) ?? ' ');
            //    }
            //    for (var column = 0; column < row.Count; ++column)
            //    {
            //        if (column > 0 && HasBorder(Border.Vertical))
            //        {
            //            line.Append(GetBorder(Border.Vertical));
            //        }
            //        var text = ReferenceEquals(row[column], null) ? "" : row[column].ToString();
            //        line.AppendFormat("{0,-" + columnWidth[column] + "}", text);
            //    }
            //    if (hasRight)
            //    {
            //        line.Append(GetBorder(Border.Right) ?? ' ');
            //    }
            //    if (!firstRow && horizontal != null)
            //    {
            //        lines.Add(horizontal);
            //    }
            //    firstRow = false;
            //    lines.Add(line.ToString());
            //}
            //if (down != null)
            //{
            //    lines.Add(down);
            //}
            //return lines;
            #endregion
        }

        private static string CreateRow(IEnumerable<string> items, char? left, char? middle, char? right)
        {
            var line = new StringBuilder();
            if (left.HasValue) line.Append(left);
            var firstItem = true;
            foreach (var item in items)
            {
                if (!firstItem && middle.HasValue) line.Append(middle);
                else firstItem = false;
                line.Append(item);
            }
            if (right.HasValue) line.Append(right);
            return line.ToString();
        }

        private static IEnumerable<string> PadColumns(char? ch, IEnumerable<int> columnWidths)
        {
            if (!ch.HasValue)
            {
                return null;
            }
            var widths = columnWidths as int[] ?? columnWidths.ToArray();
            return PadColumns(new String(ch.Value, widths.Count()).ToCharArray().Cast<object>(), widths, ch.Value);
        }

        private static IEnumerable<string> PadColumns(IEnumerable<object> items, IEnumerable<int> columnWidths, char fill = ' ')
        {
            var padded = new List<string>();
            var widthEn = columnWidths.GetEnumerator();
            foreach (var item in items)
            {
                var width = widthEn.MoveNext() ? widthEn.Current : 0;
                var col = new StringBuilder((item ?? "").ToString());
                while (col.Length < width)
                {
                    col.Append(fill);
                }
                padded.Add(col.ToString());
            }
            return padded;
        }

        private string CreateHorizontalBorder(char? ch, char? left, char? mid, char? right, IEnumerable<int> columnWidths)
        {
            if (!ch.HasValue && !left.HasValue && !right.HasValue && !mid.HasValue)
            {
                return null;
            }
            var leftCorner = left ?? ch ?? GetBorder(Border.Left);
            var rightCorner = right ?? ch ?? GetBorder(Border.Right);
            var text = new StringBuilder();
            if (leftCorner.HasValue)
            {
                text.Append(leftCorner);
            }
            var firstCol = true;
            foreach (var width in columnWidths)
            {
                if (!firstCol && HasBorder(Border.Vertical))
                {
                    text.Append(ch ?? ' ');
                }
                firstCol = false;
                text.Append(ch ?? ' ', width);
            }
            if (rightCorner.HasValue)
            {
                text.Append(rightCorner);
            }
            return text.ToString();
        }

        private char? GetBorder(Border border)
        {
            return _border[(int) border];
        }

        private bool HasBorder(params Border[] borders)
        {
            return borders.Any(border => _border[(int) border].HasValue);
        }
    }

    // TODO
    // Add borders
    // Add headers
    // Number rows
    // Remove empty columns/rows
    // Parameterise spacing
}
