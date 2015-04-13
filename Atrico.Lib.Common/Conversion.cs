using System;

namespace Atrico.Lib.Common
{
	/// <summary>
	/// Type conversion utility
	/// </summary>
	public static class Conversion
	{
		public static TTo Convert<TTo>(object from)
		{
			// Simple cast
			try
			{
				return (TTo) from;
			}
			catch (Exception)
			{
				// If target is boolean, make sure from type is boolean
				// TODO - Do this correctly when you implement convert to
				if (typeof (TTo) == typeof (bool))
				{
					throw new Exception(string.Format("'{0}' cannot be converted to {1}", from, typeof (TTo)));
				}

				// System conversion
				try
				{
					return (TTo) System.Convert.ChangeType(from, typeof (TTo));
				}
				catch (Exception ex)
				{
					throw new Exception(string.Format("'{0}' cannot be converted to {1}", from, typeof (TTo)), ex);
				}
			}
		}
	}
}
