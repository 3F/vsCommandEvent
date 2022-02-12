/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Bridge;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Main container of the 'Solution Events'
    /// </summary>
    [Guid("552AA0E0-9EFC-4394-B18B-41CF2F549FB3")]
    public interface ISolutionEvent
    {
        /// <summary>
        /// Status of activation.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Unique name for identification.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// About event.
        /// </summary>
        string Caption { get; set; }

        /// <summary>
        /// Support of the MSBuild engine.
        /// </summary>
        bool SupportMSBuild { get; set; }

        /// <summary>
        /// Support of the SBE-Scripts engine.
        /// </summary>
        bool SupportSBEScripts { get; set; }

        /// <summary>
        /// The context of action.
        /// </summary>
        BuildType BuildType { get; set; }

        /// <summary>
        /// User interaction.
        /// Waiting until user presses yes/no etc,
        /// </summary>
        bool Confirmation { get; set; }

        /// <summary>
        /// Used mode.
        /// </summary>
        IMode Mode { get; set; }

        /// <summary>
        /// Handling process.
        /// </summary>
        IEventProcess Process { get; set; }

        /// <summary>
        /// Unique identifier at runtime.
        /// </summary>
        Guid Id { get; }
    }
}
