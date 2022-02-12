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
    /// Action for Script Mode
    /// </summary>
    public class ActionScript: ActionAbstract, IAction
    {
        /// <summary>
        /// Process for specified event.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <returns>Result of handling.</returns>
        public override bool process(ISolutionEvent evt)
        {
            parse(evt, ((IModeScript)evt.Mode).Command);
            return true;
        }

        /// <param name="cmd"></param>
        public ActionScript(ICommand cmd)
            : base(cmd)
        {

        }
    }
}
