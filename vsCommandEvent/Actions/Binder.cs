/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using net.r_eg.SobaScript;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Extensions;
using OWPIdent = net.r_eg.vsCE.Receiver.Output.Ident;
using OWPItems = net.r_eg.vsCE.Receiver.Output.Items;
using net.r_eg.vsCE.Events.CommandEvents;

#if SDK15_OR_HIGH
using Microsoft.VisualStudio.Shell;
#endif

namespace net.r_eg.vsCE.Actions
{
    /// <summary>
    /// Binder / Coordinator of main routes.
    /// </summary>
    public class Binder
    {
        internal readonly CancelBuildState buildState = new CancelBuildState();

        protected ISobaCLoader cLoader;

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

        public Binder(ICommand cmd, ISobaCLoader loader)
        {
            Cmd     = cmd ?? throw new ArgumentNullException(nameof(cmd));
            cLoader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        internal Binder(ICommand cmd, ISobaCLoader loader, CancelBuildState state)
            : this(cmd, loader)
        {
            buildState = state ?? throw new ArgumentNullException(nameof(state));
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

            try
            {
                if(Cmd.exec(evt, SolutionEventType.OWP)) {
                    Log.Debug($"[Output] {evt.Name} action completed successfully.");
                }
                return VSConstants.S_OK;
            }
            catch(Exception ex)
            {
                Log.Error($"SBE 'Output' error: {ex.Message}");
                Log.Debug(ex.StackTrace);
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
            if(SlnEvents == null) return VSConstants.S_FALSE;

            ICommandEvent[] evt = SlnEvents.Event;
            if(isDisabledAll(evt)) return VSConstants.S_OK;

            if(!IsAllowActions) return _ignoredAction(SolutionEventType.CommandEvent);

            foreach(ICommandEvent item in evt)
            {
                IFilter f = findFilter(item, pre, guid, id, customIn, customOut);
                if(f == null) continue;

                Log.Trace($"Catched: '{guid}', '{id}', '{customIn}', '{customOut}', '{cancelDefault}' /'{pre}'");

                commandEvent(item);

                if(pre && f.Pre && f.Cancel) {
                    cancelDefault = true;
                    Log.Info($"The command has been canceled by {item.Name} action.");
                }
            }
            return Status._.contains(SolutionEventType.CommandEvent, StatusType.Fail)? VSConstants.S_FALSE : VSConstants.S_OK;
        }

        protected void commandEvent(ICommandEvent item)
        {
            try
            {
                if(Cmd.exec(item, SolutionEventType.CommandEvent)) {
                    Log.Debug($"[CommandEvent] {item.Name} action completed successfully.");
                }
                Status._.add(SolutionEventType.CommandEvent, StatusType.Success);
            }
            catch(Exception ex) {
                Log.Error($"CommandEvent error: {ex.Message}");
                Log.Debug(ex.StackTrace);
            }
            Status._.add(SolutionEventType.CommandEvent, StatusType.Fail);
        }

        protected IFilter findFilter(ICommandEvent item, bool pre, string guid, int id, object customIn, object customOut)
        {
            if(item.Filters == null) return null;

            foreach(IFilter f in item.Filters)
            {
                if(!f.Pre && !f.Post) continue;
                if((pre && !f.Pre) || (!pre && !f.Post)) continue;

                if(f.Id != id || !f.Guid.CompareGuids(guid)) continue;

                if(f.IgnoreCustomIn && f.IgnoreCustomOut) return f;

                if((f.IgnoreCustomIn || f.CustomIn.EqualsMixedObjects(customIn, nullAndEmptyStr: true)) 
                    && (f.IgnoreCustomOut || f.CustomOut.EqualsMixedObjects(customOut, nullAndEmptyStr: true)))
                {
                    return f;
                }
            }
            return null;
        }

        protected string getProjectName(IVsHierarchy pHierProj)
        {
            string projectName = ((IEnvironmentExt)Cmd.Env).getProjectNameFrom(pHierProj, true);
            if(!String.IsNullOrEmpty(projectName)) {
                return projectName;
            }

#if SDK15_OR_HIGH
            ThreadHelper.ThrowIfNotOnUIThread(); //TODO: upgrade to 15
#endif

            // http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivshierarchy.getproperty.aspx
            // http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.__vshpropid.aspx
            pHierProj.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out object name);

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
