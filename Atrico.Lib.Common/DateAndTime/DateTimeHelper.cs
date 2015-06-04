using System;

namespace Atrico.Lib.Common.DateAndTime
{
    /// <summary>
    ///     Default handler for current date/time
    /// </summary>
    public class DateTimeHelper : IDateTime
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public DateTimeOffset NowOffset
        {
            get { return DateTimeOffset.Now; }
        }
    }
}