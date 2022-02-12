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
    public class GeneralException: NotSupportedException
    {
        public GeneralException()
        {

        }

        public GeneralException(string message)
            : base(message)
        {

        }

        public GeneralException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public GeneralException(string message, params object[] args)
            : base(format(ref message, args))
        {

        }

        public GeneralException(string message, Exception innerException, params object[] args)
            : base(format(ref message, args), innerException)
        {

        }

        protected static string format(ref string message, params object[] args)
        {
            return String.Format(message, args);
        }
    }
}
