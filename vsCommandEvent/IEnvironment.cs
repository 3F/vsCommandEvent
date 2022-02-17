﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Collections.Generic;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Bridge;
using net.r_eg.vsCE.Events;
using DProject = EnvDTE.Project;
using EProject = Microsoft.Build.Evaluation.Project;

namespace net.r_eg.vsCE
{
    [Guid("27F04A53-A0B9-431B-83FE-827AC09FB127")]
    public interface IEnvironment
    {
        /// <summary>
        /// List of EnvDTE projects.
        /// </summary>
        IEnumerable<DProject> ProjectsDTE { get; }

        /// <summary>
        /// List of Microsoft.Build.Evaluation projects.
        /// </summary>
        IEnumerable<EProject> ProjectsMBE { get; }

        /// <summary>
        /// Simple list of names from EnvDTE projects
        /// </summary>
        List<string> ProjectsList { get; }

        /// <summary>
        /// Active configuration for current solution.
        /// </summary>
        EnvDTE80.SolutionConfiguration2 SolutionActiveCfg { get; }

        /// <summary>
        /// Formatted string with an active configuration for current solution.
        /// </summary>
        string SolutionActiveCfgString { get; }

        /// <summary>
        /// Current context for actions.
        /// </summary>
        BuildType BuildType { get; set; }

        /// <summary>
        /// All configurations for current solution.
        /// </summary>
        IEnumerable<EnvDTE80.SolutionConfiguration2> SolutionConfigurations { get; }

        /// <summary>
        /// Project Name by default or "StartUp Project".
        /// </summary>
        string StartupProjectString { get; }

        /// <summary>
        /// DTE2 context.
        /// </summary>
        EnvDTE80.DTE2 Dte2 { get; }

        /// <summary>
        /// Events in the extensibility model
        /// </summary>
        EnvDTE.Events Events { get; }

        AggregatedEventsEnvDte AggregatedEvents { get; }

        /// <summary>
        /// Get status of opened solution.
        /// </summary>
        bool IsOpenedSolution { get; }

        /// <summary>
        /// Full path to directory where placed solution file.
        /// </summary>
        string SolutionPath { get; }

        /// <summary>
        /// Full path to solution file.
        /// </summary>
        string SolutionFile { get; }

        /// <summary>
        /// Name of used solution file without extension
        /// </summary>
        string SolutionFileName { get; }

        /// <summary>
        /// Contains all of the commands in the environment
        /// </summary>
        EnvDTE.Commands Commands { get; }

        /// <summary>
        /// Access to OutputWindowPane through IOW
        /// </summary>
        IOW OutputWindowPane { get; }

        /// <summary>
        /// Get instance of the Build.Evaluation.Project for accessing to properties etc.
        /// </summary>
        /// <param name="name">Specified project name. null value will use the name from startup-project.</param>
        /// <returns>Found relevant Microsoft.Build.Evaluation.Project.</returns>
        EProject getProject(string name = null);

        /// <summary>
        /// Returns formatted configuration from the SolutionConfiguration2
        /// </summary>
        string SolutionCfgFormat(EnvDTE80.SolutionConfiguration2 cfg);

        /// <summary>
        /// Getting an unified property for all existing projects. 
        /// Aka "Solution property".
        /// </summary>
        /// <param name="name">Property name</param>
        string getSolutionProperty(string name);

        /// <summary>
        /// Execution command with DTE
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="args">Command arguments</param>
        void exec(string name, string args = "");

        /// <summary>
        /// Raise Command ID for EnvDTE.
        /// </summary>
        /// <param name="guid">Scope by Guid.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        void raise(string guid, int id, object customIn, object customOut);

        /// <summary>
        /// To update the Project Name by default aka "StartUp Project".
        /// </summary>
        /// <param name="name">Uses default behavior if empty or null.</param>
        void updateStartupProject(string name);
    }
}
