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
        /// <summary>
        /// Header of information.
        /// </summary>
        public Header Header
        {
            get { return header; }
            set { header = value; }
        }
        [NonSerialized]
        private Header header = new Header();

        /// <summary>
        /// Configuration of components.
        /// </summary>
        public Component[] Components
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration of event.
        /// </summary>
        public Event[] Event
        {
            get { return evt; }
            set { evt = value; }
        }
        [NonSerialized]
        private Event[] evt = new Event[] { };

        /// <summary>
        /// The event by type.
        /// </summary>
        /// <param name="type">Available event.</param>
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
    }
}
