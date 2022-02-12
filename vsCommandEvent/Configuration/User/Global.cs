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
        /// <summary>
        /// Debug mode for application.
        /// </summary>
        public bool DebugMode
        {
            get;
            set;
        }

        /// <summary>
        /// Flag of ignoring configuration.
        /// </summary>
        public bool IgnoreConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// List of levels for disabling from logger.
        /// </summary>
        //[JsonProperty(TypeNameHandling = TypeNameHandling.None, ItemTypeNameHandling = TypeNameHandling.All)]
        public Dictionary<string, bool> LogIgnoreLevels
        {
            get { return logIgnoreLevels; }
            set { logIgnoreLevels = value; }
        }
        private Dictionary<string, bool> logIgnoreLevels = new Dictionary<string, bool>();
    }
}
