/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Events
{
    public class EventProcess: IEventProcess
    {
        /// <summary>
        /// Waiting completion
        /// </summary>
        public bool Waiting
        {
            get { return waiting; }
            set { waiting = value; }
        }
        private bool waiting = true;

        /// <summary>
        /// Hiding of processing or not
        /// </summary>
        public bool Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }
        private bool hidden = true;

        /// <summary>
        /// How long to wait the execution, in seconds. 0 value - infinitely
        /// </summary>
        public int TimeLimit
        {
            get { return timeLimit; }
            set { timeLimit = value; }
        }
        private int timeLimit = 30;
    }
}
