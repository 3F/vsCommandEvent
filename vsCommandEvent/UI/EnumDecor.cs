/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.UI
{
    public static class EnumDecor
    {
        private const string VS_CONST = "Microsoft.VisualStudio.VSConstants";

        public static string Shorten(string input)
        {
            if(input == null) return null;

            if(input.StartsWith(VS_CONST))
            {
                return input.Substring(VS_CONST.Length);
            }

            return input;
        }
    }
}
