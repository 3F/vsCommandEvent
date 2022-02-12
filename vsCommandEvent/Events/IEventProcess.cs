/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Declaring of handling process
    /// </summary>
    public interface IEventProcess
    {
        /// <summary>
        /// Waiting completion
        /// </summary>
        bool Waiting { get; set; }

        /// <summary>
        /// Hiding of processing or not
        /// </summary>
        bool Hidden { get; set; }

        /// <summary>
        /// How long to wait the execution, in seconds. 0 value - infinitely
        /// </summary>
        int TimeLimit { get; set; }
    }
}
