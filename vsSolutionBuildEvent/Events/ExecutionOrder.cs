﻿/*
 * Copyright (c) 2013-2016,2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
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

namespace net.r_eg.vsSBE.Events
{
    public class ExecutionOrder: IExecutionOrder
    {
        public const string FIRST_PROJECT   = ":?: First Project";
        public const string LAST_PROJECT    = ":?: Last Project";
        public const string FIRST_TYPE      = ":?: First Type";
        public const string LAST_TYPE       = ":?: Last Type";

        /// <summary>
        /// Project name
        /// </summary>
        public string Project
        {
            get;
            set;
        }

        /// <summary>
        /// Range of execution
        /// </summary>
        public ExecutionOrderType Order
        {
            get;
            set;
        }

        /// <summary>
        /// Checks name for special types.
        /// </summary>
        /// <param name="name">Project name.</param>
        /// <returns></returns>
        public static bool IsSpecial(string name)
        {
            return name == FIRST_PROJECT
                    || name == LAST_PROJECT
                    || name == FIRST_TYPE
                    || name == LAST_TYPE;
        }
    }
}
