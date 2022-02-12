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
    /// Specifies basic item.
    /// </summary>
    [Guid("38C1F903-2584-4D1D-98A3-922A953280C8")]
    public interface IItem
    {
        /// <summary>
        /// Gets current raw data or sets new.
        /// </summary>
        string Raw { get; set; }

        /// <summary>
        /// Updating raw data.
        /// </summary>
        /// <param name="data"></param>
        void updateRaw(string data);
    }
}
