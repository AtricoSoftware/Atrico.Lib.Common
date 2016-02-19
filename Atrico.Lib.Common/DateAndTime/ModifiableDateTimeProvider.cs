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

		public DateTime Now
		{
			get { return DateTime.Now.AddDays(_daysOffset); }
		}

		public DateTimeOffset NowOffset
		{
			get { return DateTimeOffset.Now.AddDays(_daysOffset); }
		}

		public void SetDate(DateTime date)
		{
			_daysOffset = (long)((date.Date - DateTime.Now.Date).TotalDays);
		}
	}
}