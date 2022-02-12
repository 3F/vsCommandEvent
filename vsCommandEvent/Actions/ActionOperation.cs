/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.vsCE.Events;
using ECommand = net.r_eg.vsCE.Events.Types.Command;

namespace net.r_eg.vsCE.Actions
{
    /// <summary>
    /// Action for Operation Mode
    /// </summary>
    public class ActionOperation: ActionAbstract, IAction
    {
        /// <summary>
        /// Process for specified event.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <returns>Result of handling.</returns>
        public override bool process(ISolutionEvent evt)
        {
            IModeOperation operation = (IModeOperation)evt.Mode;
            if(operation.Command == null || operation.Command.Length < 1) {
                return true;
            }
            DTEOperation dteo = new DTEOperation(cmd.Env, cmd.EventType);

            if(!evt.SupportMSBuild && !evt.SupportSBEScripts) {
                dteo.exec(operation.Command, operation.AbortOnFirstError);
                return true;
            }

            // need evaluation for data

            ECommand[] parsed = new ECommand[operation.Command.Length];
            for(int i = 0; i < operation.Command.Length; ++i)
            {
                parsed[i].name = parse(evt, operation.Command[i].name);
                parsed[i].args = parse(evt, operation.Command[i].args);
            }

            dteo.exec(parsed, operation.AbortOnFirstError);
            return true;
        }

        /// <param name="cmd"></param>
        public ActionOperation(ICommand cmd)
            : base(cmd)
        {

        }
    }
}
