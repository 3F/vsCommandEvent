/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.ComponentModel;
using net.r_eg.vsCE.Events.Commands;
using Newtonsoft.Json;

namespace net.r_eg.vsCE.Events.Mapping.Json
{
    /// <summary>
    /// Adds convenient and compatible way to working with ICommand objects for Json serializers.
    /// </summary>
    public abstract class ModeCommand: ICommand
    {
        /// <summary>
        /// Command for handling.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore] // "Command" was never released for vsCE. Only for vsSBE but removed after 1.14.0
        public string Command
        {
            get => command;
            set
            {
                if(value != null) {
                    _command = value.Replace("\r\n", "\n").Split('\n');
                }
                command = value;
            }
        }
        protected string command = String.Empty;

        /// <summary>
        /// Readable or friendly  'Command' property as wrapper of original.
        /// </summary>
        [JsonProperty(PropertyName = "Command")]
        protected string[] _Command
        {
            get => _command;
            set
            {
                if(value != null) {
                    command = String.Join("\n", value);
                }
                _command = value;
            } 
        }
        protected string[] _command;
    }
}
