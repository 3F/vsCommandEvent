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
using net.r_eg.vsCE.Configuration;

namespace net.r_eg.vsCE
{
    public interface IEvLevel
    {
        /// <summary>
        /// When the solution has been opened or created
        /// </summary>
        event EventHandler OpenedSolution;

        /// <summary>
        /// When the solution has been closed
        /// </summary>
        event EventHandler ClosedSolution;

        /// <summary>
        /// Used Environment
        /// </summary>
        IEnvironment Environment { get; }

        /// <summary>
        /// Binder of action
        /// </summary>
        Actions.Binder Action { get; }

        /// <summary>
        /// Manager of configurations.
        /// </summary>
        IManager ConfigManager { get; }

        /// <summary>
        /// Solution has been opened.
        /// </summary>
        /// <param name="pUnkReserved">Reserved for future use.</param>
        /// <param name="fNewSolution">true if the solution is being created. false if the solution was created previously or is being loaded.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        int solutionOpened(object pUnkReserved, int fNewSolution);

        /// <summary>
        /// Solution has been closed.
        /// </summary>
        /// <param name="pUnkReserved">Reserved for future use.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        int solutionClosed(object pUnkReserved);
    }
}
