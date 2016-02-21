/*
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
using net.r_eg.vsCE.Events.CommandEvents;
using net.r_eg.vsCE.Exceptions;
using net.r_eg.vsCE.SBEScripts.Dom;
using net.r_eg.vsCE.SBEScripts.Exceptions;
using net.r_eg.vsCE.SBEScripts.SNode;

namespace net.r_eg.vsCE.SBEScripts.Components
{
    [Component("DTE", "For work with EnvDTE.\nAssembly-wrapped COM library containing the objects and members for Visual Studio core automation.\n- http://msdn.microsoft.com/en-us/library/EnvDTE.aspx")]
    public class DTEComponent: Component, IComponent
    {
        /// <summary>
        /// Provides command-events for automation clients.
        /// </summary>
        protected EnvDTE.CommandEvents cmdEvents;

        /// <summary>
        /// The last received command from EnvDTE.
        /// </summary>
        protected volatile IFilter lastCommandEvent = new Filter();

        /// <summary>
        /// Work with commands.
        /// </summary>
        protected DTEOperation dteo;

        /// <summary>
        /// object synch.
        /// </summary>
        private Object _lock = new Object();

        /// <summary>
        /// Ability to work with data for current component
        /// </summary>
        public override string Condition
        {
            get { return "DTE "; }
        }

        /// <summary>
        /// Ability of work with CommandEvent.
        /// </summary>
        protected bool IsAvaialbleCommandEvent
        {
            get { return env != null && env.Events != null; }
        }

        /// <param name="env">Used environment</param>
        public DTEComponent(IEnvironment env)
            : base(env)
        {
            dteo = new DTEOperation(env, Events.SolutionEventType.General);
            attachCommandEvents();
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

            switch(subtype)
            {
                case "exec": {
                    return stExec(new PM(request));
                }
                case "raise": {
                    return stRaise(new PM(request));
                }
                case "events": {
                    return stEvents(new PM(request));
                }
            }

            throw new SubtypeNotFoundException("Subtype `{0}` is not found", subtype);
        }

        /// <summary>
        /// DTE-command to execution
        /// e.g: #[DTE exec: command(arg)]
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Property("exec", "DTE-command to execution, e.g.: exec: command(arg)", CValueType.Void, CValueType.Input)]
        protected string stExec(IPM pm)
        {
            if(!pm.It(LevelType.Property, "exec") || !pm.IsRight(LevelType.RightOperandColon)) {
                throw new IncorrectNodeException(pm);
            }

            string cmd = pm.Levels[0].Data.Trim();
            if(String.IsNullOrWhiteSpace(cmd)) {
                throw new InvalidArgumentException("The command cannot be empty.");
            }
            Log.Debug("Execute command `{0}`", cmd);

            dteo.exec(new string[] { cmd }, false);
            return Value.Empty;
        }

        /// <summary>
        /// Raise Command ID for EnvDTE.
        /// e.g: #[DTE raise(guid, id, customIn, customOut)]
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Method(
                "raise",
                "Raise Command ID for EnvDTE.",
                new string[] { "guid", "id", "customIn", "customOut" },
                new string[] { "Scope by Guid", "The command ID", "Mixed input parameters inc. complex object as: {}, {\"str\", true}, etc.", "Mixed output parameters inc. complex object as: {}, {\"str\", true}, etc." },
                CValueType.Void,
                CValueType.String, CValueType.Integer, CValueType.Mixed, CValueType.Mixed
        )]
        protected string stRaise(IPM pm)
        {
            if(!pm.FinalEmptyIs(0, LevelType.Method, "raise")) {
                throw new OperationNotFoundException("stRaise: not found - '{0}' /'{1}'", pm.Levels[1].Data, pm.Levels[1].Type);
            }

            Argument[] args = pm.Levels[0].Args;

            if(args.Length != 4 
                || args[0].type != ArgumentType.StringDouble
                || args[1].type != ArgumentType.Integer)
            {
                throw new InvalidArgumentException("stRaise: incorrect arguments to `raise(string guid, integer id, mixed customIn, mixed customOut)`");
            }

            string guid = (string)args[0].data;
            int id      = (int)args[1].data;
            
            object customIn;
            if(args[2].type == ArgumentType.Object) {
                customIn = (object)Value.extract((Argument[])args[2].data);
            }
            else {
                customIn = args[2].data;
            }

            object customOut;
            if(args[3].type == ArgumentType.Object) {
                customOut = (object)Value.extract((Argument[])args[3].data);
            }
            else {
                customOut = args[3].data;
            }

            Log.Trace("stRaise: guid - '{0}', id - '{1}', In - '{2}', Out - '{3}' ", guid, id, customIn, customOut);
            raise(guid, id, ref customIn, ref customOut);
            return Value.Empty;
        }

        /// <summary>
        /// For work with available events.
        /// #[DTE events]
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Property("events", "Operations with events.", CValueType.Void, CValueType.Void)]
        protected string stEvents(IPM pm)
        {
            if(!pm.It(LevelType.Property, "events")) {
                throw new IncorrectNodeException(pm);
            }

            if(pm.It(LevelType.Property, "LastCommand")) {
                return stLastCommand(pm);
            }

            throw new IncorrectNodeException(pm);
        }

        /// <summary>
        /// Last received command from EnvDTE
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        [Property("LastCommand", "The last received command.", "events", "stEvents")]
        [Property("Guid", "Scope of Command ID", "LastCommand", "stLastCommand", CValueType.String)]
        [Property("Id", "Command ID", "LastCommand", "stLastCommand", CValueType.Integer)]
        [Property("CustomIn", "Custom input parameters.", "LastCommand", "stLastCommand", CValueType.Object)]
        [Property("CustomOut", "Custom output parameters.", "LastCommand", "stLastCommand", CValueType.Object)]
        [Property("Pre", "Flag of execution of the command - Before / After", "LastCommand", "stLastCommand", CValueType.Boolean)]
        protected string stLastCommand(IPM pm)
        {
            if(!IsAvaialbleCommandEvent) {
                throw new NotSupportedOperationException("CommandEvents: aren't available for current context. Use full environment.");
            }

            if(pm.FinalEmptyIs(LevelType.Property, "Guid")) {
                return (lastCommandEvent.Guid) ?? Value.Empty;
            }

            if(pm.FinalEmptyIs(LevelType.Property, "Id")) {
                return Value.from(lastCommandEvent.Id);
            }

            if(pm.FinalEmptyIs(LevelType.Property, "CustomIn")) {
                return Value.pack(lastCommandEvent.CustomIn) ?? Value.Empty;
            }

            if(pm.FinalEmptyIs(LevelType.Property, "CustomOut")) {
                return Value.pack(lastCommandEvent.CustomOut) ?? Value.Empty;
            }

            if(pm.FinalEmptyIs(LevelType.Property, "Pre")) {
                return Value.from(lastCommandEvent.Pre); // see commandEvent below
            }

            throw new IncorrectNodeException(pm);
        }

        /// <summary>
        /// Raise Command ID for EnvDTE.
        /// </summary>
        /// <param name="guid">Scope by Guid.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        protected virtual void raise(string guid, int id, ref object customIn, ref object customOut)
        {
            env.raise(guid, id, ref customIn, ref customOut);
        }

        protected void attachCommandEvents()
        {
            if(!IsAvaialbleCommandEvent || env.Events.CommandEvents == null) {
                Log.Info("CommandEvents: aren't available for current context.");
                return; //this can be for emulated DTE2 context
            }

            cmdEvents = env.Events.CommandEvents;
            lock(_lock) {
                detachCommandEvents();
                cmdEvents.BeforeExecute += commandEventBefore;
                cmdEvents.AfterExecute  += commandEventAfter;
            }
        }

        protected void detachCommandEvents()
        {
            if(cmdEvents == null) {
                return;
            }

            lock(_lock) {
                cmdEvents.BeforeExecute -= commandEventBefore;
                cmdEvents.AfterExecute  -= commandEventAfter;
            }
        }

        protected void commandEventBefore(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            commandEvent(true, guid, id, customIn, customOut);
        }

        protected void commandEventAfter(string guid, int id, object customIn, object customOut)
        {
            commandEvent(false, guid, id, customIn, customOut);
        }

        private void commandEvent(bool pre, string guid, int id, object customIn, object customOut)
        {
            lastCommandEvent = new Filter() {
                Guid        = guid,
                Id          = id,
                CustomIn    = customIn,
                CustomOut   = customOut,
                Pre         = pre // only as flag (Before / After) for DTEComponent
            };
        }
    }
}
