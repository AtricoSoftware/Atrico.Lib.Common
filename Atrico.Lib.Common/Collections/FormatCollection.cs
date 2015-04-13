using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Atrico.Lib.Common.Collections
{
	public static class FormatCollection
	{
		public static string ToCollectionString(this IEnumerable collection, string braces = "[]", bool bracesIfEmpty = true)
		{
			return ToCollectionString(collection, new HashSet<object>(), braces, bracesIfEmpty);
		}


		public static string ToCollectionString(this IEnumerable collection, ISet<object> highlight, string braces = "[]", bool bracesIfEmpty = true)
		{
			var first = true;
			var text = new StringBuilder();
			foreach (var item in collection)
			{
				if (!first)
				{
					text.Append(',');
				}
				else
				{
					first = false;
				}

				if (highlight.Contains(item))
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
				text.Insert(0, braces[0]);
				text.Append(braces[1]);
			}
			return text.ToString();
		}
	}
}
