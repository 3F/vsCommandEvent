/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Events.Commands;
using net.r_eg.vsCE.Events.Types;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Processing with environment of Visual Studio
    /// </summary>
    [Guid("1349D4E7-907A-4670-A752-7422FF0A0892")]
    public interface IModeOperation: ICommandArray<Command>
    {
        /// <summary>
        /// Abort operations on first error
        /// </summary>
        bool AbortOnFirstError { get; set; }
    }
}
