using System;

namespace Atrico.Lib.Common.DateAndTime
{
	/// <summary>
	///	Modifiable provider for date/time
	///	The date can be set
	/// </summary>
	public class ModifiableDateTimeProvider : IDateTimeProvider, ISetDateTime
	{
		private long _daysOffset = 0L;

		public DateTimeOffset Now
		{
			get { return DateTimeOffset.Now.AddDays(_daysOffset); }
		}

		public void SetDate(DateTimeOffset date)
		{
			_daysOffset = (long)((date.Date - DateTimeOffset.Now.Date).TotalDays);
		}
	}
}