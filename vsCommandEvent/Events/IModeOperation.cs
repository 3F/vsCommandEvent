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

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Events.Commands;
using net.r_eg.vsCE.Events.Types;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Processing with environment of Visual Studio
    /// </summary>
    [Guid("1349D4E7-907A-4670-A752-7422FF0A0892")]
    public interface IModeOperation: ICommandArray<Command>
    {
        /// <summary>
        /// Abort operations on the first error
        /// </summary>
        bool AbortOnFirstError { get; set; }
    }
}