/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Events.Types
{
    /// <summary>
    /// Simple commands with format like this:
    /// https://msdn.microsoft.com/en-us/library/envdte._dte.executecommand.aspx
    /// </summary>
    public struct Command
    {
        /// <summary>
        /// Name of command.
        /// </summary>
        public string name;

        /// <summary>
        /// Arguments of command.
        /// </summary>
        public string args;
    }
}
