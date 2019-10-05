/*
 * Copyright (c) 2015,2016,2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
