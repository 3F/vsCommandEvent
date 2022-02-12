/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Configuration.User
{
    [Guid("D9460698-4331-4843-BDAC-F8C7C5F845A2")]
    public interface IGlobal
    {
        /// <summary>
        /// Debug mode for application.
        /// </summary>
        bool DebugMode { get; set; }

        /// <summary>
        /// Flag of ignoring configuration.
        /// </summary>
        bool IgnoreConfiguration { get; set; }

        /// <summary>
        /// List of levels for disabling from logger.
        /// </summary>
        Dictionary<string, bool> LogIgnoreLevels { get; set; }
    }
}
