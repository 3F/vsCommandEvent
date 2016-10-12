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

using Newtonsoft.Json;

namespace net.r_eg.vsCE.Configuration.User
{
    public class UserValue: IUserValue
    {
        /// <summary>
        /// Type of link to external value.
        /// </summary>
        public LinkType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Guid of external node.
        /// </summary>
        public string Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Manager of accessing to remote value.
        /// </summary>
        [JsonIgnore]
        public IManager Manager
        {
            get
            {
                if(manager == null) {
                    manager = new Manager(this);
                }
                return manager;
            }
        }
        private IManager manager;


        /// <summary>
        /// Initialize with new Guid and specific LinkType.
        /// </summary>
        /// <param name="Type">Type of link to external value.</param>
        public UserValue(LinkType Type)
        {
            Guid        = System.Guid.NewGuid().ToString();
            this.Type   = Type;
        }

        public UserValue()
        {

        }
    }
}