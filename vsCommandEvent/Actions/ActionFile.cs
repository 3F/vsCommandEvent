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

using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE.Actions
{
    /// <summary>
    /// Action for Files Mode
    /// </summary>
    public class ActionFile: ActionAbstract, IAction
    {
        /// <summary>
        /// Process for specified event.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <returns>Result of handling.</returns>
        public override bool process(ISolutionEvent evt)
        {
            string cFiles = ((IModeFile)evt.Mode).Command;

            cFiles = parse(evt, cFiles);
            shell(evt, treatNewlineAs(" & ", cFiles));

            return true;
        }

        /// <param name="cmd"></param>
        public ActionFile(ICommand cmd)
            : base(cmd)
        {

        }
    }
}
