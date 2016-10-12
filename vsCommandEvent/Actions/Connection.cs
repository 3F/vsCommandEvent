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
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Extensions;
using OWPIdent = net.r_eg.vsCE.Receiver.Output.Ident;
using OWPItems = net.r_eg.vsCE.Receiver.Output.Items;

namespace net.r_eg.vsCE.Actions
{
    /// <summary>
    /// Binder / Coordinator of main routes.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// object synch.
        /// </summary>
        private Object _lock = new Object();

        /// <summary>
        /// The main handler of commands.
        /// </summary>
        public ICommand Cmd
        {
            get;
            protected set;
        }

        /// <summary>
        /// Flag of permission for any actions.
        /// </summary>
        protected bool IsAllowActions
        {
            get { return !Settings._.IgnoreActions; }
        }

        /// <summary>
        /// Access to available events.
        /// </summary>
        protected ISolutionEvents SlnEvents
        {
            get { return Settings.Cfg; }
        }

        /// <summary>
        /// Full process of building.
        /// </summary>
        /// <param name="data">Raw data</param>
        /// <param name="guid">Guid string of pane</param>
        /// <param name="item">Name of item pane</param>
        public void bindBuildRaw(string data, string guid, string item)
        {
            OWPItems._.getEW(new OWPIdent() { guid = guid, item = item }).updateRaw(data); //TODO:
            if(!IsAllowActions)
            {
                if(!isDisabledAll(SlnEvents.Event)) {
                    _ignoredAction(SolutionEventType.OWP);
                }
                return;
            }

            foreach(ISolutionEventOWP evt in SlnEvents.Event) {
                if(evt.Enabled) {
                    sbeOutput(evt, ref data, guid, item);
                }
            }
        }

        /// <summary>
        /// Binding of the execution Command ID for EnvDTE /Before.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        /// <param name="cancelDefault">Whether the command has been cancelled.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        public int bindCommandDtePre(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            return commandEvent(true, guid, id, customIn, customOut, ref cancelDefault);
        }

        /// <summary>
        /// Binding of the execution Command ID for EnvDTE /After.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        public int bindCommandDtePost(string guid, int id, object customIn, object customOut)
        {
            bool cancelDefault = false;
            return commandEvent(false, guid, id, customIn, customOut, ref cancelDefault);
        }

        /// <summary>
        /// Resetting all progress of handling events
        /// </summary>
        /// <returns>true value if successful resetted</returns>
        public bool reset()
        {
            if(!IsAllowActions) {
                return false;
            }
            Status._.flush();
            return true;
        }

        public Connection(ICommand cmd)
        {
            this.Cmd = cmd;
        }


        /// <summary>
        /// Entry point to the OWP
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="raw"></param>
        /// <param name="guid">Guid string of pane</param>
        /// <param name="item">Name of item pane</param>
        protected int sbeOutput(ISolutionEventOWP evt, ref string raw, string guid, string item)
        {
            if(!(new Receiver.Output.Matcher()).match(evt.Match, raw, guid, item)) {
                return VSConstants.S_OK;
            }

            try {
                if(Cmd.exec(evt, SolutionEventType.OWP)) {
                    Log.Info("[Output] finished SBE: {0}", evt.Caption);
                }
                return VSConstants.S_OK;
            }
            catch(Exception ex) {
                Log.Error("SBE 'Output' error: {0}", ex.Message);
            }
            return VSConstants.S_FALSE;
        }

        /// <param name="pre">Flag of Before/After execution.</param>
        /// <param name="guid">The GUID.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        /// <param name="cancelDefault">Whether the command has been cancelled.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        protected int commandEvent(bool pre, string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            if(SlnEvents == null) { // activation of this event type can be before opening solution
                return VSConstants.S_FALSE;
            }
            ICommandEvent[] evt = SlnEvents.Event;

            if(isDisabledAll(evt)) {
                return VSConstants.S_OK;
            }

            if(!IsAllowActions) {
                return _ignoredAction(SolutionEventType.CommandEvent);
            }

            foreach(ICommandEvent item in evt)
            {
                if(item.Filters == null) {
                    // well, should be some protection for user if we will listen all events... otherwise we can lose control
                    continue;
                }

                var Is = item.Filters.Where(f => 
                            (
                              ((pre && f.Pre) || (!pre && f.Post))
                                && 
                                (
                                  (f.Id == id && f.Guid != null && f.Guid.CompareGuids(guid))
                                   && 
                                   (
                                     (
                                       (f.CustomIn != null && f.CustomIn.EqualsMixedObjects(customIn))
                                       || (f.CustomIn == null && customIn.IsNullOrEmptyString())
                                     )
                                     &&
                                     (
                                       (f.CustomOut != null && f.CustomOut.EqualsMixedObjects(customOut))
                                       || (f.CustomOut == null && customOut.IsNullOrEmptyString())
                                     )
                                   )
                                )
                            )).Select(f => f.Cancel);

                if(Is.Count() < 1) {
                    continue;
                }

                Log.Trace("[CommandEvent] catched: '{0}', '{1}', '{2}', '{3}', '{4}' /'{5}'",
                                                        guid, id, customIn, customOut, cancelDefault, pre);

                commandEvent(item);

                if(pre && Is.Any(f => f)) {
                    cancelDefault = true;
                    Log.Info("[CommandEvent] original command has been canceled for action: '{0}'", item.Caption);
                }
            }
            return Status._.contains(SolutionEventType.CommandEvent, StatusType.Fail)? VSConstants.S_FALSE : VSConstants.S_OK;
        }

        protected void commandEvent(ICommandEvent item)
        {
            try
            {
                if(Cmd.exec(item, SolutionEventType.CommandEvent)) {
                    Log.Info("[CommandEvent] finished: '{0}'", item.Caption);
                }
                Status._.add(SolutionEventType.CommandEvent, StatusType.Success);
            }
            catch(Exception ex) {
                Log.Error("CommandEvent error: '{0}'", ex.Message);
            }
            Status._.add(SolutionEventType.CommandEvent, StatusType.Fail);
        }

        protected string getProjectName(IVsHierarchy pHierProj)
        {
            string projectName = ((IEnvironmentExt)Cmd.Env).getProjectNameFrom(pHierProj, true);
            if(!String.IsNullOrEmpty(projectName)) {
                return projectName;
            }

            object name;
            // http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivshierarchy.getproperty.aspx
            // http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.__vshpropid.aspx
            pHierProj.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out name);

            return name.ToString();
        }

        /// <param name="evt">Array of handling events</param>
        /// <returns>true value if all event are disabled for present array</returns>
        protected bool isDisabledAll(ISolutionEvent[] evt)
        {
            foreach(ISolutionEvent item in evt) {
                if(item.Enabled) {
                    return false;
                }
            }
            return true;
        }

        private int _ignoredAction(SolutionEventType type)
        {
            Log.Trace("[{0}] Ignored action. It's already started in other processes of VS.", type);
            return VSConstants.S_OK;
        }
    }
}
