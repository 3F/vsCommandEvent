/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace net.r_eg.vsCE
{
    public class JsonSerializationBinder: SerializationBinder, ISerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType($"{typeName}, {assemblyName}");
        }
    }
}
