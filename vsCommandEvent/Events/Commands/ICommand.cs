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
    /// Specifies basic fields for command.
    /// </summary>
    [Guid("F80C5586-6157-408A-9029-80FE2FC851B3")]
    public interface ICommand<T>
    {
        /// <summary>
        /// Main command for handling.
        /// </summary>
        T Command { get; set; }
    }

    [Guid("E87D1386-1A8A-40E5-9379-D7790863FC90")]
    public interface ICommand: ICommand<string>
    {

    }
}
