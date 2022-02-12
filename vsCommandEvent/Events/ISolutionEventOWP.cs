/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Events.OWP;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Support of the OutputWindowPane
    /// </summary>
    [Guid("C27A1E8C-7808-4529-BAC4-E8322D4F11CD")]
    public interface ISolutionEventOWP: ISolutionEvent
    {
        /// <summary>
        /// List of statements.
        /// </summary>
        IMatching[] Match { get; set; }
    }
}