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

namespace net.r_eg.vsSBE.Events.OWP
{
    public class Condition: IMatching
    {
        /// <summary>
        /// Phrase for comparison.
        /// </summary>
        public string Phrase
        {
            get;
            set;
        }

        /// <summary>
        /// How to compare.
        /// </summary>
        public ComparisonType Type
        {
            get;
            set;
        }

        /// <summary>
        /// The Name of pane for Condition.
        /// </summary>
        public string PaneName
        {
            get;
            set;
        }

        /// <summary>
        /// The Guid of pane for Condition.
        /// </summary>
        public string PaneGuid
        {
            get;
            set;
        }
    }
}
