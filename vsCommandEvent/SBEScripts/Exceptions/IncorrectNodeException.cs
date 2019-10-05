﻿/*
 * Copyright (c) 2015,2016,2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
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

using System;
using System.Diagnostics;
using net.r_eg.vsCE.SBEScripts.SNode;

namespace net.r_eg.vsCE.SBEScripts.Exceptions
{
    [Serializable]
    public class IncorrectNodeException: OperationNotFoundException
    {
        public IncorrectNodeException()
            : base(String.Empty)
        {

        }

        public IncorrectNodeException(string message)
            : base(message)
        {

        }

        public IncorrectNodeException(string message, params object[] args)
            : base(message, args)
        {

        }

        /// <summary>
        /// Throw error with IPM
        /// </summary>
        /// <param name="pm"></param>
        /// <param name="level">Selected level.</param>
        public IncorrectNodeException(IPM pm, int level = 0)
            : this()
        {
            if(pm != null) {
                pm.fail(level, (new StackTrace()).GetFrame(1).GetMethod().Name);
            }
        }
    }
}
