/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Runtime.InteropServices;
using net.r_eg.EvMSBuild;
using net.r_eg.SobaScript;
using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE.Actions
{
    [Guid("8E774159-2221-4435-A6C9-A40B78A369FD")]
    public interface ICommand
    {
        /// <summary>
        /// SBE-Scripts core
        /// </summary>
        ISobaScript SBEScript { get; }

        /// <summary>
        /// MSBuild core
        /// </summary>
        IEvMSBuild MSBuild { get; }

        /// <summary>
        /// Used environment
        /// </summary>
        IEnvironment Env { get; }

        /// <summary>
        /// Specified Event type.
        /// </summary>
        SolutionEventType EventType { get; }

        /// <summary>
        /// Find and execute action by specified event.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <param name="type">The type of event.</param>
        /// <returns>true value if it was handled.</returns>
        bool exec(ISolutionEvent evt, SolutionEventType type);

        /// <summary>
        /// Find and execute action with default event type.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <returns>true value if it was handled.</returns>
        bool exec(ISolutionEvent evt);
    }
}
