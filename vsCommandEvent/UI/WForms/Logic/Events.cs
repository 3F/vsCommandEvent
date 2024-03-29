﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using net.r_eg.vsCE.Bridge;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Configuration.User;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Extensions;

namespace net.r_eg.vsCE.UI.WForms.Logic
{
    /// <summary>
    /// TODO: !Most important! This from vsSBE 'as is', need to refactor.
    /// </summary>
    internal class Events
    {
        /// <summary>
        /// Prefix for new action by default.
        /// </summary>
        public const string ACTION_PREFIX       = "Act";

        /// <summary>
        /// Prefix for cloned action by default.
        /// </summary>
        public const string ACTION_PREFIX_CLONE = "CopyOf";

        /// <summary>
        /// Registered used SBE-events
        /// </summary>
        protected List<SBEWrap> events = new List<SBEWrap>();
        
        /// <summary>
        /// Selected item of event
        /// </summary>
        protected volatile int currentEventItem = 0;

        /// <summary>
        /// List of available types of the build
        /// </summary>
        protected List<BuildType> buildType = new List<BuildType>();

        /// <summary>
        /// Backup of settings.
        /// </summary>
        protected RestoreData backup = new RestoreData();
        
        /// <summary>
        /// object synch.
        /// </summary>
        private Object _lock = new Object();

        /// <summary>
        /// Provides operations with environment
        /// </summary>
        public IEnvironment Env
        {
            get;
            protected set;
        }

        /// <summary>
        /// Used loader
        /// </summary>
        public Bootloader Loader
        {
            get;
            protected set;
        }

        /// <summary>
        /// Manager of configurations.
        /// </summary>
        public Configuration.IManager CfgManager
        {
            get {
                return Settings.CfgManager;
            }
        }

        /// <summary>
        /// Current SBE-event
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SBEWrap SBE
        {
            get { return events[0]; }
        }

        /// <summary>
        /// Current item of SBE
        /// </summary>
        public ISolutionEvent SBEItem
        {
            get {
                if(SBE.evt.Count < 1) {
                    return null;
                }
                return SBE.evt[Math.Max(0, Math.Min(currentEventItem, SBE.evt.Count - 1))];
            }
        }

        /// <summary>
        /// Current Mode from SBE-item
        /// </summary>
        public IMode SBEItemMode
        {
            get {
                return (SBEItem.Mode == null) ? DefaultMode : SBEItem.Mode;
            }
        }

        /// <summary>
        /// Initialize the mode with end-type
        /// </summary>
        public virtual IMode DefaultMode
        {
            get { return new ModeFile(); }
        }

        /// <summary>
        /// Access to available events.
        /// </summary>
        public ISolutionEvents SlnEvents
        {
            get { return Settings.Cfg; }
        }

        /// <summary>
        /// Next unique name for action
        /// </summary>
        public string UniqueNameForAction
        {
            get {
                return genUniqueName(ACTION_PREFIX, SBE.evt);
            }
        }

        public void addEvent(SBEWrap evt)
        {
            events.Add(evt);
        }

        /// <param name="item">Selected item of event</param>
        public void setEventIndexes(int item)
        {
            currentEventItem = Math.Max(0, Math.Min(item, SBE.evt.Count - 1));
        }

        public void updateInfo(int index, string name, bool enabled, string caption)
        {
            SBE.evt[index].Name = name;
            SBE.evt[index].Enabled = enabled;
            SBE.evt[index].Caption = caption;
        }

        public void updateInfo(int index, Event evt)
        {
            SBE.evt[index] = evt;
        }

        /// <summary>
        /// Initialize the new Mode by type
        /// </summary>
        /// <param name="type">Available Modes</param>
        /// <returns>Mode with default values</returns>
        public IMode initMode(ModeType type)
        {
            switch(type) {
                case ModeType.Interpreter: {
                    return new ModeInterpreter();
                }
                case ModeType.File: {
                    return new ModeFile();
                }
                case ModeType.Script: {
                    return new ModeScript();
                }
                case ModeType.Targets: {
                    return new ModeTargets();
                }
                case ModeType.CSharp: {
                    return new ModeCSharp();
                }
                case ModeType.Operation: {
                    return new ModeOperation();
                }
                case ModeType.EnvCommand: {
                    return new ModeEnvCommand();
                }
            }
            return DefaultMode;
        }

        public virtual string formatMSBuildProperty(string name, string project = null)
        {
            if(project == null) {
                return String.Format("$({0})", name);
            }
            return String.Format("$({0}:{1})", name, project);
        }

        public string genUniqueName(string prefix, List<ISolutionEvent> scope)
        {
            int id = getUniqueId(prefix, scope);
            return String.Format("{0}{1}", prefix, (id < 1) ? "" : id.ToString());
        }

        public virtual string validateName(string name)
        {
            if(String.IsNullOrEmpty(name)) {
                return UniqueNameForAction;
            }

            name = Regex.Replace(name, 
                                    @"(?:
                                            ^([^a-z]+)
                                        |
                                            ([^a-z_0-9]+)
                                        )", 
                                    delegate(Match m) { return String.Empty; }, 
                                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            
            return String.IsNullOrEmpty(name)? UniqueNameForAction : name;
        }

        public void saveData()
        {
            CfgManager.UserConfig.save();
            CfgManager.Config.save(); // all changes has been passed by reference
            backupUpdate();
        }
        
        public void restoreData()
        {
            if(Settings.CfgUser != null) {
                Settings.CfgUser.avoidRemovingFromCache();
            }
            backupRestore();
        }
        
        /// <summary>
        /// Clone configuration from specific context into current.
        /// </summary>
        /// <param name="from">Clone from this context.</param>
        public void cloneCfg(ContextType from)
        {
            CfgManager.Config.load(CfgManager.getConfigFor(from).Data.CloneBySerializationWithType<ISolutionEvents, SolutionEvents>());
            CfgManager.UserConfig.load(CfgManager.getUserConfigFor(from).Data.CloneBySerializationWithType<IData, Data>());
        }

        /// <summary>
        /// Update of backup copies of user config for actual context.
        /// </summary>
        public void updateUserCfg()
        {
            backup.update(CfgManager.UserConfig.Data.CloneBySerializationWithType<IData, Data>(), CfgManager.Context);
        }

        public void fillEvents()
        {
            events.Clear();
            addEvent(new SBEWrap(SolutionEventType.Common));
        }

        public void fillBuildTypes(ComboBox combo)
        {
            buildType.Clear();
            combo.Items.Clear();

            buildType.Add(BuildType.Common);
            combo.Items.Add("");

            buildType.Add(BuildType.Build);
            combo.Items.Add("Build");

            buildType.Add(BuildType.Rebuild);
            combo.Items.Add("Rebuild");

            buildType.Add(BuildType.Clean);
            combo.Items.Add("Clean");

            buildType.Add(BuildType.Deploy);
            combo.Items.Add("Deploy");

            buildType.Add(BuildType.Start);
            combo.Items.Add("Start Debugging");

            buildType.Add(BuildType.StartNoDebug);
            combo.Items.Add("Start Without Debugging");

            buildType.Add(BuildType.Publish);
            combo.Items.Add("Publish");

            buildType.Add(BuildType.BuildSelection);
            combo.Items.Add("Build Selection");

            buildType.Add(BuildType.RebuildSelection);
            combo.Items.Add("Rebuild Selection");

            buildType.Add(BuildType.CleanSelection);
            combo.Items.Add("Clean Selection");

            buildType.Add(BuildType.DeploySelection);
            combo.Items.Add("Deploy Selection");

            buildType.Add(BuildType.PublishSelection);
            combo.Items.Add("Publish Selection");

            buildType.Add(BuildType.BuildOnlyProject);
            combo.Items.Add("Build Project");

            buildType.Add(BuildType.RebuildOnlyProject);
            combo.Items.Add("Rebuild Project");

            buildType.Add(BuildType.CleanOnlyProject);
            combo.Items.Add("Clean Project");

            buildType.Add(BuildType.Compile);
            combo.Items.Add("Compile");

            buildType.Add(BuildType.LinkOnly);
            combo.Items.Add("Link Only");

            buildType.Add(BuildType.BuildCtx);
            combo.Items.Add("BuildCtx");

            buildType.Add(BuildType.RebuildCtx);
            combo.Items.Add("RebuildCtx");

            buildType.Add(BuildType.CleanCtx);
            combo.Items.Add("CleanCtx");

            buildType.Add(BuildType.DeployCtx);
            combo.Items.Add("DeployCtx");

            buildType.Add(BuildType.PublishCtx);
            combo.Items.Add("PublishCtx");

            combo.SelectedIndex = 0;
        }

        public int getBuildTypeIndex(BuildType type)
        {
            return buildType.IndexOf(type);
        }

        public BuildType getBuildTypeBy(int index)
        {
            Debug.Assert(index != -1);
            return buildType[index];
        }

        /// <param name="copy">Cloning the event-item at the specified index</param>
        /// <returns>added item</returns>
        public ISolutionEvent addEventItem(int copy = -1)
        {
            ISolutionEvent added;
            bool isNew = (copy >= SBE.evt.Count || copy < 0);

            switch(SBE.type)
            {
                case SolutionEventType.Common:
                case SolutionEventType.CommandEvent:
                case SolutionEventType.OWP: {
                    var evt = (isNew)? new Event() : SBE.evt[copy].CloneBySerializationWithType<ISolutionEvent, Event>();
                    SlnEvents.Event = SlnEvents.Event.GetWithAdded(evt);
                    added = evt;
                    break;
                }
                default: {
                    throw new ArgumentException($"Unsupported SolutionEventType: '{SBE.type}'");
                }
            }
            SBE.update();
            
            // fix new data

            if(isNew) {
                added.Name = UniqueNameForAction;
                return added;
            }

            added.Caption   = String.Format("Copy of '{0}' - {1}", added.Name, added.Caption);
            added.Name      = genUniqueName(ACTION_PREFIX_CLONE + added.Name, SBE.evt);
            cacheUnlink(added.Mode);
            
            return added;
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void moveEventItem(int from, int to)
        {
            if(from == to) {
                return;
            }

            switch(SBE.type) {
                case SolutionEventType.Common:
                case SolutionEventType.CommandEvent:
                case SolutionEventType.OWP: {
                    SlnEvents.Event = SlnEvents.Event.GetWithMoved(from, to);
                    break;
                }
            }
            SBE.update();
            setEventIndexes(to);
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void removeEventItem(int index)
        {
            cacheToRemove(SBE.evt[index].Mode);

            switch(SBE.type) {
                case SolutionEventType.Common:
                case SolutionEventType.CommandEvent:
                case SolutionEventType.OWP: {
                    SlnEvents.Event = SlnEvents.Event.GetWithRemoved(index);
                    break;
                }
            }
            SBE.update();
        }

        /// <summary>
        /// Prepare data to removing from cache.
        /// </summary>
        /// <param name="mode">Data from used mode.</param>
        public void cacheToRemove(IMode mode)
        {
            if(mode.Type == ModeType.CSharp)
            {
                IModeCSharp cfg = (IModeCSharp)mode;
                if(cfg.CacheData == null) {
                    return;
                }

                Settings.CfgUser.toRemoveFromCache(cfg.CacheData);
                cacheUnlink(mode);
            }
        }

        /// <summary>
        /// Unlink data from cache container.
        /// </summary>
        /// <param name="mode">Data from used mode.</param>
        public void cacheUnlink(IMode mode)
        {
            if(mode.Type == ModeType.CSharp) {
                ((IModeCSharp)mode).CacheData = null;
            }
        }

        /// <summary>
        /// To reset cache data.
        /// </summary>
        /// <param name="mode"></param>
        public void cacheReset(IMode mode)
        {
            if(mode.Type == ModeType.CSharp)
            {
                IModeCSharp cfg = (IModeCSharp)mode;
                if(cfg.CacheData != null) {
                    cfg.CacheData.Manager.reset();
                }
            }
        }

        /// <summary>
        /// Gets current ICommon configuration.
        /// </summary>
        /// <returns></returns>
        public ICommon getCommonCfg(ModeType type)
        {
            var data    = Settings.CfgUser.Common;
            Route route = new Route() { Event = SBE.type, Mode = type };

            if(!data.ContainsKey(route)) {
                data[route] = new Common();
            }
            return data[route];
        }

        /// <summary>
        /// Execution by user.
        /// </summary>
        public void execAction()
        {
            if(SBEItem == null) {
                Log.Info("No actions to execution. Add new, then try again.");
                return;
            }
            Actions.ICommand cmd = new Actions.Command
            (
                Loader.Env,
                Loader.Soba,
                Loader.Soba.EvMSBuild
            );

            ISolutionEvent evt      = SBEItem;
            SolutionEventType type  = SBE.type;
            Log.Info($"Execute an action '{evt.Name}' manually. Emulate '{type}' event. {evt.Caption}");

            cmd.Env.BuildType = BuildType.Common; //TODO: IBuild.updateBuildType
            try {
                bool res = cmd.exec(evt, type);
                Log.Info($"Completed an action '{evt.Name}': {res}");
            }
            catch(Exception ex) {
                Log.Error($"Failed an action '{evt.Name}': {ex.Message}");
                Log.Debug(ex.StackTrace);
            }
        }

        public Events(Bootloader loader)
        {
            Loader          = loader;
            Env             = loader.Env;
            backupUpdate();
        }

        /// <summary>
        /// Updating of deep copies from configuration data using all available contexts.
        /// </summary>
        protected void backupUpdate()
        {
            backupUpdate(ContextType.Common);
            if(CfgManager.IsExistCfg(ContextType.Solution)) {
                backupUpdate(ContextType.Solution);
            }
        }

        /// <summary>
        /// Updating of deep copies from configuration data using specific context.
        /// </summary>
        protected void backupUpdate(ContextType context)
        {
            backup.update(CfgManager.getConfigFor(context).Data.CloneBySerializationWithType<ISolutionEvents, SolutionEvents>(), context);
            backup.update(CfgManager.getUserConfigFor(context).Data.CloneBySerializationWithType<IData, Data>(), context);
        }

        /// <summary>
        /// Restore configuration data from backup using all available contexts.
        /// </summary>
        protected void backupRestore()
        {
            if(CfgManager.IsExistCfg(ContextType.Solution)) {
                backupRestore(ContextType.Solution);
            }
            backupRestore(ContextType.Common);
        }

        /// <summary>
        /// Restore configuration data from backup using specific context.
        /// </summary>
        protected void backupRestore(ContextType context)
        {
            CfgManager.getConfigFor(context).load(backup.getConfig(context).CloneBySerializationWithType<ISolutionEvents, SolutionEvents>());
            CfgManager.getUserConfigFor(context).load(backup.getUserConfig(context).CloneBySerializationWithType<IData, Data>());
        }

        /// <summary>
        /// Generating id for present scope
        /// </summary>
        /// <param name="prefix">only for specific prefix</param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected virtual int getUniqueId(string prefix, List<ISolutionEvent> scope)
        {
            int maxId = 0;
            foreach(ISolutionEvent item in scope)
            {
                if(String.IsNullOrEmpty(item.Name)) {
                    continue;
                }

                try
                {
                    Match m = Regex.Match(item.Name, String.Format(@"^{0}(\d*)$", prefix), RegexOptions.IgnoreCase);
                    if(!m.Success) {
                        continue;
                    }
                    string num = m.Groups[1].Value;

                    maxId = Math.Max(maxId, (num.Length > 0)? Int32.Parse(num) + 1 : 1);
                }
                catch(Exception ex) {
                    Log.Debug("getUniqueId: {0} ::'{1}'", ex.ToString(), prefix);
                }
            }
            return maxId;
        }
    }
}