﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace net.r_eg.vsCE.UI {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("net.r_eg.vsCE.UI.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon Package_32 {
            get {
                object obj = ResourceManager.GetObject("Package_32", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using ICommand = net.r_eg.vsCE.Actions.ICommand;
        ///using ISolutionEvent = net.r_eg.vsCE.Events.ISolutionEvent;
        ///
        ///namespace vsCommandEvent
        ///{
        ///    public class CSharpMode
        ///    {
        ///        public static int Init(ICommand cmd, ISolutionEvent evt)
        ///        {
        ///            return 0;
        ///        }
        ///    }
        ///}.
        /// </summary>
        internal static string StringCSharpModeCodeByDefault {
            get {
                return ResourceManager.GetString("StringCSharpModeCodeByDefault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Project ToolsVersion=&quot;4.0&quot; xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
        ///
        ///    &lt;Target Name=&quot;Init&quot;&gt;
        ///        &lt;!-- ... --&gt;
        ///    &lt;/Target&gt;
        ///
        ///    &lt;!--
        ///        Additional properties:
        ///            $(ActionName)
        ///            $(BuildType)
        ///            $(EventType)
        ///            $(SupportMSBuild)
        ///            $(SupportSBEScripts)
        ///            $(SolutionActiveCfg)
        ///            $(StartupProject)
        ///    --&gt;
        ///&lt;/Project&gt;.
        /// </summary>
        internal static string StringDefaultValueForTargetsMode {
            get {
                return ResourceManager.GetString("StringDefaultValueForTargetsMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot;
        ///    Sample
        ///&quot;]
        ///#[var v = 1.2.3]
        ///#[var log = $(TMP)/v.txt]
        ///
        ///#[($(Configuration) ~= Deb || true)
        ///{
        ///    #[var tStart    = $([System.DateTime]::Parse(&quot;%mdate%&quot;).ToBinary())]
        ///    #[var tNow      = $([System.DateTime]::UtcNow.Ticks)]
        ///    #[var revBuild  = $([System.TimeSpan]::FromTicks($([MSBuild]::Subtract(#[var tNow], #[var tStart]))).TotalMinutes.ToString(&quot;0&quot;))]
        ///    #[var v         = #[var v].$([MSBuild]::Modulo(#[var revBuild], $([System.Math]::Pow(2, 14))))]
        ///}]
        ///
        ///#[var v = $([System.String]:: [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string StringScriptExampleSBE {
            get {
                return ResourceManager.GetString("StringScriptExampleSBE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This option is means a wait for completion action with main thread! therefore:
        ///* [Performance] - For repeated events such as EW, OWP, Logging etc. probably need a more time for services of your all defined actions. Remember this!
        ///* [Compatibility] - However, some your actions can require a lock of the main thread for waiting to the next steps, otherwise some actions can lose a some technical data for processes of your script on next step..
        /// </summary>
        internal static string StringWarnForWaiting {
            get {
                return ResourceManager.GetString("StringWarnForWaiting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#ifndef VSCE_VERSION_H_
        ///#define VSCE_VERSION_H_
        ///
        ///#define VER_NUMBER_STRING           L&quot;0.12.4&quot;;
        ///#define VER_NUMBER_WITH_REV_STRING  L&quot;0.12.4.17639&quot;;
        ///#define VER_BRANCH_NAME             L&quot;develop&quot;;
        ///#define VER_BRANCH_SHA1             L&quot;e3de826&quot;;
        ///#define VER_BRANCH_REV_COUNT        L&quot;296&quot;;
        ///#define VER_INFORMATIONAL           L&quot;0.12.4.17639 [ e3de826 ]&quot;;
        ///#define VER_INFORMATIONAL_FULL      L&quot;0.12.4.17639 [ e3de826 ] /&apos;develop&apos;:296&quot;;
        ///
        ///#endif.
        /// </summary>
        internal static string WizardVerCppDefine {
            get {
                return ResourceManager.GetString("WizardVerCppDefine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#ifndef VSCE_VERSION_H_
        ///#define VSCE_VERSION_H_
        ///
        ///#include &lt;string&gt;
        ///
        ///namespace example
        ///{
        ///    struct Version
        ///    {
        ///        struct TNum
        ///        {
        ///            const int major;
        ///            const int minor;
        ///            const int build;
        ///            const int revision;
        ///
        ///            TNum(int major, int minor, int build = 0, int revision = 0) 
        ///                : major(major), minor(minor), build(build), revision(revision) { }
        ///
        ///            TNum() : TNum(0, 12, 4, 17639) { }
        ///
        ///      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WizardVerCppStruct {
            get {
                return ResourceManager.GetString("WizardVerCppStruct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to namespace example
        ///{
        ///    internal struct Version
        ///    {
        ///        public static readonly System.Version number    = new System.Version(0, 12, 4, 17639);
        ///        public const string numberString                = &quot;0.12.4&quot;;
        ///        public const string numberWithRevString         = &quot;0.12.4.17639&quot;;
        ///        public const string branchName                  = &quot;develop&quot;;
        ///        public const string branchSha1                  = &quot;e3de826&quot;;
        ///        public const string branchRevCount              = &quot;296&quot;;
        ///         [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WizardVerCSharpStruct {
            get {
                return ResourceManager.GetString("WizardVerCSharpStruct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!-- the mixed format --&gt;
        ///&lt;Identifier Id=&quot;C972EFAB-8642-444F-8033-FF5C3720E97F&quot;&gt;
        ///    &lt;Name&gt;AwesomeApp&lt;/Name&gt;
        ///    &lt;Author&gt;Mr.Smith&lt;/Author&gt;
        ///&gt;&gt;&gt; &lt;Version&gt;0.12.4.17639&lt;/Version&gt; &lt;&lt;&lt;
        ///    &lt;Description xml:space=&quot;preserve&quot;&gt;&lt;/Description&gt;
        ///    &lt;Locale&gt;1033&lt;/Locale&gt;
        ///    &lt;InstalledByMsi&gt;false&lt;/InstalledByMsi&gt;
        ///    &lt;SupportedProducts&gt;
        ///        &lt;VisualStudio Version=&quot;14.0&quot;&gt;
        ///            &lt;Edition&gt;Enterprise&lt;/Edition&gt;
        ///            &lt;Edition&gt;Ultimate&lt;/Edition&gt;
        ///            &lt;Edition&gt;Premium&lt;/Edition&gt;
        ///            &lt;Ed [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WizardVerDirectRepl {
            get {
                return ResourceManager.GetString("WizardVerDirectRepl", resourceCulture);
            }
        }
    }
}