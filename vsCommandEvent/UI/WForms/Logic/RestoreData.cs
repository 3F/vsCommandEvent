/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Collections.Generic;
using net.r_eg.vsCE.Configuration;
using IUserData = net.r_eg.vsCE.Configuration.User.IData;
using UserData = net.r_eg.vsCE.Configuration.User.Data;

namespace net.r_eg.vsCE.UI.WForms.Logic
{
    /// <summary>
    /// Logic for restoring settings.
    /// </summary>
    public sealed class RestoreData
    {
        /// <summary>
        /// All instances of Config.
        /// </summary>
        private Dictionary<ContextType, ISolutionEvents> configs = new Dictionary<ContextType, ISolutionEvents>();

        /// <summary>
        /// All instances of UserConfig.
        /// </summary>
        private Dictionary<ContextType, IUserData> userConfigs = new Dictionary<ContextType, IUserData>();

        /// <summary>
        /// Get Configuration data for selected context.
        /// </summary>
        /// <param name="context">Context for using.</param>
        /// <returns></returns>
        public ISolutionEvents getConfig(ContextType context)
        {
            if(configs.ContainsKey(context)) {
                return configs[context];
            }
            return new SolutionEvents();
        }

        /// <summary>
        /// Get User-Configuration data for selected context.
        /// </summary>
        /// <param name="context">Context for using.</param>
        /// <returns></returns>
        public IUserData getUserConfig(ContextType context)
        {
            if(userConfigs.ContainsKey(context)) {
                return userConfigs[context];
            }
            return new UserData();
        }

        /// <summary>
        /// Update Configuration data for specific context.
        /// </summary>
        /// <param name="data">Configuration data.</param>
        /// <param name="context">Specific context.</param>
        public void update(ISolutionEvents data, ContextType context)
        {
            configs[context] = data;
        }

        /// <summary>
        /// Update User-Configuration data for specific context.
        /// </summary>
        /// <param name="data">Configuration data.</param>
        /// <param name="context">Specific context.</param>
        public void update(IUserData data, ContextType context)
        {
            userConfigs[context] = data;
        }
    }
}