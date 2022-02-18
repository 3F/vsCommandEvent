[![](https://raw.githubusercontent.com/3F/vsCommandEvent/master/vsCommandEvent/Resources/Package.png)](https://github.com/3F/vsCommandEvent) [**vsCommandEvent**](https://github.com/3F/vsCommandEvent)

Extending Visual Studio **on the fly** via [E-MSBuild]((https://github.com/3F/E-MSBuild)), [SobaScript]((https://github.com/3F/SobaScript)), C#, ...

*Feel like a master.*

```r
Copyright (c) 2015-2022  Denis Kuzmin <x-3F@outlook.com> github/3F
```

[ 「 <sub>@</sub> ☕ 」 ](https://3F.github.io/Donation/) [![LGPLv3](https://img.shields.io/badge/license-LGPLv3-008033.svg)](https://github.com/3F/vsCommandEvent/blob/master/LICENSE)

[![Build status](https://ci.appveyor.com/api/projects/status/gwmda50hdcu9esws/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/vscommandevent-2jxea/branch/master)
[![VSIX](https://img.shields.io/badge/dynamic/xml?color=6C2C7C&label=VSIX&query=//text()&url=https://raw.githubusercontent.com/3F/vsCommandEvent/master/.version)](https://visualstudiogallery.msdn.microsoft.com/ad9f19b2-04c0-46fe-9637-9a52ce4ca661/)
[![GetNuTool](https://img.shields.io/badge/🧩-GetNuTool-93C10B.svg)](https://github.com/3F/GetNuTool)
[![SobaScript](https://img.shields.io/badge/🧩-SobaScript-8E5733.svg)](https://github.com/3F/SobaScript)
[![E-MSBuild](https://img.shields.io/badge/🧩-E--MSBuild-C8597A.svg)](https://github.com/3F/E-MSBuild)

[![Build history](https://buildstats.info/appveyor/chart/3Fs/vscommandevent-2jxea?buildCount=20&showStats=true)](https://ci.appveyor.com/project/3Fs/vscommandevent-2jxea/history)

**[Download](https://github.com/3F/vsCommandEvent/releases/latest)**

* [VisualStudio Marketplace](https://visualstudiogallery.msdn.microsoft.com/ad9f19b2-04c0-46fe-9637-9a52ce4ca661/)

## Why vsCommandEvent ?

vsCommandEvent was based on [vsSolutionBuildEvent](https://github.com/3F/vsSolutionBuildEvent) engine to continue the mission of providing flexible customization for your environment and its automation.

Yet another advanced handlers of the most events but unlike the first it was focused on advanced manipulations with Visual Studio IDE and its runtime.

![](media/devenv.png)

* **[Examples](https://vsce.r-eg.net/doc/Examples/)** *- scripts, solutions, syntax etc.,*
* [vsSolutionBuildEvent engine](https://github.com/3F/vsSolutionBuildEvent)
* [Processing modes](https://vsce.r-eg.net/doc/Modes/)
    * [Script Mode](https://vsce.r-eg.net/doc/Modes/Script/)
    * [Targets Mode](https://vsce.r-eg.net/doc/Modes/Targets/)
    * [C# Mode](https://vsce.r-eg.net/doc/Modes/CSharp/)
    * [EnvCommand](https://vsce.r-eg.net/doc/Modes/EnvCommand/)
* [SobaScript](https://github.com/3F/SobaScript)
* [E-MSBuild](https://github.com/3F/E-MSBuild)
* [Wiki](https://vsce.r-eg.net/)

### Overriding commands

The development environment of any users can be flexibly changed according to your needs in a few steps.

This is possible because vsCommandEvent may override a lot of things from IDE. It also provides [flexible actions](https://vsce.r-eg.net/doc/Modes/) due to the fact that it was based on [vsSolutionBuildEvent](https://github.com/3F/vsSolutionBuildEvent). 

![](https://3F.github.io/web.vsCE/doc/Resources/examples/EnvCommand.png)

![](media/gif/AboutVS.gif)

You can even [override the 'Exit'](https://vsce.r-eg.net/doc/Examples/Overriding/) (including [X] and Alt + F4 hotkey) for Visual Studio IDE on the fly depending on some state of the document etc. 

### A new look at old things

Why not look at some similar solutions from [vsSolutionBuildEvent](https://github.com/3F/vsSolutionBuildEvent).

Although it cannot be same as for vsCommandEvent (since it was based on another technologies), let's try to look closer. Can we achieve the same result?

#### Solution-wide Build Events

vsCommandEvent initially may work with commands from VS. Thus, you can catch commands when starting the any Build operations still for the entire solution or individually for each project separately. 

[This is it](https://vsce.r-eg.net/doc/Features/Solution-wide/#how-to) ~

Description | Guid | Id | in | out
------------|------|----|----|----
Started - Build Solution |{5EFC7975-14BC-11CF-9B2B-00AA00573819} | 882 | | 
Started - Rebuild Solution |{5EFC7975-14BC-11CF-9B2B-00AA00573819} | 883 | | 
Started - Clean Solution |{5EFC7975-14BC-11CF-9B2B-00AA00573819} | 885 | | 

#### Automatic Version Numbering

Still available versioning as you prefer. Moreover, with the vsCommandEvent you can handle versioning for most operations of Visual Studio. [Just try as you need](https://vsce.r-eg.net/doc/Examples/Version%20number/).

![](https://3F.github.io/web.vsCE/doc/Resources/examples/VersionClass.gif)

#### Stop build on first error

Disturbs [Warnings] and [Errors] ?! no problem, [**manage it**](https://vsce.r-eg.net/doc/Examples/Errors.Stop%20build/):

![](https://3F.github.io/web.vsSBE/doc/Resources/examples/stop_build.png)

## Advanced Actions

*vsCommandEvent* provides most of the action types from [vsSolutionBuildEvent](https://marketplace.visualstudio.com/vsgallery/0d1dbfd7-ed8a-40af-ae39-281bfeca2334) engine:

* Files Mode, Operation Mode, Interpreter Mode, Script Mode, Targets Mode, [C# Mode](https://vsce.r-eg.net/doc/Modes/CSharp/), **and more** such as [EnvCommand Mode](https://vsce.r-eg.net/doc/Modes/EnvCommand/) etc.

Supports advanced MSBuild & SBE-Scripts engine for powerful usage. And lot of other features for the convenience of your work with the build, tests, versioning, IO operations, and so on. See the documentation.

A few [modes](https://vsce.r-eg.net/doc/Modes/) for you:

### Targets Mode

You can even work with MSBuild [Targets](https://msdn.microsoft.com/en-us/library/vstudio/ms171462.aspx) / [Tasks](https://msdn.microsoft.com/en-us/library/vstudio/ms171466.aspx) and other 'as is' (classic compatible mode).

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">

    <Target Name="Init">
        <!-- your awesome code ... -->
    </Target>
    
</Project>
```

### C# Mode

You can also choose this as an preferred [action type](https://vsce.r-eg.net/doc/Modes/CSharp/).

```csharp
namespace vsCommandEvent
{
    public class CSharpMode
    {
        public static int Init(ICommand cmd, ISolutionEvent evt)
        {
            // your awesome code ...
        }
    }
}
```

## Advanced MSBuild

Through [E-MSBuild](https://github.com/3F/E-MSBuild) engine.

```js
#[$(
    [System.Math]::Exp('$(
        [MSBuild]::Multiply(
            $([System.Math]::Log(10)), 
            4
        ))'
    )
)]
```

```js
$(n = 0)       $(desc = "Hello ")
$(n += 3.14)   $(desc += "from vsSBE !")
$(n += $(n))   $(p1 = " Platform is $(Platform)")
```
...

## #SobaScript ##

[https://github.com/3F/SobaScript](https://github.com/3F/SobaScript) -- Extensible Modular Scripting Programming Language.

```js
#["
    Basic example
"]
#[var v = 1.2.3]
#[var log = $(TMP)/v.txt]

#[($(Configuration) ~= Deb || true)
{
    #[var tBase     = $([System.DateTime]::Parse('2015/10/01').ToBinary())]
    #[var tNow      = $([System.DateTime]::UtcNow.Ticks)]
    #[var revBuild  = #[$(
                        [System.TimeSpan]::FromTicks('$(
                            [MSBuild]::Subtract(
                            $(tNow), 
                            $(tBase))
                        )')
                        .TotalMinutes
                        .ToString('0'))]]
    
    #[var v = $(v).$([MSBuild]::Modulo($(revBuild), $([System.Math]::Pow(2, 14))))]
}]

#[var v = $([System.String]::Format("v{0}\r\n\t", $(v)))]
#[File write("#[var log]"):> Example #[var v] Generated by vsSolutionBuildEvent]
#[IO scall("notepad", "#[var log]")]

$(n = $([System.Math]::Exp('$([MSBuild]::Multiply($([System.Math]::Log(2)), 16))')))
$(n)
```

Use our available components or extend everything by creating [**new**](https://vssbe.r-eg.net/doc/Dev/New%20Component/).

## Wiki

[Read or Edit](https://vsce.r-eg.net/)

## 🖼️

![](https://3F.github.io/web.vsCE/doc/Resources/Screenshots/vsCommandEvent_menu.png)

![](https://3F.github.io/web.vsCE/doc/Resources/Screenshots/main_v1.0.png)

![](https://3F.github.io/web.vsCE/doc/Resources/examples/CommandEvent.gif)

![](https://3F.github.io/web.vsCE/doc/Resources/examples/cmds/live.gif)

**[ [ . . .](https://vsce.r-eg.net/Screenshots/) ]**

