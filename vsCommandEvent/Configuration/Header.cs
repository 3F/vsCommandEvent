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

namespace net.r_eg.vsCE.Configuration
{
    public class Header
    {
        public string[] _ = new string[]
        {
            " Current file requires a vsCommandEvent engine.", 
            " Free plugin for Visual Studio:",
            "  * https://visualstudiogallery.msdn.microsoft.com/ad9f19b2-04c0-46fe-9637-9a52ce4ca661/",
            "  * http://vsce.r-eg.net",
            " Feedback: entry.reg@gmail.com"
        };

        /// <summary>
        /// Compatibility of configurations.
        /// </summary>
        public string Compatibility
        {
            get { return compatibility; }
            set { compatibility = value; }
        }
        /// <summary>
        /// The version below used by default if current attr is not found.
        /// </summary>
        private string compatibility = "1.0";

    }
}
