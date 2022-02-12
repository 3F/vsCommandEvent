/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Events.OWP
{
    [Guid("AF518505-C235-47CA-BA65-72EF8110E3B1")]
    public interface IMatching
    {
        /// <summary>
        /// Phrase for comparison.
        /// </summary>
        string Phrase { get; set; }

        /// <summary>
        /// How to compare.
        /// </summary>
        ComparisonType Type { get; set; }

        /// <summary>
        /// The Name of pane for Condition.
        /// </summary>
        string PaneName { get; set; }

        /// <summary>
        /// The Guid of pane for Condition.
        /// </summary>
        string PaneGuid { get; set; }
    }
}
