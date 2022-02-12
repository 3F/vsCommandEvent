/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Runtime.InteropServices;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE
{
    [Guid("8DFEA125-47CC-46A5-9A70-8202372A0680")]
    public interface ISolutionEvents
    {
        /// <summary>
        /// Header of information.
        /// </summary>
        Header Header { get; set; }

        /// <summary>
        /// Configuration of components.
        /// </summary>
        Component[] Components { get; set; }

        /// <summary>
        /// Configuration of event.
        /// </summary>
        Event[] Event { get; set; }

        /// <summary>
        /// Getting event by type.
        /// </summary>
        /// <param name="type">Available event.</param>
        ISolutionEvent[] getEvent(SolutionEventType type);
    }
}
