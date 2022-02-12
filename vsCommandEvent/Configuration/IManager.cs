/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Runtime.InteropServices;
using IUserData = net.r_eg.vsCE.Configuration.User.IData;

namespace net.r_eg.vsCE.Configuration
{
    [Guid("D129EF4C-EEDA-4B8D-A7AE-CCD12A157411")]
    public interface IManager
    {
        /// <summary>
        /// Get Config instance for used context.
        /// </summary>
        IConfig<ISolutionEvents> Config { get; }

        /// <summary>
        /// Get UserConfig instance for used context.
        /// </summary>
        IConfig<IUserData> UserConfig { get; }

        /// <summary>
        /// Current used context.
        /// </summary>
        ContextType Context { get; }

        /// <summary>
        /// Switcher of used context.
        /// </summary>
        /// <param name="context">New context for using.</param>
        /// <returns>Self reference.</returns>
        IManager switchOn(ContextType context);

        /// <summary>
        /// Checks existance of configuration for specific context.
        /// </summary>
        /// <param name="context">Context for checking.</param>
        /// <returns>true value if exists in selected context.</returns>
        bool IsExistCfg(ContextType context);

        /// <summary>
        /// Get Config instance for selected context without an switching.
        /// </summary>
        /// <param name="context">Context for using.</param>
        /// <returns></returns>
        IConfig<ISolutionEvents> getConfigFor(ContextType context);

        /// <summary>
        /// Get UserConfig instance for selected context without an switching.
        /// </summary>
        /// <param name="context">Context for using.</param>
        /// <returns></returns>
        IConfig<IUserData> getUserConfigFor(ContextType context);

        /// <summary>
        /// Add Config instance for specific context.
        /// </summary>
        /// <param name="cfg">Config instance.</param>
        /// <param name="context">Specific context.</param>
        /// <param name="force">Add with replacement if true.</param>
        /// <returns>true value if successfully added.</returns>
        bool add(IConfig<ISolutionEvents> cfg, ContextType context, bool force = false);

        /// <summary>
        /// Add UserConfig instance for specific context.
        /// </summary>
        /// <param name="cfg">UserConfig instance.</param>
        /// <param name="context">Specific context.</param>
        /// <param name="force">Add with replacement if true.</param>
        /// <returns>true value if successfully added.</returns>
        bool add(IConfig<IUserData> cfg, ContextType context, bool force = false);
        
        /// <summary>
        /// Add and use configurations with selected context.
        /// </summary>
        /// <param name="cfg">Config instance.</param>
        /// <param name="userCfg">UserConfig instance.</param>
        /// <param name="context">Context for switching.</param>
        void addAndUse(IConfig<ISolutionEvents> cfg, IConfig<IUserData> userCfg, ContextType context);
        
        /// <summary>
        /// Unset Config instance from selected context.
        /// </summary>
        /// <param name="context">Selected context.</param>
        void unsetConfig(ContextType context);

        /// <summary>
        /// Unset UserConfig instance from selected context.
        /// </summary>
        /// <param name="context">Selected context.</param>
        void unsetUserConfig(ContextType context);

        /// <summary>
        /// Unset all instance from selected context and switch on other.
        /// </summary>
        /// <param name="context">Selected context.</param>
        /// <param name="to">Context for switching.</param>
        void unsetAndUse(ContextType context, ContextType to);
    }
}
