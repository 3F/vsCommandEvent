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

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsSBE.Configuration.User
{
    [Guid("B305C9E9-F900-4F86-A4B8-BF57A97A9AEA")]
    public interface ICacheHeader
    {
        /// <summary>
        /// When has been updated.
        /// UTC as a rule.
        /// </summary>
        long Updated { get; set; }

        /// <summary>
        /// Type of hashing.
        /// </summary>
        HashType Algorithm { get; set; } 

        /// <summary>
        /// Hash value of data.
        /// </summary>
        string Hash { get; set; }
    }
}
