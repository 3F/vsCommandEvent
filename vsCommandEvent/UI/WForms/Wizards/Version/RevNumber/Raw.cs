/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.UI.WForms.Wizards.Version.RevNumber
{
    internal struct Raw: IRevNumber
    {
        /// <summary>
        /// The type of this revision number.
        /// </summary>
        public Type Type
        {
            get { return Type.Raw; }
        }
    }
}
