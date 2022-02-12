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
        public string[] _ => new string[]
        {
            " This file for vsCommandEvent ",
            " https://github.com/3F/vsCommandEvent "
        };

        public string Compatibility { get; set; } = "1.0";

    }
}
