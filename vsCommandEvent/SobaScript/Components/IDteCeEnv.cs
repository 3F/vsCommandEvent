/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.SobaScript.Z.VS.Dte;

namespace net.r_eg.vsCE.SobaScript.Components
{
    public interface IDteCeEnv: IDteEnv
    {
        /// <summary>
        /// Raise Command.
        /// </summary>
        /// <param name="guid">Scope by Guid.</param>
        /// <param name="id">The command ID.</param>
        /// <param name="customIn">Custom input parameters.</param>
        /// <param name="customOut">Custom output parameters.</param>
        void Raise(string guid, int id, object customIn, object customOut);
    }
}