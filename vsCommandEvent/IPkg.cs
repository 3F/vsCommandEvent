/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Threading;
using Microsoft.VisualStudio.Shell;

#if VSSDK_15_AND_NEW
using System.Threading.Tasks;
#endif

namespace net.r_eg.vsCE
{
    internal interface IPkg
    {
        CancellationToken CancellationToken { get; }

#if VSSDK_15_AND_NEW

        /// <summary>
        /// Finds or creates tool window.
        /// </summary>
        /// <param name="type">tool window type</param>
        /// <param name="create">try to create tool when true</param>
        /// <param name="id">tool window id</param>
        /// <returns></returns>
        Task<ToolWindowPane> getToolWindowAsync(Type type, bool create = true, int id = 0);

        /// <param name="type">service type.</param>
        /// <returns></returns>
        Task<object> getSvcAsync(Type type);

#else

        /// <summary>
        /// Finds or creates tool window.
        /// </summary>
        /// <param name="type">tool window type</param>
        /// <param name="create">try to create tool when true</param>
        /// <param name="id">tool window id</param>
        /// <returns></returns>
        ToolWindowPane getToolWindow(Type type, bool create = true, int id = 0);

        /// <param name="type">service type.</param>
        /// <returns></returns>
        object getSvc(Type type);

#endif

        VSTools.ErrorList.IPane ErrorList { get; }
    }
}
