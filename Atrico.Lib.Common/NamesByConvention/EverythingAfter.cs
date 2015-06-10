using System;
using System.Linq;

// ReSharper disable once CheckNamespace

namespace Atrico.Lib.Common.NamesByConvention
{
	public class EverythingAfter : NameByConvention
	{
		private readonly string _text;

        public EverythingAfter(string baseName, string text)
			: base(baseName)
		{
			_text = text;
		}

		protected override string CreateName(string baseName)
		{
			var name = baseName.Split(new[] {_text}, StringSplitOptions.RemoveEmptyEntries);
			return name.LastOrDefault();
		}
	}
}
