﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.IO;
using System.Linq;
using net.r_eg.MvsSln;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Exceptions;
using BuildType = net.r_eg.vsCE.Bridge.BuildType;
using EProject = Microsoft.Build.Evaluation.Project;
using ProjectItem = net.r_eg.MvsSln.Core.ProjectItem;

namespace net.r_eg.vsCE
{
    public abstract class EnvAbstract
    {
        /// <summary>
        /// Project by default or "StartUp Project".
        /// </summary>
        public abstract string StartupProjectString { get; protected set; }

        /// <summary>
        /// Full path to solution file.
        /// </summary>
        public abstract string SolutionFile { get; protected set; }

        protected abstract void UpdateSlnEnv(ISlnResult sln);

        /// <summary>
        /// Current context for actions.
        /// </summary>
        public BuildType BuildType
        {
            get;
            set;
        } = BuildType.Common;

        /// <summary>
        /// Access to parsed solution data.
        /// </summary>
        protected ISlnResult Sln
        {
            get => UpdateSln();
            set => _sln = value;

        } protected ISlnResult _sln;

        /// <summary>
        /// Activated environment for projects processing.
        /// </summary>
        protected IXProjectEnv SlnEnv
        {
            get
            {
                UpdateSln();
                return _slnEnv;
            }
            set => _slnEnv = value;

        } protected IXProjectEnv _slnEnv;

        /// <summary>
        /// Get instance of the Build.Evaluation.Project for accessing to properties etc.
        /// </summary>
        /// <param name="name">Specified project name. null value will use the name from startup-project.</param>
        /// <returns>Found relevant Microsoft.Build.Evaluation.Project.</returns>
        public virtual EProject getProject(string name = null)
        {
            // NOTE: Do not use ProjectCollection.GlobalProjectCollection from EnvDTE Environment because it can be empty.
            //       https://github.com/3F/vsSolutionBuildEvent/issues/8
            //       Either use DTE projects collection to refer to MBE projects, or use MvsSln's GetOrLoadProject

            Log.Trace($"getProject: started with '{name}' /{StartupProjectString}");

            if(String.IsNullOrEmpty(name)) {
                name = StartupProjectString;
            }

            ProjectItem project = Sln.ProjectItems.FirstOrDefault(p => p.name == name);
            if(project.fullPath == null) {
                throw new NotFoundException($"Project '{name}' was not found. ['{project.name}', '{project.pGuid}']");
            }

            return SlnEnv?.GetOrLoadProject(project);
        }

        /// <summary>
        /// Returns formatted configuration from the SolutionConfiguration2
        /// </summary>
        public string SolutionCfgFormat(EnvDTE80.SolutionConfiguration2 cfg)
        {
            if(cfg == null) {
                return formatCfg(PropertyNames.UNDEFINED);
            }
            return formatCfg(cfg.Name, cfg.PlatformName);
        }

        protected string formatCfg(string name, string platform = null)
        {
            return ConfigItem.Format(name, platform ?? name);
        }

        private ISlnResult UpdateSln()
        {
            var input = SolutionFile;

            if(input == null) {
                throw new ArgumentNullException(nameof(SolutionFile));
            }

            if(_sln?.SolutionFile == input) {
                return _sln;
            }

            if(!File.Exists(input)) { // may not exist at this invoking when creating new solution)
                throw new NotFoundException($"Sln file does not exist: {input}.");
            }

            Log.Debug($"Updating sln data: {input}");

            _sln = new SlnParser().Parse
            (
                input,
                SlnItems.Projects | SlnItems.SolutionConfPlatforms | SlnItems.ProjectConfPlatforms
            );

            UpdateSlnEnv(_sln);
            return _sln;
        }
    }
}