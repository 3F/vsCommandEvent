/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Configuration
{
    /// <summary>
    /// Configure existing components
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Identification by class name
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Activation status
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }
    }
}
