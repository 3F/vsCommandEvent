/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Events.CommandEvents;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Specifies work with CommandEvents from EnvDTE
    /// </summary>
    [Guid("F013837B-B624-44B0-B54F-7F7F988C9795")]
    public interface ICommandEvent: ISolutionEvent 
    {
        /// <summary>
        /// Conditions from EnvDTE commands.
        /// </summary>
        IFilter[] Filters { get; set; }
    }
}