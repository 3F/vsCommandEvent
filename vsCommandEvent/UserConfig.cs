/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.IO;
using System.Text;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Exceptions;
using IUserData = net.r_eg.vsCE.Configuration.User.IData;
using IUserDataSvc = net.r_eg.vsCE.Configuration.User.IDataSvc;
using UserData = net.r_eg.vsCE.Configuration.User.Data;

namespace net.r_eg.vsCE
{
    internal class UserConfig: PackerAbstract<IUserData, UserData>, IConfig<IUserData>
    {
        /// <summary>
        /// Extension of UserConfig.
        /// </summary>
        internal const string EXT = ".user";

        /// <summary>
        /// When data is updated.
        /// </summary>
        public event EventHandler<DataArgs<IUserData>> Updated = delegate(object sender, DataArgs<IUserData> e) { };

        /// <summary>
        /// User data at runtime.
        /// </summary>
        public IUserData Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads data from user config file.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        /// <param name="prefix">Special version of configuration file.</param>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        public bool load(string path, string prefix)
        {
            Link = getLink(path, Config.Entity.NAME + EXT, prefix);
            return loadByLink(Link);
        }

        /// <summary>
        /// Settings from other object.
        /// </summary>
        /// <param name="data">Object with configuration.</param>
        public void load(IUserData data)
        {
            Data = data;
            Updated(this, new DataArgs<IUserData>() { Data = Data });
        }

        /// <summary>
        /// Use link from other configuration for loading new settings.
        /// </summary>
        /// <param name="link">Link from other configuration.</param>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        public bool load(string link)
        {
            Link = link + EXT;
            return loadByLink(Link);
        }

        /// <summary>
        /// Load settings from file with path by default.
        /// </summary>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        public bool load()
        {
            return load(Settings.CfgManager.Config.Link);
        }

        /// <summary>
        /// Save settings.
        /// </summary>
        public void save()
        {
            if(Link == null) {
                Log.Trace("User Configuration: Ignore saving. Link is null.");
                return;
            }

            ((IUserDataSvc)Data).updateCommon(false);
            try
            {
                Data.updateCache();
                using(TextWriter stream = new StreamWriter(Link, false, Encoding.UTF8)) {
                    serialize(stream, Data);
                }
                InRAM = false;

                Log.Trace("User Configuration: has been updated '{0}'", Link);
                Updated(this, new DataArgs<IUserData>() { Data = Data });
            }
            catch(Exception ex) {
                Log.Debug("Cannot apply user configuration '{0}'", ex.Message);
            }
        }

        /// <summary>
        /// Unload User data.
        /// </summary>
        public void unload()
        {
            Link = null;
            Data = null;
            Updated(this, new DataArgs<IUserData>() { Data = null });
        }

        /// <summary>
        /// Load settings by link to configuration file.
        /// </summary>
        /// <param name="link">Link to configuration file.</param>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        protected virtual bool loadByLink(string link)
        {
            InRAM = false;
            try
            {
                using(StreamReader stream = new StreamReader(link, Encoding.UTF8, true))
                {
                    Data = deserialize(stream);
                    if(Data == null) {
                        throw new UnspecSBEException("file is empty");
                    }
                }
                Log.Trace("User settings: has been loaded from '{0}'", link);
            }
            catch(FileNotFoundException)
            {
                Data    = new UserData();
                InRAM   = true;
                Log.Trace("User settings: Initialized new.");
            }
            catch(Exception ex)
            {
                Log.Debug("User settings is corrupt - '{0}'", ex.Message);
                Data    = new UserData();
                InRAM   = true;
            }

            ((IUserDataSvc)Data).updateCommon(true);
            Configuration.User.Manager.update(Data);

            Updated(this, new DataArgs<IUserData>() { Data = Data });
            return !InRAM;
        }
    }
}
