using System;

namespace Atrico.Lib.Common.DateAndTime
{
    /// <summary>
    ///     Default provider for current date/time
    /// </summary>
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now
        {
            get { return DateTimeOffset.Now; }
        }
    }
}