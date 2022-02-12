/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Configuration.User
{
    [Guid("BF646E0B-0E9C-460A-9236-2177E971C1DA")]
    public interface ICommon
    {
        /// <summary>
        /// Word wrapping for main editor.
        /// </summary>
        bool WordWrap { get; set; }

        /// <summary>
        /// Value of zooming for main editor.
        /// </summary>
        int Zoomed { get; set; }
    }
}
