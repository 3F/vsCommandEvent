/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Events.Commands
{
    /// <summary>
    /// Specifies basic fields for command list.
    /// </summary>
    [Guid("2229F68B-0FC7-46E3-953A-AFC53650DA63")]
    public interface ICommandArray<T>
    {
        /// <summary>
        /// Atomic commands for handling.
        /// </summary>
        T[] Command { get; set; }
    }
}
