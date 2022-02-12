/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Events.Types;

namespace net.r_eg.vsCE.Actions
{
    /// <summary>
    /// Action for EnvCommand Mode
    /// </summary>
    public class ActionEnvCommand: ActionAbstract, IAction
    {
        /// <summary>
        /// Process for specified event.
        /// </summary>
        /// <param name="evt">Configured event.</param>
        /// <returns>Result of handling.</returns>
        public override bool process(ISolutionEvent evt)
        {
            IModeEnvCommand mode = (IModeEnvCommand)evt.Mode;
            if(mode.Command == null || mode.Command.Length < 1) {
                return true;
            }

            if(!evt.SupportMSBuild && !evt.SupportSBEScripts) {
                return raise(mode.Command, mode.AbortOnFirstError);
            }
            
            var parsed = new CommandDte[mode.Command.Length];
            for(int i = 0; i < mode.Command.Length; ++i)
            {
                parsed[i] = new CommandDte()
                {
                    Guid        = parse(evt, mode.Command[i].Guid),
                    Id          = mode.Command[i].Id,
                    CustomIn    = parse(mode.Command[i].CustomIn, evt),
                    CustomOut   = parse(mode.Command[i].CustomOut, evt)
                };
            }
            
            return raise(parsed, mode.AbortOnFirstError);
        }

        /// <param name="cmd"></param>
        public ActionEnvCommand(ICommand cmd)
            : base(cmd)
        {

        }

        /// <summary>
        /// Raise commands for EnvDTE.
        /// </summary>
        /// <param name="commands">Atomic DTE-commands for handling.</param>
        /// <param name="abortOnFirstError">Abort commands on first error if true.</param>
        /// <returns>true value if all raised without errors.</returns>
        protected bool raise(CommandDte[] commands, bool abortOnFirstError)
        {
            bool success = true;
            foreach(CommandDte c in commands)
            {
                object cin  = c.CustomIn;
                object cout = c.CustomOut;

                try {
                    cmd.Env.raise(c.Guid, c.Id, ref cin, ref cout);
                }
                catch(Exception ex)
                {
                    Log.Warn("Action-EnvCommand: problem with command: '{0}', '{1}' :: '{2}'", c.Guid, c.Id, ex.Message);
                    if(abortOnFirstError) {
                        return false;
                    }
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// Parse object data.
        /// </summary>
        /// <param name="oin">Object for parsing inc. arrays.</param>
        /// <param name="evt"></param>
        /// <returns></returns>
        protected object parse(object oin, ISolutionEvent evt)
        {
            if(oin == null) {
                return null;
            }

            if(oin is string) {
                return parse(evt, (string)oin);
            }

            if(!oin.GetType().IsArray) {
                return oin;
            }

            object[] oinArr = (object[])oin;
            object[] ret    = new object[oinArr.Length];

            for(int i = 0; i < oinArr.Length; ++i) {
                ret[i] = parse(oinArr[i], evt);
            }
            return ret;
        }
    }
}
