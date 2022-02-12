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
    /// Specifies operations with items.
    /// </summary>
    [Guid("962E70D3-FBA4-4019-82AD-2473C45F7D7B")]
    public interface IItems
    {
        /// <summary>
        /// Get item for type and identifier.
        /// </summary>
        /// <param name="type">Type of item.</param>
        /// <param name="ident">Identifier of item.</param>
        /// <returns>Unspecified common item.</returns>
        IItem get(ItemType type, Ident ident);

        /// <summary>
        /// Get EW item for identifier.
        /// </summary>
        /// <param name="ident">Identifier of item.</param>
        /// <returns>EW item.</returns>
        IItemEW getEW(Ident ident);
    }
}
