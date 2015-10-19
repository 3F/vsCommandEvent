/*
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

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Events.CommandEvents;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Specifies work with CommandEvents from EnvDTE
    /// </summary>
    [Guid("F013837B-B624-44B0-B54F-7F7F988C9795")]
    public interface ICommandEvent: ISolutionEvent 
    {
        /// <summary>
        /// Conditions from EnvDTE commands.
        /// </summary>
        IFilter[] Filters { get; set; }
    }
}