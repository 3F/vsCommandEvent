﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.IO;
using System.Reflection;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.vsCE.Configuration;
using IUserData = net.r_eg.vsCE.Configuration.User.IData;

namespace net.r_eg.vsCE
{
    internal sealed class Settings: IAppSettings
    {
        /// <summary>
        /// Name of item in VS OutputWindow
        /// </summary>
        public const string OWP_ITEM_VSSBE = "vsCommandEvent";

        /// <summary>
        /// Name of application.
        /// </summary>
        public const string APP_NAME = "vsCommandEvent";

        /// <summary>
        /// Short name of application.
        /// </summary>
        public const string APP_NAME_SHORT = "vsCE";
        
        /// <summary>
        /// When DebugMode is updated.
        /// Useful for clients, for example with IEntryPointClient.
        /// </summary>
        public event EventHandler<DataArgs<bool>> DebugModeUpdated = delegate(object sender, DataArgs<bool> e) { };

        /// <summary>
        /// When IAppSettings.WorkPath was updated.
        /// </summary>
        public event EventHandler<DataArgs<string>> WorkPathUpdated = delegate (object sender, DataArgs<string> e) { };

        /// <summary>
        /// Debug mode for application.
        /// </summary>
        public bool DebugMode
        {
            get {
                return debugMode;
            }
            set {
                debugMode = value;
                DebugModeUpdated(this, new DataArgs<bool>() { Data = debugMode });
            }
        }
        private bool debugMode = false;

        /// <summary>
        /// Ignores all actions if value set as true.
        /// To support of cycle control, e.g.: PRE -> POST [recursive DTE: PRE -> POST] -> etc.
        /// </summary>
        public bool IgnoreActions
        {
            get { return ignoreActions; }
            set { ignoreActions = value; }
        }
        private volatile bool ignoreActions = false;

        /// <summary>
        /// Checks availability data for used configurations.
        /// </summary>
        public bool IsCfgExists
        {
            get {
                return (Config != null && UserConfig != null);
            }
        }

        /// <summary>
        /// Common path of library.
        /// </summary>
        public string CommonPath
        {
            get
            {
                if(commonPath != null) {
                    return commonPath;
                }

                string vsdir    = System.Environment.GetEnvironmentVariable("VisualStudioDir");
                string path     = Path.Combine((vsdir)?? Path.GetTempPath(), APP_NAME).DirectoryPathFormat();

                if(!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                commonPath = path;
                return commonPath;
            }
        }
        private string commonPath;

        /// <summary>
        /// Full path to library.
        /// </summary>
        public string LibPath
        {
            get
            {
                if(String.IsNullOrWhiteSpace(libPath)) {
                    libPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).DirectoryPathFormat();
                }
                return libPath;
            }
        }
        private string libPath;

        /// <summary>
        /// Working path for library.
        /// </summary>
        public string WorkPath
        {
            get
            {
                if(String.IsNullOrWhiteSpace(workPath)) {
                    workPath = "".DirectoryPathFormat();
                    Log.Trace("WorkPath is empty or null, use `{0}` by default.", workPath);
                    //throw new SBEException("WorkPath is empty or null");
                }
                return workPath;
            }
        }
        private string workPath;

        /// <summary>
        /// OWP item name by default.
        /// </summary>
        public string DefaultOWPItem => "Build";

        /// <summary>
        /// Manager of configurations.
        /// </summary>
        public IManager ConfigManager
        {
            get
            {
                if(configManager == null) {
                    configManager = new Manager(this);
                }
                return configManager;
            }
        }
        private IManager configManager;

        /// <summary>
        /// Main configuration data.
        /// </summary>
        public ISolutionEvents Config
        {
            get
            {
                if(ConfigManager.Config == null) {
                    return null;
                }
                return ConfigManager.Config.Data;
            }
        }

        /// <summary>
        /// User configuration data.
        /// </summary>
        public IUserData UserConfig
        {
            get
            {
                if(ConfigManager.UserConfig == null) {
                    return null;
                }
                return ConfigManager.UserConfig.Data;
            }
        }

        ///// <summary>
        ///// Global configuration data.
        ///// </summary>
        //public IUserData GlobalConfig
        //{
        //    get
        //    {
        //        IConfig<IUserData> gcfg = ConfigManager.getUserConfigFor(ContextType.Static);
        //        if(gcfg == null) {
        //            return null;
        //        }
        //        return gcfg.Data;
        //    }
        //}

        /// <summary>
        /// Static alias. Manager of configurations.
        /// </summary>
        public static IManager CfgManager
        {
            get { return _.ConfigManager; }
        }

        /// <summary>
        /// Static alias. Main configuration data.
        /// </summary>
        public static ISolutionEvents Cfg
        {
            get { return _.Config; }
        }

        /// <summary>
        /// Static alias. User configuration data.
        /// </summary>
        public static IUserData CfgUser
        {
            get { return _.UserConfig; }
        }

        ///// <summary>
        ///// Static alias. Global configuration data.
        ///// </summary>
        //public static IUserData CfgGlobal
        //{
        //    get { return _.GlobalConfig; }
        //}

        /// <summary>
        /// Static alias. Working path for library.
        /// </summary>
        public static string WPath
        {
            get { return _.WorkPath; }
        }

        /// <summary>
        /// Static alias. Full path to library.
        /// </summary>
        public static string LPath
        {
            get { return _.LibPath; }
        }

        /// <summary>
        /// Updates working path for library.
        /// </summary>
        /// <param name="path">New path.</param>
        public void setWorkPath(string path)
        {
            workPath = path.DirectoryPathFormat();
            WorkPathUpdated(this, new DataArgs<string>() { Data = workPath });
        }

        /// <summary>
        /// Thread-safe getting instance from Settings.
        /// </summary>
        public static IAppSettings _
        {
            get { return _lazy.Value; }
        }
        private static readonly Lazy<Settings> _lazy = new Lazy<Settings>(() => new Settings());

        private Settings() { }
    }
}
