using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Atrico.Lib.Common.Collections
{
    public static class FormatCollection
    {
        public static string ToCollectionString(this IEnumerable collection, string braces, bool bracesIfEmpty = true)
        {
            return ToCollectionString(collection, null, braces, bracesIfEmpty);
        }
       public static string ToCollectionString(this IEnumerable collection, ISet<object> highlight, string braces, bool bracesIfEmpty = true)
        {
            return ToCollectionString(collection, highlight, braces.Substring(0, 1), braces.Substring(1, 1), ",", bracesIfEmpty);
        }

        public static string ToCollectionString(this IEnumerable collection, string openBrace = "[", string closeBrace = "]", string separator = ",", bool bracesIfEmpty = true)
        {
            return ToCollectionString(collection, null, openBrace, closeBrace, separator, bracesIfEmpty);
        }

        public static string ToCollectionString(this IEnumerable collection, ISet<object> highlight, string openBrace = "[", string closeBrace = "]", string separator = ",", bool bracesIfEmpty = true)
        {
            var first = true;
            var text = new StringBuilder();
            foreach (var item in collection)
            {
                if (!first)
                {
                    text.Append(separator);
                }
                else
                {
                    first = false;
                }

                if (highlight != null && highlight.Contains(item))
                {
                    text.AppendFormat("*{0}*", item);
                }
                else
                {
                    text.Append(item);
                }
            }
            if (bracesIfEmpty || text.Length > 0)
            {
                if (!string.IsNullOrEmpty(openBrace))
                {
                    text.Insert(0, openBrace);
                }
                if (!string.IsNullOrEmpty(closeBrace))
                {
                    text.Append(closeBrace);
                }
            }
            return text.ToString();
        }
    }
}
