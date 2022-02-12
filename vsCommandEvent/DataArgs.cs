/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;

namespace net.r_eg.vsCE
{
    [Serializable]
    public class DataArgs<T>: EventArgs
    {
        /// <summary>
        /// Provides a T value to use with events.
        /// </summary>
        public T Data
        {
            get;
            set;
        }
    }
}
