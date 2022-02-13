/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Events.CommandEvents
{
    /// <summary>
    /// Specifies filters for ICommandEvent
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// For work with command ID
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Scope by GUID
        /// </summary>
        string Guid { get; set; }

        /// <summary>
        /// Filter by Custom input parameters
        /// </summary>
        object CustomIn { get; set; }

        /// <summary>
        /// Filter by Custom output parameters
        /// </summary>
        object CustomOut { get; set; }

        /// <summary>
        /// Cancel command if it's possible
        /// </summary>
        bool Cancel { get; set; }

        /// <summary>
        /// Use Before executing command
        /// </summary>
        bool Pre { get; set; }

        /// <summary>
        /// Use After executed command
        /// </summary>
        bool Post { get; set; }

        /// <summary>
        /// User note.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Ignore <see cref="CustomIn"/> if true.
        /// </summary>
        bool IgnoreCustomIn { get; set; }

        /// <summary>
        /// Ignore <see cref="CustomOut"/> if true.
        /// </summary>
        bool IgnoreCustomOut { get; set; }
    }
}