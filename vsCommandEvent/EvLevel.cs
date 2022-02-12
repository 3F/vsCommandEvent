/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using EnvDTE80;
using Microsoft.VisualStudio;
using net.r_eg.vsCE.Configuration;
using net.r_eg.EvMSBuild;
using net.r_eg.SobaScript.Components;
using System.Linq;
using AppSettings = net.r_eg.vsCE.Settings;

#if SDK15_OR_HIGH
using Microsoft.VisualStudio.Shell;
#endif

namespace net.r_eg.vsCE
{
    public class EvLevel: IEvLevel, Bridge.IEvent
    {
        internal readonly CancelBuildState buildState = new CancelBuildState();

        private Bootloader loader;

        private readonly object sync = new();

        public event EventHandler OpenedSolution = delegate (object sender, EventArgs e) { };

        public event EventHandler ClosedSolution = delegate (object sender, EventArgs e) { };

        public Actions.Binder Action { get; protected set; }

        public IEnvironment Environment { get; protected set; }

        public IManager ConfigManager => Settings.CfgManager;

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

            refreshComponents();
            initPropByDefault(Action.Cmd.MSBuild); //LC: #815, #814
            buildState.Reset();

            OpenedSolution(this, EventArgs.Empty);
            return VSConstants.S_OK;
        }

        public int solutionClosed(object pUnkReserved)
        {
            ConfigManager.unsetAndUse(ContextType.Solution, ContextType.Common);
            ClosedSolution(this, EventArgs.Empty);
            return VSConstants.S_OK;
        }

        public int onCommandDtePre(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            try {
                return Action.bindCommandDtePre(guid, id, customIn, customOut, ref cancelDefault);
            }
            catch(Exception ex) {
                Log.Error($"Failed EnvDTE.Command-binding/Before: {ex.Message}");
                Log.Debug(ex.StackTrace);
            }
            return VSConstants.S_FALSE;
        }

        public int onCommandDtePost(string guid, int id, object customIn, object customOut)
        {
            try {
                return Action.bindCommandDtePost(guid, id, customIn, customOut);
            }
            catch(Exception ex) {
                Log.Error($"Failed EnvDTE.Command-binding/After: {ex.Message}");
                Log.Debug(ex.StackTrace);
            }
            return VSConstants.S_FALSE;
        }

        public void onBuildRaw(string data, string guid, string item)
        {
            try {
                Action.bindBuildRaw(data, guid, item);
            }
            catch(Exception ex) {
                Log.Error($"Failed build-raw: {ex.Message}");
                Log.Debug(ex.StackTrace);
            }
        }

        public void updateBuildType(Bridge.BuildType type)
        {
            if(Environment != null) {
                Environment.BuildType = type;
            }
        }

        public EvLevel(DTE2 dte2)
        {
            Environment = new Environment(dte2);
            init();
        }

        /// <summary>
        /// Initialize core.
        /// </summary>
        protected void init()
        {
#if DEBUG
            Log.Warn("Debug version");
#endif

            loader = Bootloader.Init(this);

            lock(sync)
            {
                Environment.AggregatedEvents.BeforeExecute -= onCmdBeforeExecute;
                Environment.AggregatedEvents.AfterExecute -= onCmdAfterExecute;
                Environment.AggregatedEvents.BeforeExecute += onCmdBeforeExecute;
                Environment.AggregatedEvents.AfterExecute += onCmdAfterExecute;
            }

            Action = new Actions.Binder
            (
                new Actions.Command
                (
                    Environment,
                    loader.Soba,
                    loader.Soba.EvMSBuild
                ),
                loader.Soba,
                buildState
            );

            initPropByDefault(Action.Cmd.MSBuild);
        }

        protected void refreshComponents()
        {
            if(loader == null || Settings.CfgManager.Config == null) {
                Log.Debug("Changing of activation has been ignored.");
                return;
            }

            var data = AppSettings.CfgManager.Config.Data;

            foreach(IComponent c in loader.Soba.Registered)
            {
                var found = data.Components?.FirstOrDefault(p => p.ClassName == c.GetType().Name);
                if(found == null)
                {
                    // Each component provides its default state for IComponent.Enabled
                    // We'll just continue 'as is' if this component is not presented in config.
                    continue;
                }

#if DEBUG
                if(c.Enabled != found.Enabled) {
                    Log.Trace($"Bootloader - Component '{found.ClassName}': Changing of activation status '{c.Enabled}' -> '{found.Enabled}'");
                }
#endif
                c.Enabled = found.Enabled;
            }
        }

        /// <summary>
        /// To initialize properties by default for project.
        /// </summary>
        protected void initPropByDefault(IEvMSBuild msbuild)
        {
            IAppSettings app = AppSettings._;
            const string _PFX = AppSettings.APP_NAME_SHORT;

            msbuild.SetGlobalProperty(AppSettings.APP_NAME, vsCE.Version.S_NUM_REV);
            msbuild.SetGlobalProperty($"{_PFX}_CommonPath", app.CommonPath);
            msbuild.SetGlobalProperty($"{_PFX}_LibPath", app.LibPath);
            msbuild.SetGlobalProperty($"{_PFX}_WorkPath", app.WorkPath);
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
