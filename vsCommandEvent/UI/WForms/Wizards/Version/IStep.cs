﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.UI.WForms.Wizards.Version
{
    internal interface IStep
    {
        /// <summary>
        /// The type of step.
        /// </summary>
        StepsType Type { get; }
    }
}
