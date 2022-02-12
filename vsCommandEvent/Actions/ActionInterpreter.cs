/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Exceptions;

namespace net.r_eg.vsCE.Actions
{
    /// <summary>
    /// Action for Interpreter Mode
    /// </summary>
    public class ActionInterpreter: ActionAbstract, IAction
    {
        /// <summary>
        /// Process for specified event.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <returns>Result of handling.</returns>
        public override bool process(ISolutionEvent evt)
        {
            if(((IModeInterpreter)evt.Mode).Handler.Trim().Length < 1) {
                throw new CompilerException("Interpreter: Handler is empty or not selected.");
            }

            string script   = ((IModeInterpreter)evt.Mode).Command;
            string wrapper  = ((IModeInterpreter)evt.Mode).Wrapper;

            script = parse(evt, script);
            script = treatNewlineAs(((IModeInterpreter)evt.Mode).Newline, script);

            switch(wrapper.Length) {
                case 1: {
                    script = string.Format("{0}{1}{0}", wrapper, script.Replace(wrapper, "\\" + wrapper));
                    break;
                }
                case 2: {
                    //pair as: (), {}, [] ...
                    //e.g.: (echo str&echo.&echo str) >> out
                    string wL = wrapper[0].ToString();
                    string wR = wrapper[1].ToString();
                    script = string.Format("{0}{1}{2}", wL, script.Replace(wL, "\\" + wL).Replace(wR, "\\" + wR), wR);
                    break;
                }
            }

            string handler = ((IModeInterpreter)evt.Mode).Handler;
            if(evt.SupportMSBuild) {
                handler = cmd.MSBuild.Eval(handler);
            }

            shell(evt, string.Format("{0} {1}", handler, script));
            return true;
        }

        /// <param name="cmd"></param>
        public ActionInterpreter(ICommand cmd)
            : base(cmd)
        {

        }
    }
}
