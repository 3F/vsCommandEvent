﻿using net.r_eg.vsSBE.Devenv;
using System.Reflection;
using System.Runtime.InteropServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("Devenv vsSolutionBuildEvent [ github.com/3F/vsSolutionBuildEvent ]")]
[assembly: AssemblyDescription("Add-in")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("vsSolutionBuildEvent Devenv Command-Line")]
[assembly: AssemblyCopyright("Denis Kuzmin - entry.reg@gmail.com")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4CD3D5A0-B661-43C6-B68D-6C010D87D865")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Revision
//      Build Number
//
// You can specify all the value or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

//[assembly: AssemblyVersion("1.0.*")]

// Current format: {Major}.{Minor}.0.{Build} -> 
//     Where 0 it's the reserved number for patches from external assemblies
//     For changes use the .version file only as {Major}.{Minor}
[assembly: AssemblyVersion(Version.S_NUM)]
[assembly: AssemblyFileVersion(Version.S_NUM)]
[assembly: AssemblyInformationalVersion(Version.S_INFO)]

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified - the assembly cannot be signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. 
//   (*) If the key file and a key name attributes are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP - that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the file is installed into the CSP and used.
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]
//[assembly: AssemblyKeyName("")]
