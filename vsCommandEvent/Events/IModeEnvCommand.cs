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
    /// Processing with EnvDTE Commands of Visual Studio.
    /// </summary>
    [Guid("B29B9762-4316-4562-B0EB-266EAC8338AE")]
    public interface IModeEnvCommand: ICommandArray<CommandDte>
    {
        /// <summary>
        /// Abort commands on first error
        /// </summary>
        bool AbortOnFirstError { get; set; }
    }
}
