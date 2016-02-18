using System;

namespace Atrico.Lib.Common.DateAndTime
{
    /// <summary>
    ///     Get the current time/date
    ///     Use service to allow injection for testing
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        ///     Gets the current date/time
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        ///     Gets the current date/time as DT Offset
        /// </summary>
        DateTimeOffset NowOffset { get; }
    }
}