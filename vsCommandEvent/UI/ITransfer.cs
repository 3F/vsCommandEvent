/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE.UI
{
    public interface ITransfer
    {
        /// <summary>
        /// Various commands such as a DTE, etc.
        /// </summary>
        void command(string data);

        /// <summary>
        /// Basic view of property by name/project.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="project"></param>
        void property(string name, string project = null);

        /// <summary>
        /// Provides the action by event type.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="cfg">The event configuration for action.</param>
        void action(SolutionEventType type, ISolutionEvent cfg);

        /// <summary>
        /// EnvDTE command.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="id"></param>
        /// <param name="customIn"></param>
        /// <param name="customOut"></param>
        /// <param name="description"></param>
        void command(string guid, int id, object customIn, object customOut, string description);
    }
}
