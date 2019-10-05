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

namespace net.r_eg.vsCE.SBEScripts
{
    public interface ISBEScript
    {
        /// <summary>
        /// Instance of used loader
        /// </summary>
        IBootloader Bootloader { get; }

        /// <summary>
        /// Handler of mixed data SBE-Scripts
        /// </summary>
        /// <param name="data">mixed data</param>
        /// <returns>prepared and evaluated data</returns>
        string parse(string data);

        /// <summary>
        /// Handler of mixed data SBE-Scripts
        /// </summary>
        /// <param name="data">mixed data</param>
        /// <param name="allowMSBuild">Allows post-processing with MSBuild or not. 
        /// Some components can require immediate processing with evaluation, before passing control to next level.
        /// </param>
        /// <returns>prepared and evaluated data</returns>
        string parse(string data, bool allowMSBuild);
    }
}
