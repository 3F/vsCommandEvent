﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace net.r_eg.vsCE.UI.WForms.Wizards.Version {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("net.r_eg.vsCE.UI.WForms.Wizards.Version.Resource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#ifndef VSCE_VERSION_H_
        ///#define VSCE_VERSION_H_
        ///
        ///!Items!
        ///
        ///#endif.
        /// </summary>
        internal static string CppDefineTpl {
            get {
                return ResourceManager.GetString("CppDefineTpl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to struct TNum
        ///        {
        ///            const int major;
        ///            const int minor;
        ///            const int build;
        ///            const int revision;
        ///
        ///            TNum(int major, int minor, int build = 0, int revision = 0) 
        ///                : major(major), minor(minor), build(build), revision(revision) { }
        ///
        ///            TNum() : TNum(!VerNum!) { }
        ///
        ///        } !FieldName!;.
        /// </summary>
        internal static string CppStructNumTpl {
            get {
                return ResourceManager.GetString("CppStructNumTpl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#ifndef VSCE_VERSION_H_
        ///#define VSCE_VERSION_H_
        ///!IncString!
        ///namespace !Namespace!
        ///{
        ///    !KWRef!struct !StructName!
        ///    {
        ///!Items!
        ///    }!DefVariable!;
        ///};
        ///
        ///#endif.
        /// </summary>
        internal static string CppStructTpl {
            get {
                return ResourceManager.GetString("CppStructTpl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to namespace !Namespace!
        ///{
        ///    internal struct !StructName!
        ///    {
        ///!Items!
        ///    }
        ///}.
        /// </summary>
        internal static string CSharpStructTpl {
            get {
                return ResourceManager.GetString("CSharpStructTpl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // This code was generated by a vsCommandEvent. 
        ///   Changes to this file may cause incorrect behavior and will be lost if the code is regenerated..
        /// </summary>
        internal static string Header {
            get {
                return ResourceManager.GetString("Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot; 
        ///    Prepare data
        ///&quot;]
        ///!InputNum!
        ///#[var fdata = !Fout!]
        ///
        ///!Revision!
        ///!BasicData!
        ///#[&quot; 
        ///    Update data
        ///&quot;]
        ///#[File replace.!RType!(&quot;#[var fdata]&quot;, &quot;!Pattern!&quot;, &quot;!Replacement!&quot;)].
        /// </summary>
        internal static string ScriptDirectRepl {
            get {
                return ResourceManager.GetString("ScriptDirectRepl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot; 
        ///    Prepare data
        ///&quot;]
        ///!InputNum!
        ///#[var fout  = !Fout!]
        ///
        ///#[var tpl = !Template!]
        ///
        ///!Revision!
        ///#[&quot; 
        ///    Remove placeholders
        ///&quot;]
        ///!ReplaceVersion!!ReplaceVerString!
        ///
        ///!SCM!
        ///#[&quot; 
        ///    Save result
        ///&quot;]
        ///#[File write(&quot;#[var fout]&quot;):#[var tpl]].
        /// </summary>
        internal static string ScriptMain {
            get {
                return ResourceManager.GetString("ScriptMain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[( $(Revision) === &quot;*Undefined*&quot; ) {
        ///    #[var Revision = 0]
        ///}].
        /// </summary>
        internal static string ScriptRevisionRaw {
            get {
                return ResourceManager.GetString("ScriptRevisionRaw", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot; 
        ///    Calculate revision
        ///&quot;]
        ///#[var tBase     = $([System.DateTime]::Parse(&apos;!RevTime!&apos;).ToBinary())]
        ///#[var tNow      = $([System.DateTime]::UtcNow.Ticks)]
        ///#[var Revision  = $([System.TimeSpan]::FromTicks(&apos;$([MSBuild]::Subtract($(tNow), $(tBase)))&apos;).!RevType!.ToString(&apos;0&apos;))]!RevModulo!.
        /// </summary>
        internal static string ScriptRevisionTimeDelta {
            get {
                return ResourceManager.GetString("ScriptRevisionTimeDelta", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[$(revMin = !revMin!)] #[$(revMax = !revMax!)]
        ///#[var Revision = $([MSBuild]::Add($(revMin), $([MSBuild]::Modulo($(Revision), $([MSBuild]::Subtract($(revMax), $(revMin)))))))].
        /// </summary>
        internal static string ScriptRevTimeModulo {
            get {
                return ResourceManager.GetString("ScriptRevTimeModulo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot; 
        ///    Checking of the git folder +tool &amp; define sha1, branch name, etc.
        ///&quot;]
        ///#[( #[IO exists.directory(&quot;.git&quot;)] &amp;&amp; #[IO exists.file(&quot;git.exe&quot;, true)] )
        ///{
        ///    #[var bSha1     = #[IO sout(&quot;git&quot;, &quot;rev-parse --short HEAD&quot;)]]
        ///    #[var bName     = #[IO sout(&quot;git&quot;, &quot;rev-parse --abbrev-ref HEAD&quot;)]]
        ///    #[var bRevCount = #[IO sout(&quot;git&quot;, &quot;rev-list HEAD --count&quot;)]]
        ///    !RScmData!
        ///}
        ///else {
        ///    !RScmEmpty!
        ///}].
        /// </summary>
        internal static string ScriptScmGit {
            get {
                return ResourceManager.GetString("ScriptScmGit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #[&quot; 
        ///    Checking of the git folder +tool &amp; define sha1, branch name, etc.
        ///&quot;]
        ///#[( #[IO exists.directory(&quot;.git&quot;)] &amp;&amp; #[IO exists.file(&quot;git.exe&quot;, true)] )
        ///{
        ///    !Var!
        ///}
        ///else {
        ///    !Else!
        ///}].
        /// </summary>
        internal static string ScriptScmGitBox {
            get {
                return ResourceManager.GetString("ScriptScmGitBox", resourceCulture);
            }
        }
    }
}
