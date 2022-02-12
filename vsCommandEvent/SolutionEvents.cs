/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using net.r_eg.SobaScript.Exceptions;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE
{
    [Serializable]
    public class SolutionEvents: ISolutionEvents
    {
        /// <inheritdoc cref="ISolutionEvents.Header"/>
        public Header Header { get; set; } = new Header();

        /// <inheritdoc cref="ISolutionEvents.Components"/>
        public Component[] Components { get; set; }

        /// <inheritdoc cref="ISolutionEvents.Event"/>
        public Event[] Event { get; set; } = new Event[] { };

        /// <inheritdoc cref="ISolutionEvents.getEvent"/>
        /// <exception cref="NotFoundException"></exception>
        public ISolutionEvent[] getEvent(SolutionEventType type)
        {
            switch(type)
            {
                case SolutionEventType.Common:
                case SolutionEventType.CommandEvent:
                case SolutionEventType.OWP: {
                    return Event;
                }
            }

            throw new NotFoundException(type);
        }

        public bool ShouldSerializeComponents() => Components?.Length > 0;
        public bool ShouldSerializePreBuild() => Event?.Length > 0;
    }
}
