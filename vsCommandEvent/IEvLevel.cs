/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
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
