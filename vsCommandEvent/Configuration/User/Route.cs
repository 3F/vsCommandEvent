/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE.Configuration.User
{
    public struct Route: IRoute
    {
        /// <summary>
        /// Identifier of Event.
        /// </summary>
        public SolutionEventType Event
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of Mode.
        /// </summary>
        public ModeType Mode
        {
            get;
            set;
        }
    }
}
