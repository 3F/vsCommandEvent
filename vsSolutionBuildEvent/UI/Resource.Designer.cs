﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace net.r_eg.vsSBE.UI {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("net.r_eg.vsSBE.UI.Resource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string similar to using ICommand = net.r_eg.vsSBE.Actions.ICommand;
        ///using ISolutionEvent = net.r_eg.vsSBE.Events.ISolutionEvent;
        ///
        ///namespace vsSolutionBuildEvent
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
        ///&lt;Project DefaultTargets=&quot;Build&quot; ToolsVersion=&quot;12.0&quot; xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
        ///    &lt;!-- ... --&gt;
        ///&lt;/Project&gt;.
        /// </summary>
        internal static string StringDefaultValueForTargetsMode {
            get {
                return ResourceManager.GetString("StringDefaultValueForTargetsMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot;
        ///      Example
        ///&quot;]
        ///#[var ver = v1.2.3]
        ///#[var log = $(TMP)/ver.txt]
        ///
        ///#[($(Configuration) ~= Deb || true) {
        ///    #[var tStart   = $([System.DateTime]::Parse(&quot;%mdate%&quot;).ToBinary())]
        ///    #[var tNow     = $([System.DateTime]::UtcNow.Ticks)]
        ///    #[var revBuild = $([System.TimeSpan]::FromTicks($([MSBuild]::Subtract(#[var tNow], #[var tStart]))).TotalMinutes.ToString(&quot;0&quot;))]
        ///    #[var ver      = #[var ver].#[var revBuild]]
        ///}]
        ///
        ///#[File write(&quot;#[var log]&quot;):&gt; Example #[var ver] \r\n\t\t Generated by vsSolu [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string StringScriptExampleSBE {
            get {
                return ResourceManager.GetString("StringScriptExampleSBE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This option mean a wait for completion action with main thread! therefore:
        ///* [Performance] - For repeated events such as EW, OWP, Logging etc. probably need a more time for services of your all defined actions. Remember this!
        ///* [Compatibility] - However, some your actions can require a lock of the main thread for waiting to the next steps, otherwise some actions can lose a some technical data for processes of your script on next step..
        /// </summary>
        internal static string StringWarnForWaiting {
            get {
                return ResourceManager.GetString("StringWarnForWaiting", resourceCulture);
            }
        }
    }
}
