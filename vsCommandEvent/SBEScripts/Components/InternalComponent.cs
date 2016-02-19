﻿/*
 * Copyright (c) 2013-2016  Denis Kuzmin (reg) <entry.reg@gmail.com>
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

using System;
using net.r_eg.vsCE.Actions;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Exceptions;
using net.r_eg.vsCE.SBEScripts.Dom;
using net.r_eg.vsCE.SBEScripts.Exceptions;
using net.r_eg.vsCE.SBEScripts.SNode;

namespace net.r_eg.vsCE.SBEScripts.Components
{
    /// <summary>
    /// All internal operations with vsCE
    /// </summary>
    [Component("vsCE", new string[] { "Core" }, "All internal operations with vsCE")]
    public class InternalComponent: Component, IComponent
    {
        /// <summary>
        /// Ability to work with data for current component
        /// </summary>
        public override string Condition
        {
            get { return @"(?:vsCE|Core)\s"; }
        }

        /// <summary>
        /// Use regex engine
        /// </summary>
        public override bool CRegex
        {
            get { return true; }
        }

        /// <summary>
        /// Handler for current data
        /// </summary>
        /// <param name="data">mixed data</param>
        /// <returns>prepared and evaluated data</returns>
        public override string parse(string data)
        {
            var point       = entryPoint(data);
            string subtype  = point.Key;
            string request  = point.Value;

            Log.Trace("`{0}`: subtype - `{1}`, request - `{2}`", ToString(), subtype, request);

            switch(subtype) {
                case "events": {
                    return stEvents(new PM(request));
                }
            }

            throw new SubtypeNotFoundException("Subtype `{0}` is not found", subtype);
        }

        /// <summary>
        /// Work with the events node - `events.Type`
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Property("events", "Work with events")]
        [Property("Pre", "Pre-Build\nBefore assembling", "events", "stEvents"), Property("", "Pre", "stEvents")]
        [Property("Post", "Post-Build\nAfter assembling", "events", "stEvents"), Property("", "Post", "stEvents")]
        [Property("Cancel", "Cancel-Build\nby user or when occurs error", "events", "stEvents"), Property("", "Cancel", "stEvents")]
        [Property("CommandEvent", "CommandEvent (DTE)\nAll Command Events from EnvDTE", "events", "stEvents"), Property("", "CommandEvent", "stEvents")]
        [Property("Warnings", "Warnings-Build\nWarnings during assembly processing", "events", "stEvents"), Property("", "Warnings", "stEvents")]
        [Property("Errors", "Errors-Build\nErrors during assembly processing", "events", "stEvents"), Property("", "Errors", "stEvents")]
        [Property("OWP", "Output-Build customization\nFull control", "events", "stEvents"), Property("", "OWP", "stEvents")]
        [Property("Transmitter", "Transmitter\nTransmission of the build-data to outer handler", "events", "stEvents"), Property("", "Transmitter", "stEvents")]
        [Property("Logging", "Logging\nAll processes with internal logging", "events", "stEvents"), Property("", "Logging", "stEvents")]
        protected string stEvents(IPM pm)
        {
            if(!pm.Is(LevelType.Property, "events")) {
                throw new IncorrectNodeException(pm);
            }

            ILevel etype = pm.Levels[1];
            if(etype.Type != LevelType.Property) {
                throw new IncorrectNodeException(pm, 1);
            }

            if(pm.Is(2, LevelType.Method, "item"))
            {
                SolutionEventType type;
                try {
                    type = (SolutionEventType)Enum.Parse(typeof(SolutionEventType), etype.Data);
                }
                catch(ArgumentException) {
                    throw new OperandNotFoundException("The event type `{0}` was not found.", etype.Data);
                }
                return stEventItem(type, pm.pinTo(2));
            }

            throw new IncorrectNodeException(pm, 2);
        }

        /// <summary>
        /// Work with event-item node.
        ///     `events.Type.item(string name | integer index)`
        /// </summary>
        /// <param name="type">Type of available events</param>
        /// <param name="pm"></param>
        /// <returns>evaluated data</returns>
        [Method(
                "item", 
                "Event item by name", 
                "", "stEvents", 
                new string[] { "name" }, 
                new string[] { "Name of the event" }, 
                CValueType.Void, 
                CValueType.String
        )]
        [Method(
                "item", 
                "Event item by index", 
                "", 
                "stEvents", 
                new string[] { "index" }, 
                new string[] { "Index of the event >= 1" },
                CValueType.Void, 
                CValueType.Integer
        )]
        protected string stEventItem(SolutionEventType type, IPM pm)
        {
            ILevel level = pm.Levels[0];

            int index = -1;
            ISolutionEvent evt;

            if(level.Is(ArgumentType.StringDouble)) {
                evt = getEventByName(type, (string)level.Args[0].data, out index);
            }
            else if(level.Is(ArgumentType.Integer))
            {
                index   = (int)level.Args[0].data;
                evt     = getEventByIndex(type, index);
            }
            else {
                throw new InvalidArgumentException("Incorrect arguments to `item( string name | integer index )`");
            }

            // .item(...).

            if(pm.Is(1, LevelType.Property, "Enabled")) {
                return pEnabled(evt, pm.pinTo(2));
            }
            if(pm.Is(1, LevelType.Property, "Status")) {
                return itemStatus(type, index, pm.pinTo(1));
            }
            if(pm.Is(1, LevelType.Property, "stdout")) {
                return pStdout(evt, pm.pinTo(2));
            }
            if(pm.Is(1, LevelType.Property, "stderr")) {
                return pStderr(evt, pm.pinTo(2));
            }

            throw new IncorrectNodeException(pm, 1);
        }

        /// <summary>
        /// `item(...).Status`
        /// `item(...).Status.HasErrors`
        /// </summary>
        /// <param name="type">Selected event type.</param>
        /// <param name="index">Access by index.</param>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Property("Status", "Available statuses for selected event-item.", "item", "stEventItem")]
        [Property("HasErrors", "Checking existence of errors after executed action for selected event-item.", "Status", "itemStatus", CValueType.Boolean)]
        protected string itemStatus(SolutionEventType type, int index, IPM pm)
        {
            if(!pm.Is(LevelType.Property, "Status")) {
                throw new IncorrectNodeException(pm);
            }

            if(pm.FinalEmptyIs(1, LevelType.Property, "HasErrors"))
            {
                string status = Value.from(Status._.get(type, index) == StatusType.Fail);
#if DEBUG
                Log.Trace("pStatus: status - '{0}'", status);
#endif
                return status;
            }

            throw new IncorrectNodeException(pm, 1);
        }

        [Property("stdout", "Get data from stdout for action which is executed asynchronously.", "item", "stEventItem", CValueType.String)]
        protected string pStdout(ISolutionEvent evt, IPM pm)
        {
            if(pm.FinalEmptyIs(LevelType.RightOperandEmpty)) {
                return Value.from(HProcess.stdout(evt.Id));
            }

            throw new IncorrectNodeException(pm);
        }

        [Property("stderr", "Get data from stderr for action which is executed asynchronously.", "item", "stEventItem", CValueType.String)]
        protected string pStderr(ISolutionEvent evt, IPM pm)
        {
            if(pm.FinalEmptyIs(LevelType.RightOperandEmpty)) {
                return Value.from(HProcess.stderr(evt.Id));
            }

            throw new IncorrectNodeException(pm);
        }

        /// <summary>
        /// item(...).Enabled
        /// item(...).Enabled = true|false
        /// </summary>
        /// <param name="evt">Selected event</param>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Property("Enabled", "Gets or Sets Enabled status for selected event-item", "item", "stEventItem", CValueType.Boolean, CValueType.Boolean)]
        protected string pEnabled(ISolutionEvent evt, IPM pm)
        {
            if(pm.FinalEmptyIs(LevelType.RightOperandEmpty)) {
                return Value.from(evt.Enabled);
            }

            evt.Enabled = Value.toBoolean(pm.Levels[0].Data);

            Log.Trace("pEnabled: updated status '{0}' for '{1}'", evt.Enabled, evt.Name);
            return Value.Empty;
        }

        protected virtual ISolutionEvent[] getEvent(SolutionEventType type)
        {
            try {
                return Settings.Cfg.getEvent(type); //TODO:
            }
            catch(NotFoundException) {
                throw new NotSupportedOperationException("The event type '{0}' is not supported yet.", type);
            }
        }

        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="index">Index of selected type of event by name</param>
        /// <returns></returns>
        private ISolutionEvent getEventByName(SolutionEventType type, string name, out int index)
        {
            if(String.IsNullOrWhiteSpace(name)) {
                throw new NotFoundException("The name of event type is null or empty.");
            }

            index = -1;
            foreach(ISolutionEvent item in getEvent(type)) {
                ++index;
                if(item.Name == name) {
                    return item;
                }
            }

            throw new NotFoundException("The event type '{0}' with name '{1}' is not exists.", type, name);
        }

        private ISolutionEvent getEventByIndex(SolutionEventType type, int index)
        {
            ISolutionEvent[] evt = getEvent(type);
            try {
                return evt[index - 1]; // starts with 1
            }
            catch(IndexOutOfRangeException) {
                throw new NotFoundException("Incorrect index '{0}' for event type - `{1}`  /{2}", index, type, evt.Length);
            }
        }
    }
}