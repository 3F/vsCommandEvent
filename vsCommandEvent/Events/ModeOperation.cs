/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using Newtonsoft.Json;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Processing with Environment of Visual Studio.
    /// </summary>
    public class ModeOperation: IMode, IModeOperation
    {
        /// <summary>
        /// Type of implementation.
        /// </summary>
        public ModeType Type
        {
            get { return ModeType.Operation; }
        }

        /// <summary>
        /// Atomic commands for handling.
        /// </summary>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public Types.Command[] Command
        {
            get;
            set;
        }

        /// <summary>
        /// Abort operations on first error.
        /// </summary>
        public bool AbortOnFirstError
        {
            get { return abortOnFirstError; }
            set { abortOnFirstError = value; }
        }
        private bool abortOnFirstError = false;

        /// <param name="command"></param>
        public ModeOperation(Types.Command[] command)
        {
            Command = command;
        }

        public ModeOperation()
        {

        }
    }
}
