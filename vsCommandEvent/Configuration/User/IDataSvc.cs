/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.vsCE.Configuration.User
{
    [Guid("09660649-B7BC-43D3-8E8F-951308B953D5")]
    internal interface IDataSvc
    {
        /// <summary>
        /// Update Common property.
        /// </summary>
        /// <param name="isLoad">Update for loading or saving.</param>
        void updateCommon(bool isLoad);
    }
}
