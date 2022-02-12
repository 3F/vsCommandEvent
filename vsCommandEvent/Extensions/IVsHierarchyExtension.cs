/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using DProject = EnvDTE.Project;

namespace net.r_eg.vsCE.Extensions
{
    public static class IVsHierarchyExtension
    {
        public static Guid GetProjectGuid(this IVsHierarchy pHierProj)
        {
            if(pHierProj == null) {
                return Guid.Empty;
            }

#if VSSDK_15_AND_NEW
            ThreadHelper.ThrowIfNotOnUIThread();
#endif

            pHierProj.GetGuidProperty(
                (uint)VSConstants.VSITEMID.Root, 
                (int)__VSHPROPID.VSHPROPID_ProjectIDGuid, 
                out Guid id
            );

            return id;
        }

        public static DProject GetEnvDteProject(this IVsHierarchy pHierProj)
        {
            if(pHierProj == null) {
                return null;
            }

#if VSSDK_15_AND_NEW
            ThreadHelper.ThrowIfNotOnUIThread();
#endif

            pHierProj.GetProperty(
                (uint)VSConstants.VSITEMID.Root, 
                (int)__VSHPROPID.VSHPROPID_ExtObject, 
                out object dteProject
            );

            return (DProject)dteProject;
        }

        public static IVsHierarchy GetIVsHierarchy(this DProject dProject)
        {
            if(dProject == null) {
                return null;
            }

#if VSSDK_15_AND_NEW
            ThreadHelper.ThrowIfNotOnUIThread();
#endif

            IVsSolution sln = (IVsSolution)Package.GetGlobalService(typeof(SVsSolution));
            sln.GetProjectOfUniqueName(dProject.FullName, out IVsHierarchy hr);

            return hr;
        }
    }
}
