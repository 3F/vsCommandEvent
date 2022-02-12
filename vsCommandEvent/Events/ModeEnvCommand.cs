/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.vsCE.Events.Types;
using Newtonsoft.Json;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Processing with EnvDTE Commands of Visual Studio.
    /// </summary>
    public class ModeEnvCommand: IMode, IModeEnvCommand
    {
        /// <summary>
        /// Type of implementation.
        /// </summary>
        public ModeType Type
        {
            get { return ModeType.EnvCommand; }
        }

        /// <summary>
        /// Atomic DTE-commands for handling.
        /// </summary>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public CommandDte[] Command
        {
            get;
            set;
        }

        /// <summary>
        /// Abort operations on the first error
        /// </summary>
        public bool AbortOnFirstError
        {
            get { return abortOnFirstError; }
            set { abortOnFirstError = value; }
        }
        private bool abortOnFirstError = true;

        /// <param name="command"></param>
        public ModeEnvCommand(CommandDte[] command)
        {
            Command = command;
        }

        public ModeEnvCommand()
        {

        }
    }
}
