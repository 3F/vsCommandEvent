/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Collections.Generic;
using net.r_eg.vsCE.Events;

namespace net.r_eg.vsCE.UI.WForms.Logic
{
    public class SBEWrap
    {
        /// <summary>
        /// Wrapped event
        /// </summary>
        public List<ISolutionEvent> evt;

        /// <summary>
        /// Specific type
        /// </summary>
        public SolutionEventType type;

        /// <param name="type"></param>
        public SBEWrap(SolutionEventType type)
        {
            this.type = type;
            update();
        }

        /// <summary>
        /// Updating list from used array data
        /// </summary>
        public void update()
        {
            if(Settings.Cfg.getEvent(type) != null) {
                evt = new List<ISolutionEvent>(Settings.Cfg.getEvent(type));
                return;
            }

            Log.Debug("SBEWrap: evt is null for type '{0}'", type);
            evt = new List<ISolutionEvent>();
        }
    }
}