/*
 * Copyright (c) 2013-2016  Denis Kuzmin (reg) <entry.reg@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Exceptions;

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

            throw new NotFoundException("getEvent: the event type '{0}' is not found.", type);
        }
    }
}
