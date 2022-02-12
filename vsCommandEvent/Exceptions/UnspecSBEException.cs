/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace net.r_eg.vsCE.Exceptions
{
    [Serializable]
    public class UnspecSBEException: Exception
    {
        public readonly object value;

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(value), value);
        }

        public UnspecSBEException()
        {

        }

        public UnspecSBEException(string message) 
            : base(message)
        {

        }

        public UnspecSBEException(string message, object value)
            : base(message)
        {
            this.value = value;
        }

        public UnspecSBEException(string message, Exception innerException) 
            : base(message, innerException)
        {

        }
    }
}