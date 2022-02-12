/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Events.OWP
{
    public class Condition: IMatching
    {
        /// <summary>
        /// Phrase for comparison.
        /// </summary>
        public string Phrase
        {
            get;
            set;
        }

        /// <summary>
        /// How to compare.
        /// </summary>
        public ComparisonType Type
        {
            get;
            set;
        }

        /// <summary>
        /// The Name of pane for Condition.
        /// </summary>
        public string PaneName
        {
            get;
            set;
        }

        /// <summary>
        /// The Guid of pane for Condition.
        /// </summary>
        public string PaneGuid
        {
            get;
            set;
        }
    }
}
