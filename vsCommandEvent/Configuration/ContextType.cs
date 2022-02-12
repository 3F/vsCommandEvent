/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Configuration
{
    [Guid("A55E2432-81B0-407E-B3B2-29958D76B09A")]
    public enum ContextType
    {
        /// <summary>
        /// Common configuration.
        /// </summary>
        Common,

        /// <summary>
        /// Configuration of specific solution.
        /// </summary>
        Solution,

        /// <summary>
        /// Unspecified static configuration.
        /// </summary>
        Static,
    }
}
