/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using net.r_eg.vsCE.Bridge;
using net.r_eg.vsCE.Events.CommandEvents;
using net.r_eg.vsCE.Events.OWP;
using Newtonsoft.Json;

namespace net.r_eg.vsCE.Events
{
    public class Event: ISolutionEvent, ISolutionEventOWP, ICommandEvent
    {
        private Guid id = Guid.NewGuid();

        /// <inheritdoc cref="ISolutionEvent.Enabled"/>
        public bool Enabled { get; set; } = true;

        /// <inheritdoc cref="ISolutionEvent.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="ISolutionEvent.Caption"/>
        public string Caption { get; set; } = string.Empty;

        /// <inheritdoc cref="ISolutionEvent.SupportMSBuild"/>
        public bool SupportMSBuild { get; set; } = true;

        /// <inheritdoc cref="ISolutionEvent.SupportSBEScripts"/>
        public bool SupportSBEScripts { get; set; } = true;

        /// <inheritdoc cref="ISolutionEvent.BuildType"/>
        public BuildType BuildType { get; set; } = BuildType.Common;

        /// <inheritdoc cref="ISolutionEvent.Confirmation"/>
        public bool Confirmation { get; set; } = false;

        /// <inheritdoc cref="ISolutionEvent.Process"/>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IEventProcess Process
        {
            get { return process; }
            set { process = (EventProcess)value; }
        }
        private EventProcess process = new EventProcess();

        /// <inheritdoc cref="ISolutionEvent.Mode"/>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IMode Mode { get; set; } = new ModeFile();

        /// <inheritdoc cref="ICommandEvent.Filters"/>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IFilter[] Filters { get; set; }

        /// <inheritdoc cref="ISolutionEventOWP.Match"/>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All, ItemTypeNameHandling = TypeNameHandling.All)]
        public IMatching[] Match { get; set; }

        /// <inheritdoc cref="ISolutionEvent.Id"/>
        [JsonIgnore]
        public Guid Id => id;

        public bool ShouldSerializeEnabled() => !Enabled;
        public bool ShouldSerializeCaption() => !string.IsNullOrEmpty(Caption);
        public bool ShouldSerializeSupportMSBuild() => !SupportMSBuild;
        public bool ShouldSerializeSupportSBEScripts() => !SupportSBEScripts;
        public bool ShouldSerializeBuildType() => BuildType != BuildType.Common;
        public bool ShouldSerializeConfirmation() => Confirmation;
        public bool ShouldSerializeProcess() => new EventProcess() != Process as EventProcess;
    }
}
