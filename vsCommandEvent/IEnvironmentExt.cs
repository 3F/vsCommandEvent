/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using Microsoft.VisualStudio.Shell.Interop;

namespace net.r_eg.vsCE
{
    public interface IEnvironmentExt
    {
        /// <summary>
        /// Gets project name from IVsHierarchy.
        /// </summary>
        /// <param name="pHierProj"></param>
        /// <param name="force">Load in global collection with __VSHPROPID.VSHPROPID_ExtObject if true.</param>
        /// <returns></returns>
        string getProjectNameFrom(IVsHierarchy pHierProj, bool force);
    }
}
