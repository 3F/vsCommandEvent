/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Events
{
    [Guid("905CFFE9-1C44-449A-939E-B0ABC4E871C5")]
    public interface IMode
    {
        /// <summary>
        /// Used type from available modes
        /// </summary>
        ModeType Type { get; }
    }
}
