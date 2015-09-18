using System.Globalization;
using System.Linq;

namespace Atrico.Lib.Common
{
    /// <summary>
    /// Utilities for decimal values
    /// </summary>
    public static class DecimalUtilities
    {
        /// <summary>
        /// Trims trailing zeros in decimal value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>String representation of value</returns>
        public static string ToTrimmedString(this decimal value)
        {
            var str = value.ToString(CultureInfo.InvariantCulture);
            if (!str.Contains('.'))
            {
                return str;
            }
            str = str.TrimEnd('0');
            str = str.TrimEnd('.');
            return str;
        }
    }
}