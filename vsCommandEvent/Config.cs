/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.IO;
using System.Text;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Exceptions;

namespace net.r_eg.vsCE
{
    internal class Config: PackerAbstract<ISolutionEvents, SolutionEvents>, IConfig<ISolutionEvents>
    {
        /// <summary>
        /// When data is updated.
        /// </summary>
        public event EventHandler<DataArgs<ISolutionEvents>> Updated = delegate(object sender, DataArgs<ISolutionEvents> e) { };

        /// <summary>
        /// Entity of configuration data.
        /// </summary>
        public struct Entity
        {
            /// <summary>
            /// Config version.
            /// Version of app managed by Package!
            /// </summary>
            public static readonly System.Version Version = new System.Version(1, 0);

            /// <summary>
            /// To file system
            /// </summary>
            public const string NAME = ".vsce";
        }

        /// <summary>
        /// SBE data at runtime.
        /// </summary>
        public ISolutionEvents Data
        {
            get;
            protected set;
        }

        /// <summary>
        /// Loads our data from file.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        /// <param name="prefix">Special version of configuration file.</param>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        public bool load(string path, string prefix)
        {
            Settings._.setWorkPath(path);
            Link = getLink(path, Entity.NAME, prefix);
            return loadByLink(Link);
        }

        /// <summary>
        /// Settings from other object.
        /// </summary>
        /// <param name="data">Object with configuration.</param>
        public void load(ISolutionEvents data)
        {
            Data = data;
            Updated(this, new DataArgs<ISolutionEvents>() { Data = Data });
        }

        /// <summary>
        /// Use link from other configuration for loading new settings.
        /// </summary>
        /// <param name="link">Link from other configuration.</param>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        public bool load(string link)
        {
            Link = link.DirectoryPathFormat();
            return load(Link, null);
        }

        /// <summary>
        /// Load settings from file with path by default.
        /// </summary>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        public bool load()
        {
            return load(Settings._.CommonPath, null);
        }

        /// <summary>
        /// Save settings.
        /// </summary>
        public void save()
        {
            if(Link == null) {
                Log.Trace("Configuration: Ignore saving. Link is null.");
                return;
            }

            try
            {
                using(TextWriter stream = new StreamWriter(Link, false, Encoding.UTF8)) {
                    serialize(stream, Data);
                }
                InRAM = false;

                Log.Trace("Configuration: has been saved in '{0}'", Settings.WPath);
                Updated(this, new DataArgs<ISolutionEvents>() { Data = Data });
            }
            catch(Exception ex) {
                Log.Error("Cannot apply configuration '{0}'", ex.Message);
            }
        }

        /// <summary>
        /// Unload User data.
        /// </summary>
        public void unload()
        {
            Link = null;
            Data = null;
            Updated(this, new DataArgs<ISolutionEvents>() { Data = null });
        }

        /// <summary>
        /// Load settings by link to configuration file.
        /// </summary>
        /// <param name="link">Link to configuration file.</param>
        /// <returns>true value if loaded from existing file, otherwise loaded as new.</returns>
        protected virtual bool loadByLink(string link)
        {
            InRAM = false;
            Log.Debug("Configuration: trying to load - '{0}'", link);
            try
            {
                using(StreamReader stream = new StreamReader(link, Encoding.UTF8, true))
                {
                    Data = deserialize(stream);
                    if(Data == null) {
                        throw new UnspecSBEException("file is empty");
                    }
                    compatibility(stream);
                }
                Log.Info("Loaded settings (v{0}): '{1}'", Data.Header.Compatibility, Settings.WPath);
            }
            catch(FileNotFoundException)
            {
                Data    = new SolutionEvents();
                InRAM   = true;
                Log.Info("Initialized with new settings.");
            }
            catch(Exception ex)
            {
                Log.Error("Configuration file is corrupt - '{0}'", ex.Message);
                Data    = new SolutionEvents(); //TODO: actions in UI, e.g.: restore, new..
                InRAM   = true;
            }

            // Now we work with latest version
            Data.Header.Compatibility = Entity.Version.ToString();
            Updated(this, new DataArgs<ISolutionEvents>() { Data = Data });

            return !InRAM;
        }

        /// <summary>
        /// Checks version and reorganizes structure if needed..
        /// </summary>
        /// <param name="stream"></param>
        private void compatibility(StreamReader stream)
        {
            System.Version cfg = System.Version.Parse(Data.Header.Compatibility);

            if(cfg.Major > Entity.Version.Major || (cfg.Major == Entity.Version.Major && cfg.Minor > Entity.Version.Minor)) {
                Log.Warn(
                    "Version {0} of configuration file is higher supported version {1}. Please update application. Several settings may be not correctly loaded.",
                    cfg.ToString(2), Entity.Version.ToString(2)
                );
            }
        }
    }
}
