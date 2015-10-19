/*
 * Copyright (c) 2013-2015  Denis Kuzmin (reg) <entry.reg@gmail.com>
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
using EnvDTE80;
using Microsoft.VisualStudio;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.SBEScripts;
using net.r_eg.vsCE.Scripts;

namespace net.r_eg.vsCE
{
    public class Binder: IBinder, Bridge.IEvent
    {
        /// <summary>
        /// When the solution has been opened
        /// </summary>
        public event EventHandler OpenedSolution = delegate(object sender, EventArgs e) { };

        /// <summary>
        /// When the solution has been closed
        /// </summary>
        public event EventHandler ClosedSolution = delegate(object sender, EventArgs e) { };

        /// <summary>
        /// Container of user-variables
        /// </summary>
        protected IUserVariable uvariable = new UserVariable();

        /// <summary>
        /// Provides command events for automation clients
        /// </summary>
        protected EnvDTE.CommandEvents cmdEvents;

        /// <summary>
        /// object synch.
        /// </summary>
        private Object _lock = new Object();

        /// <summary>
        /// Main loader
        /// </summary>
        public IBootloader Bootloader
        {
            get;
            protected set;
        }

        /// <summary>
        /// Binder of action
        /// </summary>
        public Actions.Connection Action
        {
            get;
            protected set;
        }

        /// <summary>
        /// Used Environment
        /// </summary>
        public IEnvironment Environment
        {
            get;
            protected set;
        }

        /// <summary>
        /// Manager of configurations.
        /// </summary>
        public IManager ConfigManager
        {
            get {
                return Settings.CfgManager;
            }
        }

        /// <summary>
        /// Solution has been opened.
        /// </summary>
        /// <param name="pUnkReserved">Reserved for future use.</param>
        /// <param name="fNewSolution">true if the solution is being created. false if the solution was created previously or is being loaded.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        public int solutionOpened(object pUnkReserved, int fNewSolution)
        {
            Config config           = new Config();
            UserConfig userConfig   = new UserConfig();

            bool isNew = !config.load(Environment.SolutionPath, Environment.SolutionFileName);
            Log.Trace("Config Link: '{0}'", config.Link);

            userConfig.load(config.Link);
            Log.Trace("UserConfig Link: '{0}'", userConfig.Link);

            ConfigManager.add(config, ContextType.Solution);
            ConfigManager.add(userConfig, ContextType.Solution);

            ConfigManager.switchOn((isNew || userConfig.Data.Global.IgnoreConfiguration)? ContextType.Common : ContextType.Solution);

            OpenedSolution(this, new EventArgs());
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Solution has been closed.
        /// </summary>
        /// <param name="pUnkReserved">Reserved for future use.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        public int solutionClosed(object pUnkReserved)
        {
            ConfigManager.unsetAndUse(ContextType.Solution, ContextType.Common);
            ClosedSolution(this, new EventArgs());
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Before executing Command ID for EnvDTE.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        /// <param name="cancelDefault">Whether the command has been cancelled.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        public int onCommandDtePre(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            try {
                return Action.bindCommandDtePre(guid, id, customIn, customOut, ref cancelDefault);
            }
            catch(Exception ex) {
                Log.Error("Failed EnvDTE.Command-binding/Before: '{0}'", ex.Message);
            }
            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// After executed Command ID for EnvDTE.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        /// <returns>If the method succeeds, it returns VSConstants.S_OK. If it fails, it returns an error code.</returns>
        public int onCommandDtePost(string guid, int id, object customIn, object customOut)
        {
            try {
                return Action.bindCommandDtePost(guid, id, customIn, customOut);
            }
            catch(Exception ex) {
                Log.Error("Failed EnvDTE.Command-binding/After: '{0}'", ex.Message);
            }
            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// During assembly.
        /// </summary>
        /// <param name="data">Raw data of building process</param>
        /// <param name="guid">Guid string of pane</param>
        /// <param name="item">Name of item pane</param>
        public void onBuildRaw(string data, string guid, string item)
        {
            try {
                Action.bindBuildRaw(data, guid, item);
            }
            catch(Exception ex) {
                Log.Error("Failed build-raw: '{0}'", ex.Message);
            }
        }

        /// <summary>
        /// Sets current type of the build
        /// </summary>
        /// <param name="type"></param>
        public void updateBuildType(Bridge.BuildType type)
        {
            Environment.BuildType = type;
        }

        /// <param name="dte2"></param>
        public Binder(DTE2 dte2)
        {
            this.Environment = new Environment(dte2);
            init();
        }

        /// <summary>
        /// Initialize core.
        /// </summary>
        protected void init()
        {

#if DEBUG
            Log.Warn("Used [Debug version]");
#else
            if(vsCE.Version.branchName.ToLower() != "releases") {
                Log.Warn("Used [Unofficial release]");
            }
#endif

            attachCommandEvents();
            this.Bootloader = new Bootloader(Environment, uvariable);
            this.Bootloader.register();
            this.Bootloader.updateActivation(Settings.Cfg);

            Action = new Actions.Connection(
                            new Actions.Command(Environment,
                                         new Script(Bootloader),
                                         new MSBuild.Parser(Environment, uvariable))
            );
        }

        protected void attachCommandEvents()
        {
            if(Environment.Events == null) {
                Log.Info("Context of build action: uses a limited types.");
                return; //this can be for emulated DTE2 context
            }

            cmdEvents = Environment.Events.CommandEvents; // protection from garbage collector
            lock(_lock) {
                cmdEvents.BeforeExecute -= onCmdBeforeExecute;
                cmdEvents.BeforeExecute += onCmdBeforeExecute;
                cmdEvents.AfterExecute  -= onCmdAfterExecute;
                cmdEvents.AfterExecute  += onCmdAfterExecute;
            }
        }

        protected void detachCommandEvents()
        {
            if(cmdEvents == null) {
                return;
            }
            lock(_lock) {
                cmdEvents.BeforeExecute -= onCmdBeforeExecute;
                cmdEvents.AfterExecute  -= onCmdAfterExecute;
            }
        }

        /// <summary>
        /// Provides the BuildAction
        /// Note: VSSOLNBUILDUPDATEFLAGS with IVsUpdateSolutionEvents4 exist only for VS2012 and higher
        /// http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsupdatesolutionevents4.updatesolution_beginupdateaction.aspx
        /// See for details: http://stackoverflow.com/q/27018762
        /// </summary>
        private void onCmdBeforeExecute(string guidString, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            onCommandDtePre(guidString, id, customIn, customOut, ref cancelDefault);

            Guid guid = new Guid(guidString);
            if(GuidList.VSStd97CmdID != guid && GuidList.VSStd2KCmdID != guid) {
                return;
            }

            if(UnifiedTypes.Build.VSCommand.existsById(id)) {
                updateBuildType(UnifiedTypes.Build.VSCommand.getByCommandId(id));
            }
        }

        private void onCmdAfterExecute(string guid, int id, object customIn, object customOut)
        {
            onCommandDtePost(guid, id, customIn, customOut);
        }
    }
}
