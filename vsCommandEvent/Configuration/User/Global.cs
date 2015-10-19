﻿/*
 * Copyright (c) 2013-2015  Denis Kuzmin (reg) <entry.reg@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using Newtonsoft.Json;

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
