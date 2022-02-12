/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;

namespace net.r_eg.vsCE.Bridge.Exceptions
{
    [Serializable]
    public class InvalidArgumentException: GeneralException
    {
        public InvalidArgumentException(string message)
            : base(message)
        {

        }

        public InvalidArgumentException(string message, params object[] args)
            : base(message, args)
        {

        }
    }
}
