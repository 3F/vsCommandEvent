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
    /// <summary>
    /// Specifies basic manager of accessing to remote value.
    /// </summary>
    [Guid("7E740672-7D27-4180-A5F7-0A21A0329D3A")]
    public interface IManager
    {
        /// <summary>
        /// Unspecified raw value.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Trying to get value as ICacheHeader.
        /// </summary>
        ICacheHeader CacheHeader { get; }

        /// <summary>
        /// To erase current value from common data.
        /// </summary>
        void unset();

        /// <summary>
        /// Reset data from value.
        /// </summary>
        void reset();
    }
}
