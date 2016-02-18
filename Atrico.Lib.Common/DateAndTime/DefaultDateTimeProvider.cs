using System;

namespace Atrico.Lib.Common.DateAndTime
{
    /// <summary>
    ///     Default provider for current date/time
    /// </summary>
    public class DefaultDateTimeProvider : IDateTimeProvider
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