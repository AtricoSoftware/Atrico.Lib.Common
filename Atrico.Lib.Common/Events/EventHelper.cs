using System;
using System.Threading;

namespace Atrico.Lib.Common.Events
{
    public static class EventHelper
    {
        public static void Raise(this EventHandler @event, object sender)
        {
            var localEvent = @event;
            if (!ReferenceEquals(localEvent, null))
            {
                localEvent(sender, new EventArgs());
            }
        }

        public static void Raise(this EventHandler @event, object sender, Thread thread)
        {
            var localEvent = @event;
            if (!ReferenceEquals(localEvent, null))
            {
                localEvent(sender, new EventArgs());
            }
        }

        public static void Raise<TParameter1>(this EventHandler<TParameter1> @event, object sender, TParameter1 parameter1)
        {
            var localEvent = @event;
            if (!ReferenceEquals(localEvent, null))
            {
                localEvent(sender, parameter1);
            }
        }
    }
}
