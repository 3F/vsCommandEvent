/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.CodeDom.Compiler;

namespace net.r_eg.vsCE.Exceptions
{
    [Serializable]
    public class CompilerException: UnspecSBEException
    {
        public CompilerException(CompilerError error)
            : base(error.ToString(), error)
        {

        }

        public CompilerException(string message)
            : base(message)
        {

        }
    }
}
