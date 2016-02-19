using System;

namespace Atrico.Lib.Common.DateAndTime
{
	/// <summary>
	/// Interface for setting the date/time
	/// </summary>
	public interface ISetDateTime
	{
		/// <summary>
		/// Set the current date
		/// Stored as offset so 24 hours after setting date, date will have increased
		/// </summary>
		/// <param name="date">Date to set</param>
		void SetDate(DateTimeOffset date);
	}
}