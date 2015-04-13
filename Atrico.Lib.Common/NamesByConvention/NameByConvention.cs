using System;

// ReSharper disable once CheckNamespace

namespace Atrico.Lib.Common.NamesByConvention
{
	public abstract class NameByConvention
	{
		private readonly string _baseName;
		private readonly Lazy<string> _name;

		public override string ToString()
		{
			return _name.Value;
		}

		/// <summary>
		///     Constructor
		/// </summary>
		protected NameByConvention(string baseName)
		{
			_baseName = baseName;
			_name = new Lazy<string>(() => CreateName(_baseName));
		}

		protected abstract string CreateName(string baseName);

		public static implicit operator string(NameByConvention obj)
		{
			return obj.ToString();
		}
	}
}
