/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Collections.Generic;

namespace net.r_eg.vsCE.Configuration.User
{
    public class Global: IGlobal
    {
        /// <inheritdoc cref="IGlobal.DebugMode"/>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Flag of ignoring configuration.
        /// </summary>
        public bool IgnoreConfiguration
        {
            get;
            set;
        }

        //[JsonProperty(TypeNameHandling = TypeNameHandling.None, ItemTypeNameHandling = TypeNameHandling.All)]
        public Dictionary<string, bool> LogIgnoreLevels { get; set; } = new Dictionary<string, bool>();
    }
}
