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
        ///     Sets the border as simple ASCII.
        /// </summary>
        /// <param name="internals">Ff set to <c>true</c>, internal borders are also set, otherwise external border only</param>
        public Table SetBorderSimpleAscii(bool internals = true)
        {
            // External
            SetBorder(Border.Up, '-');
            SetBorder(Border.Down, '-');
            SetBorder(Border.Left, '|');
            SetBorder(Border.Right, '|');
            SetBorder(Border.TopLeftCorner, '+');
            SetBorder(Border.TopRightCorner, '+');
            SetBorder(Border.BottomLeftCorner, '+');
            SetBorder(Border.BottomRightCorner, '+');
            // Internal
            if (internals)
            {
                SetBorder(Border.Horizontal, '-');
                SetBorder(Border.Vertical, '|');
                SetBorder(Border.TopMiddleCorner, '+');
                SetBorder(Border.MiddleLeftCorner, '+');
                SetBorder(Border.InternalCorner, '+');
                SetBorder(Border.MiddleRightCorner, '+');
                SetBorder(Border.BottomMiddleCorner, '+');
            }

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
            var horizontal = PadColumns(GetBorder(Border.Horizontal), columnWidth).ToArray();
            var down = PadColumns(GetBorder(Border.Down), columnWidth);
            var hasLeft = HasBorder(Border.Left, Border.TopLeftCorner, Border.MiddleLeftCorner, Border.BottomLeftCorner);
            var hasMiddle = HasBorder(Border.Vertical, Border.TopMiddleCorner, Border.InternalCorner, Border.BottomMiddleCorner);
            var hasRight = HasBorder(Border.Right, Border.TopRightCorner, Border.MiddleRightCorner, Border.BottomRightCorner);
            // Write each row
            var lines = new List<string>();
            // Top border
            {
                var left = hasLeft ? GetBorder(Border.TopLeftCorner) ?? GetBorder(Border.Up) : null;
                var middle = hasMiddle ? GetBorder(Border.TopMiddleCorner) ?? GetBorder(Border.Up) : null;
                var right = hasRight ? GetBorder(Border.TopRightCorner) ?? GetBorder(Border.Up) : null;
                var text = CreateRow(up, left, middle, right);
                if (!string.IsNullOrWhiteSpace(text)) lines.Add(text);
            }
            // Table rows
            var firstItem = true;
            foreach (var row in _rows)
            {
                // Internal border
                if (!firstItem)
                {
                    var left = hasLeft ? GetBorder(Border.MiddleLeftCorner) ?? GetBorder(Border.Horizontal) : null;
                    var middle = hasMiddle ? GetBorder(Border.InternalCorner) ?? GetBorder(Border.Horizontal) : null;
                    var right = hasRight ? GetBorder(Border.MiddleRightCorner) ?? GetBorder(Border.Horizontal) : null;
                    var text = CreateRow(horizontal, left, middle, right);
                    if (!string.IsNullOrWhiteSpace(text)) lines.Add(text);
                }
                else firstItem = false;
                // Items
                {
                    var left = hasLeft ? GetBorder(Border.Left) ?? (char?) ' ' : null;
                    var middle = hasMiddle ? GetBorder(Border.Vertical) ?? (char?) ' ' : null;
                    var right = hasRight ? GetBorder(Border.Right) ?? (char?) ' ' : null;
                    var paddedRow = PadColumns(row, columnWidth);
                    var text = CreateRow(paddedRow, left, middle, right);
                    if (!string.IsNullOrWhiteSpace(text)) lines.Add(text);
                }
            }
            // Bottom border
            {
                var left = hasLeft ? GetBorder(Border.BottomLeftCorner) ?? GetBorder(Border.Down) : null;
                var middle = hasMiddle ? GetBorder(Border.BottomMiddleCorner) ?? GetBorder(Border.Down) : null;
                var right = hasRight ? GetBorder(Border.BottomRightCorner) ?? GetBorder(Border.Down) : null;
                var text = CreateRow(down, left, middle, right);
                if (!string.IsNullOrWhiteSpace(text)) lines.Add(text);
            }

            return lines;
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
            var widths = columnWidths as int[] ?? columnWidths.ToArray();
            return PadColumns(new String(ch ?? ' ', widths.Count()).ToCharArray().Cast<object>(), widths, ch ?? ' ');
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