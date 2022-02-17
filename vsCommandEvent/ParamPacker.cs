/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Text;
using net.r_eg.SobaScript;

namespace net.r_eg.vsCE
{
    internal static class ParamPacker
    {
        /// <param name="input">Paired key=value values</param>
        /// <returns>{"Key1":123},{"Key2":"val2"},{"Key3":true}</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static string Pack(params object[] input)
        {
            if(input == null || (input.Length & 1) == 1)
            {
                throw new ArgumentOutOfRangeException(nameof(input));
            }

            StringBuilder sb = new();
            for(int i = 0, n = input.Length - 1; true; ++i)
            {
                sb.Append($"{{\"{input[i]}\":");

                    object v = input[++i];
                    if(v is string str)
                    {
                        sb.Append($"\"{str}\"");
                    }
                    else if(v is bool flag)
                    {
                        sb.Append(Value.From(flag));
                    }
                    else
                    {
                        sb.Append(Value.From(v));
                    }

                sb.Append('}');

                if(i < n)
                {
                    sb.Append(',');
                }
                else break;
            }
            return sb.ToString();
        }

        internal static string PackRect(int left, int top, int width, int height)
            => $"{left}|{top}|{width}|{height}";
    }
}
