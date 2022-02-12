/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.VSTools.OW
{
    [Guid("9C9CEFB5-BECE-4DB8-87EF-5C38AFA5EBD7")]
    public interface IPane
    {
        /// <summary>
        /// Gets the GUID for the pane.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Moves the focus to the current item.
        /// </summary>
        void Activate();

        /// <summary>
        /// Clears all text from pane.
        /// </summary>
        void Clear();

        /// <summary>
        /// Sends a text string into pane.
        /// </summary>
        /// <param name="text"></param>
        void OutputString(string text);
    }
}
