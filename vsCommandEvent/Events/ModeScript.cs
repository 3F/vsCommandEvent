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
    /// Scripts from available engines like MSBuild, etc.
    /// </summary>
    public class ModeScript: ModeCommand, IMode, IModeScript
    {
        /// <summary>
        /// Type of implementation
        /// </summary>
        public ModeType Type
        {
            get { return ModeType.Script; }
        }
    }
}
