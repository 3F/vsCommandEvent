/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;
using net.r_eg.vsCE.Events.Commands;

namespace net.r_eg.vsCE.Events
{
    /// <summary>
    /// Scripts from available engines like MSBuild, etc.
    /// </summary>
    [Guid("4F1765E1-CED5-4AA9-B229-617EE7B0B09D")]
    public interface IModeScript: ICommand
    {

    }
}
