/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Configuration
{
    public class Header
    {
        public string[] _ = new string[]
        {
            "https://github.com/3F/vsCommandEvent"
        };

        /// <summary>
        /// Compatibility of configurations.
        /// </summary>
        public string Compatibility
        {
            get { return compatibility; }
            set { compatibility = value; }
        }
        /// <summary>
        /// The version below used by default if current attr is not found.
        /// </summary>
        private string compatibility = "1.0";

    }
}
