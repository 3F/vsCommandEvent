﻿/* 
 * Boost Software License - Version 1.0 - August 17th, 2003
 * 
 * Copyright (c) 2013 Developed by reg <entry.reg@gmail.com>
 * 
 * Permission is hereby granted, free of charge, to any person or organization
 * obtaining a copy of the software and accompanying documentation covered by
 * this license (the "Software") to use, reproduce, display, distribute,
 * execute, and transmit the Software, and to prepare derivative works of the
 * Software, and to permit third-parties to whom the Software is furnished to
 * do so, all subject to the following:
 * 
 * The copyright notices in the Software and this entire statement, including
 * the above license grant, this restriction and the following disclaimer,
 * must be included in all copies of the Software, in whole or in part, and
 * all derivative works of the Software, unless such copies or derivative
 * works are solely in the form of machine-executable object code generated by
 * a source language processor.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
 * SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
 * FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE. 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace net.r_eg.vsSBE
{
    public class MSBuildParser: IMSBuildProperty, ISBEParserScript
    {
        /// <summary>
        /// DTE context
        /// </summary>
        protected DTE2 dte2 = null;

        /// <summary>
        /// Getting name from "Set as SturtUp Project"
        /// </summary>
        protected string StartupProjectString
        {
            get {
                foreach(string project in (Array)dte2.Solution.SolutionBuild.StartupProjects) {
                    return project;
                }
                return null;
            }
        }

        protected SolutionConfiguration2 SolutionActiveConfiguration
        {
            get { return (SolutionConfiguration2)dte2.Solution.SolutionBuild.ActiveConfiguration; }
        }

        /// <summary>
        /// Flag of optimisation reload projects.
        /// See _reloadProjectCollection() for detail
        /// TODO: https://bitbucket.org/3F/vssolutionbuildevent/issue/5/msbuild-properties-are-empty-not-listed
        /// </summary>
        private static bool _flagBugLoadProject = true;

        /// <summary>
        /// object synch.
        /// </summary>
        private Object _eLock = new Object();

        /// <summary>
        /// MSBuild Property from default Project
        /// </summary>
        /// <param name="name">key property</param>
        /// <returns>evaluated value</returns>
        public string getProperty(string name)
        {
            return getProperty(name, null);
        }

        /// <summary>
        /// MSBuild Property from specific project
        /// </summary>
        /// <param name="name">key property</param>
        /// <param name="projectName">project name</param>
        /// <exception cref="MSBuildParserProjectPropertyNotFoundException">problem with getting property</exception>
        /// <returns>evaluated value</returns>
        public string getProperty(string name, string projectName)
        {
            Project project         = getProject(projectName);
            ProjectProperty prop    = project.GetProperty(name);

            if(prop != null) {
                return prop.EvaluatedValue;
            }
            throw new MSBuildParserProjectPropertyNotFoundException(String.Format("variable - '{0}' : project - '{1}'", name, (projectName == null) ? "<default>" : projectName));
        }

        public List<MSBuildPropertyItem> listProperties(string projectName = null)
        {
            List<MSBuildPropertyItem> properties = new List<MSBuildPropertyItem>();

            Project project = getProject(projectName);
            foreach(ProjectProperty property in project.Properties) {
                properties.Add(new MSBuildPropertyItem(property.Name, property.EvaluatedValue));
            }
            return properties;
        }

        public List<string> listProjects()
        {
            List<string> projects           = new List<string>();
            IEnumerator<Project> eprojects  = _loadedProjects();

            while(eprojects.MoveNext())
            {
                string projectName = eprojects.Current.GetPropertyValue("ProjectName");
                if(!String.IsNullOrEmpty(projectName) && isActiveConfiguration(eprojects.Current)) {
                    projects.Add(projectName);
                }
            }
            return projects;
        }

        /// <summary>
        /// Evaluate data with the MSBuild engine
        /// </summary>
        /// <param name="unevaluated">raw string as $(..data..)</param>
        /// <param name="projectName">push null if default</param>
        /// <returns>evaluated value</returns>
        public virtual string evaluateVariable(string unevaluated, string projectName)
        {
            Project project = getProject(projectName);
            lock(_eLock) {
                project.SetProperty("vsSBE_latestEvaluated", unevaluated);
            }
            return project.GetProperty("vsSBE_latestEvaluated").EvaluatedValue;
        }

        /// <summary>
        /// Simple handler properties of MSBuild environment
        /// </summary>
        /// <remarks>deprecated</remarks>
        /// <param name="data">text with $(ident) data</param>
        /// <returns>text with evaluated properties</returns>
        public string parseVariablesMSBuildSimple(string data)
        {
            return Regex.Replace(data, @"
                                         (?<!\$)\$
                                         \(
                                           (?:
                                             (
                                               [^\:\r\n)]+?
                                             )
                                             \:
                                             (
                                               [^)\r\n]+?
                                             )
                                             |
                                             (
                                               [^)]*?
                                             )
                                           )
                                         \)", delegate(Match m)
            {
                // 3   -> $(name)
                // 1,2 -> $(name:project)

                if(m.Groups[3].Success) {
                    return getProperty(m.Groups[3].Value);
                }
                return getProperty(m.Groups[1].Value, m.Groups[2].Value);

            }, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).Replace("$$(", "$(");
        }

        public virtual string parseVariablesMSBuild(string data)
        {
            /*
                    (
                      \${1,2}
                    )
                    (?=
                      (
                        \(
                          (?>
                            [^()]
                            |
                            (?2)
                          )*
                        \)
                      )
                    )            -> for .NET: v             
             */
            return Regex.Replace(data,  @"(
                                            \${1,2}
                                          )
                                          (
                                            \(
                                              (?>
                                                [^()]
                                                |
                                                \((?<R>)
                                                |
                                                \)(?<-R>)
                                              )*
                                              (?(R)(?!))
                                            \)
                                          )", delegate(Match m)
            {
                // 1 - $ or $$
                // 2 - (name) or (name:project) or ([MSBuild]::MakeRelative($(path1), ...):project) .. 
                //      http://msdn.microsoft.com/en-us/library/vstudio/dd633440%28v=vs.120%29.aspx

                if(m.Groups[1].Value.Length > 1) { //escape
                    return m.Value.Substring(1);
                }

                string unevaluated  = m.Groups[2].Value;
                string projectName  = _splitGeneralProjectAttr(ref unevaluated);

                if(_isSimpleProperty(ref unevaluated)) {
                    return getProperty(unevaluated, projectName);
                }
                return evaluateVariable(string.Format("$({0})", unevaluated), projectName);
            }, RegexOptions.IgnorePatternWhitespace);
        }


        /// <summary>
        /// All variables which are not included in MSBuild environment.
        /// Customization for our data
        /// </summary>
        /// <param name="data">where to look</param>
        /// <param name="name">we're looking for..</param>
        /// <param name="value">replace with this value if found</param>
        /// <returns></returns>
        public string parseCustomVariable(string data, string name, string value)
        {
            return Regex.Replace(data,  @"(
                                            \${1,2}
                                          )
                                          \(
                                            (
                                              [^)]+?
                                            )
                                          \)", delegate(Match m)
            {
                if(m.Groups[2].Value != name || m.Groups[1].Value.Length > 1) {
                    return m.Value;
                }
                return (value == null)? "" : value;
            }, RegexOptions.IgnorePatternWhitespace);
        }

        /// <param name="dte2">DTE context</param>
        public MSBuildParser(DTE2 dte2)
        {
            this.dte2 = dte2;

#if DEBUG
            string unevaluated = "(name:project)";
            Debug.Assert(_splitGeneralProjectAttr(ref unevaluated).CompareTo("project") == 0);
            Debug.Assert(unevaluated.CompareTo("name") == 0);

            unevaluated = "(name)";
            Debug.Assert(_splitGeneralProjectAttr(ref unevaluated) == null);
            Debug.Assert(unevaluated.CompareTo("name") == 0);

            unevaluated = "([class]::func($(path:project), $([class]::func2($(path2)):project)):project)";
            Debug.Assert(_splitGeneralProjectAttr(ref unevaluated).CompareTo("project") == 0);
            Debug.Assert(unevaluated.CompareTo("[class]::func($(path:project), $([class]::func2($(path2)):project))") == 0);

            unevaluated = "([class]::func($(path:project), $([class]::func2($(path2)):project)):project))";
            Debug.Assert(_splitGeneralProjectAttr(ref unevaluated) == null);
            Debug.Assert(unevaluated.CompareTo("[class]::func($(path:project), $([class]::func2($(path2)):project)):project)") == 0);
#endif
        }

        /// <summary>
        /// get default project for access to properties etc.
        /// Startup-project in the list or the first with the same Configuration & Platform
        /// </summary>
        /// <exception cref="MSBuildParserProjectNotFoundException">something wrong with loaded projects</exception>
        /// <returns>Microsoft.Build.Evaluation.Project</returns>
        protected virtual Project getProjectDefault()
        {
            Project ret                     = null;
            IEnumerator<Project> eprojects  = _loadedProjects();

            while(eprojects.MoveNext())
            {
                bool isActive = isActiveConfiguration(eprojects.Current);
                Log.nlog.Trace(String.Format("getProjectDefault: '{0}' isActive = {1} [Sturtup: {2}]", eprojects.Current.FullPath, isActive, StartupProjectString));

                if(!String.IsNullOrEmpty(StartupProjectString) 
                    && isActive && _cmpPathBEProjectWithString(eprojects.Current, StartupProjectString))
                {
                    Log.nlog.Trace("ret selected");
                    ret = eprojects.Current;
                    break;
                }

                if(ret == null && isActive) {
                    ret = eprojects.Current; // by default if there are problems with the StartupProjectString
                }
            }

            // Solution context for selected
            if(ret != null) {
                //TODO: private storage get/set for specific properties from Solution-context.
                //      Currently overrides only with reloading GlobalProjectCollection, because this prop should be reset if project selected by manually.
                ret.SetGlobalProperty("Configuration", SolutionActiveConfiguration.Name);
                ret.SetGlobalProperty("Platform", SolutionActiveConfiguration.PlatformName);

                Log.nlog.Debug("Selected default project: " + ret.FullPath);
                Log.nlog.Debug(String.Format("Override properties: ({0}, {1})", SolutionActiveConfiguration.Name, SolutionActiveConfiguration.PlatformName));
                return ret;
            }
            throw new MSBuildParserProjectNotFoundException("not found project: <default>");
        }

        /// <exception cref="MSBuildParserProjectNotFoundException">if not found the specific project</exception>
        protected virtual Project getProject(string project)
        {
            if(project == null) {
                return getProjectDefault();
            }

            IEnumerator<Project> eprojects = _loadedProjects();
            while(eprojects.MoveNext()) {
                if(eprojects.Current.GetPropertyValue("ProjectName").Equals(project) && isActiveConfiguration(eprojects.Current)) {
                    return eprojects.Current;
                }
            }
            throw new MSBuildParserProjectNotFoundException(String.Format("not found project: '{0}'", project));
        }

        protected bool isActiveConfiguration(Project project)
        {
            string configuration    = project.GetPropertyValue("Configuration");
            string platform         = project.GetPropertyValue("Platform");

            foreach(EnvDTE.SolutionContext sln in SolutionActiveConfiguration.SolutionContexts)
            {
                Log.nlog.Trace(String.Format("isActiveConfiguration for '{0}' : '{1}' [{2} = {3} ; {4} = {5}]",
                                              project.FullPath, sln.ProjectName, sln.ConfigurationName, configuration, sln.PlatformName, platform));

                if(_cmpPathBEProjectWithString(project, sln.ProjectName)
                    && sln.ConfigurationName.Equals(configuration) && sln.PlatformName.Equals(platform))
                {
                    Log.nlog.Trace("isActiveConfiguration: matched");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This solution for similar problems - MS Connect Issue #508628:
        /// http://connect.microsoft.com/VisualStudio/feedback/details/508628/
        /// </summary>
        private void _reloadProjectCollection()
        {
            Log.nlog.Debug(string.Format("Solution.Projects = {0}", SolutionActiveConfiguration.SolutionContexts.Count));
            foreach(EnvDTE.SolutionContext sln in SolutionActiveConfiguration.SolutionContexts)
            {
                string pFile = System.IO.Path.Combine(Config.WorkPath, sln.ProjectName);

                Dictionary<string, string> prop = new Dictionary<string, string>();
                prop["Configuration"]   = sln.ConfigurationName;
                prop["Platform"]        = sln.PlatformName;

                Log.nlog.Trace(string.Format("reload->ActiveConfiguration :: '{0}' [{1} ; {2}]", pFile, sln.ConfigurationName, sln.PlatformName));
                //TODO: optimize - only what we need. Currently, this for fixes problem with GlobalProjectCollection
                ProjectCollection.GlobalProjectCollection.LoadProject(pFile, prop, null);
            }
        }

        /// <exception cref="MSBuildParserProjectNotFoundException"></exception>
        private IEnumerator<Project> _loadedProjects()
        {
            if(_flagBugLoadProject) {
                Log.nlog.Debug("call UnloadAllProjects()");
                //TODO: unloaded all because we have a problem with GlobalProjectCollection on some VS versions
                //      see _reloadProjectCollection. For optimize we can also load only what we need... fix me
                ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
            }

            ICollection<Project> prgs = ProjectCollection.GlobalProjectCollection.LoadedProjects;
            if(prgs == null || prgs.Count < 1) // https://bitbucket.org/3F/vssolutionbuildevent/issue/3/
            {
                _flagBugLoadProject = true; // on some VS versions

                Log.nlog.Debug("call _reloadProjectCollection()");
                _reloadProjectCollection();
            }

            prgs = ProjectCollection.GlobalProjectCollection.LoadedProjects;
            if(prgs == null || prgs.Count < 1) { //if still...
                throw new MSBuildParserProjectNotFoundException("not loaded projects");
            }
            return prgs.GetEnumerator();
        }

        /// <summary>
        /// Getting the project name and format unevaluated variable
        /// ~ (variable:project) -> variable & project
        /// </summary>
        /// <param name="unevaluated">to be formatted after handling</param>
        /// <returns>project name or null if not present</returns>
        private string _splitGeneralProjectAttr(ref string unevaluated)
        {
            unevaluated = unevaluated.Substring(1, unevaluated.Length - 2);
            int pos     = unevaluated.LastIndexOf(':');

            if(pos == -1) {
                return null; //(ProjectOutputFolder.Substring(0, 1)), (OS), (OS.Length)
            }
            if(unevaluated.ElementAt(pos - 1).CompareTo(':') == 0) {
                return null; //([System.DateTime]::Now), ([System.Guid]::NewGuid())
            }
            if(unevaluated.IndexOf(')', pos) != -1) {
                return null; // allow only for latest block (general option)  :project)) | :project) ... )-> :project)
            }

            string project  = unevaluated.Substring(pos + 1, unevaluated.Length - pos - 1);
            unevaluated     = unevaluated.Substring(0, pos);

            return project;
        }

        private bool _isSimpleProperty(ref string unevaluated)
        {
            if(unevaluated.IndexOfAny(new char[]{'.', ':', '(', ')', '\'', '"'}) != -1) {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Comparison of paths projects
        /// </summary>
        /// <param name="eProject">Microsoft.Build.Evaluation.Project provides absolute path with the FullPath property</param>
        /// <param name="project">
        /// project name with any path, e.g.: 
        ///   * 'C:\Projects\Foo\Foo.csproj' == '..\..\Foo\Foo.csproj'
        ///   * 'C:\Projects\Foo\Foo.csproj' == 'Foo.csproj'
        ///   * 'C:\Projects\Foo\Foo.csproj' == 'C:\Projects\Foo\Foo.csproj'
        ///   
        /// The properties below can provide the path + name:
        ///   * EnvDTE.Project.UniqueName
        ///   * EnvDTE.SolutionBuild.StartupProjects
        ///   * EnvDTE.SolutionContext.ProjectName
        /// </param>
        /// <returns>true if equal</returns>
        private bool _cmpPathBEProjectWithString(Project eProject, string project)
        {
            if(String.IsNullOrEmpty(project)) {
                return false;
            }
            string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Config.WorkPath, project));

            Log.nlog.Trace(String.Format("_cmpPathBEProjectWithString: '{0}' :: '{1}' :: '{2}' == {3}", Config.WorkPath, project, path, eProject.FullPath));
            return path.Equals(eProject.FullPath);
        }
    }

    /// <summary>
    /// item of property: name = value
    /// </summary>
    public sealed class MSBuildPropertyItem
    {
        public string name;
        public string value;

        public MSBuildPropertyItem(string name, string value)
        {
            this.name  = name;
            this.value = value;
        }
    }

    //TODO:
    public struct SBECustomVariable
    {
        public const string OWP_BUILD           = "vsSBE_OWPBuild";
        public const string OWP_BUILD_WARNINGS  = "vsSBE_OWPBuildWarnings";
        public const string OWP_BUILD_ERRORS    = "vsSBE_OWPBuildErrors";
    }
}
