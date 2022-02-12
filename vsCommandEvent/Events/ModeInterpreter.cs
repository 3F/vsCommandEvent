/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.vsCE.Events.Mapping.Json;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Processing with streaming tools
    /// </summary>
    public class ModeInterpreter: ModeCommand, IMode, IModeInterpreter
    {
        /// <summary>
        /// Type of implementation
        /// </summary>
        public ModeType Type
        {
            get { return ModeType.Interpreter; }
        }

        /// <summary>
        /// Stream handler
        /// </summary>
        public string Handler
        {
            get { return handler; }
            set { handler = value; }
        }
        private string handler = string.Empty;

        /// <summary>
        /// Treat newline as
        /// </summary>
        public string Newline
        {
            get { return newline; }
            set { newline = value; }
        }
        private string newline = string.Empty;

        /// <summary>
        /// Symbol/s for wrapping of commands
        /// </summary>
        public string Wrapper
        {
            get { return wrapper; }
            set { wrapper = value; }
        }
        private string wrapper = string.Empty;
    }
}
