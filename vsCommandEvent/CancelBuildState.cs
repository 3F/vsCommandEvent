﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE
{
    internal sealed class CancelBuildState
    {
        /// <summary>
        /// Actual state for VS API.
        /// </summary>
        public int CanceledInt => Canceled ? 1 : 0;

        /// <summary>
        /// Actual state.
        /// </summary>
        public bool Canceled { get; private set; }

        public void Cancel() => Canceled = true;

        public void Reset() => Canceled = false;

        public bool UpdateFlagIfCanceled(ref int flag)
        {
            if(Canceled)
            {
                Log.Debug($"Canceled operation due to {nameof(CancelBuildState)} -> {flag}");
                flag = CanceledInt;
            }
            return Canceled;
        }
    }
}
