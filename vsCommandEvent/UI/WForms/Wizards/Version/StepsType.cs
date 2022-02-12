/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.UI.WForms.Wizards.Version
{
    internal enum StepsType
    {
        /// <summary>
        /// To select type of generation.
        /// </summary>
        Gen,

        /// <summary>
        /// To configure struct or class.
        /// </summary>
        Struct,

        /// <summary>
        /// To configure data of struct or class.
        /// </summary>
        CfgData,

        /// <summary>
        /// To configure the direct replacement.
        /// </summary>
        DirectRepl,

        /// <summary>
        /// To reconfigure of available fields.
        /// </summary>
        Fields,

        /// <summary>
        /// Final step with result.
        /// </summary>
        Final,
    }
}
