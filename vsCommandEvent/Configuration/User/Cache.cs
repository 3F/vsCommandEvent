/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Configuration.User
{
    public class Cache: ICacheHeader
    {
        /// <summary>
        /// When has been updated.
        /// UTC as a rule.
        /// </summary>
        public long Updated
        {
            get;
            set;
        }

        /// <summary>
        /// Type of hashing.
        /// </summary>
        public HashType Algorithm
        {
            get;
            set;
        }

        /// <summary>
        /// Hash value of data.
        /// </summary>
        public string Hash
        {
            get;
            set;
        }
    }
}
