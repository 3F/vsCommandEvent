/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
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
