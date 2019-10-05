/*
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
using net.r_eg.vsCE.Events.Commands;
using net.r_eg.vsCE.Events.Types;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Processing with EnvDTE Commands of Visual Studio.
    /// </summary>
    [Guid("B29B9762-4316-4562-B0EB-266EAC8338AE")]
    public interface IModeEnvCommand: ICommandArray<CommandDte>
    {
        /// <summary>
        /// Abort commands on first error
        /// </summary>
        bool AbortOnFirstError { get; set; }
    }
}
