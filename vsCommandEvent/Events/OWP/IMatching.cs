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
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Events.OWP
{
    [Guid("AF518505-C235-47CA-BA65-72EF8110E3B1")]
    public interface IMatching
    {
        /// <summary>
        /// Phrase for comparison.
        /// </summary>
        string Phrase { get; set; }

        /// <summary>
        /// How to compare.
        /// </summary>
        ComparisonType Type { get; set; }

        /// <summary>
        /// The Name of pane for Condition.
        /// </summary>
        string PaneName { get; set; }

        /// <summary>
        /// The Guid of pane for Condition.
        /// </summary>
        string PaneGuid { get; set; }
    }
}
