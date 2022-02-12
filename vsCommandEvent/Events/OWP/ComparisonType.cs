/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Events.OWP
{
    /// <summary>
    /// Available methods of comparison.
    /// </summary>
    [Guid("2E81983C-2370-46F4-B951-8B1E30B887EB")]
    public enum ComparisonType
    {
        /// <summary>
        /// Equality with case sensitive.
        /// </summary>
        Default,

        /// <summary>
        /// Using .NET Framework Regular Expressions.
        /// </summary>
        Regexp,

        /// <summary>
        /// Using common wildcards.
        /// </summary>
        Wildcards
    }
}
