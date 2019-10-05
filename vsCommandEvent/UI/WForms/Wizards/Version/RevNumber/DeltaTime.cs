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
using System.Collections.Generic;
using System.Linq;

namespace net.r_eg.vsCE.UI.WForms.Wizards.Version.RevNumber
{
    internal sealed class DeltaTime: IRevNumber
    {
        /// <summary>
        /// Interval for time.
        /// </summary>
        public IntervalType interval = IntervalType.TotalMinutes;

        /// <summary>
        /// Basis for time.
        /// </summary>
        public DateTime timeBase = DateTime.Today;

        /// <summary>
        /// The type of this revision number.
        /// </summary>
        public Type Type
        {
            get { return Type.DeltaTime; }
        }

        /// <summary>
        /// Available types of intervals for 'Delta of time' method.
        /// </summary>
        public enum IntervalType
        {
            TotalMinutes,
            TotalHours,
            TotalSeconds,
            TotalDays,
            //TotalMilliseconds, :)
        }

        /// <summary>
        /// List of available types of intervals for 'Delta of time' method.
        /// </summary>
        public List<KeyValuePair<IntervalType, string>> IntervalTypeList
        {
            get;
            private set;
        }

        public DeltaTime()
        {
            IntervalTypeList = Enum.GetValues(typeof(IntervalType))
                                    .Cast<IntervalType>()
                                    .Select(v => new KeyValuePair<IntervalType, string>(v, v.ToString()))
                                    .ToList();
        }
    }
}
