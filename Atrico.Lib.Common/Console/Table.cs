using System.Collections.Generic;
using System.Text;

namespace Atrico.Lib.Common.Console
{
    /// <summary>
    ///     Write a table to the console
    /// </summary>
    public class Table
    {
        private readonly List<List<object>> _rows = new List<List<object>>();

        public int Rows
        {
            get { return _rows.Count; }
        }

        public int Columns
        {
            get { return _rows.Count == 0 ? 0 : _rows[0].Count; }
        }

        /// <summary>
        ///     Appends the row at the bottom of table
        /// </summary>
        /// <param name="row">The items in the row</param>
        public void AppendRow(params object[] row)
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
            // Output lines
            var lines = new List<string>();
            foreach (var row in _rows)
            {
                var line = new StringBuilder();
                for (var column = 0; column < row.Count; ++column)
                {
                    if (line.Length > 0)
                    {
                        line.Append(' ');
                    }
                    var text = ReferenceEquals(row[column], null) ? "" : row[column].ToString();
                    line.AppendFormat("{0,-" + columnWidth[column] + "}", text);
                }
                lines.Add(line.ToString());
            }
            return lines;
        }
    }

    // TODO
    // Add borders
    // Add headers
    // Number rows
    // Remove empty columns/rows
    // Parameterise spacing
}
