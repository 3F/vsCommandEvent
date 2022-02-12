/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Receiver.Output
{
    /// <summary>
    /// Specifies types of available items.
    /// </summary>
    [Guid("3DD1EA27-A02E-4982-B71C-F72329CAC723")]
    public enum ItemType
    {
        /// <summary>
        /// The item based on errors/warnings container.
        /// </summary>
        EW,
    }
}
