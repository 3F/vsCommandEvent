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

namespace net.r_eg.vsCE.SBEScripts.Dom
{
    /// <summary>
    /// Type of used element in node
    /// </summary>
    public enum InfoType
    {
        /// <summary>
        /// Basic type
        /// </summary>
        Unspecified,

        /// <summary>
        /// Defaults for DefinitionAttribute
        /// </summary>
        Component,

        /// <summary>
        /// Unspecified complex expression/construction
        /// </summary>
        Definition,

        /// <summary>
        /// Defaults for MethodAttribute
        /// </summary>
        Method,

        /// <summary>
        /// Defaults for PropertyAttribute
        /// </summary>
        Property,

        /// <summary>
        /// Another variant of existing Component
        /// </summary>
        AliasToComponent,

        /// <summary>
        /// Another variant of existing Definition
        /// </summary>
        AliasToDefinition,
    }
}
