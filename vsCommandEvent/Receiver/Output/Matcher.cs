﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Text.RegularExpressions;
using net.r_eg.vsCE.Extensions;

namespace net.r_eg.vsCE.Receiver.Output
{
    /// <summary>
    /// TODO: this old matcher from v0.4 or less, need refact for new structure
    /// </summary>
    public class Matcher
    {
        /// <param name="filters">Specific filter for checking.</param>
        /// <param name="raw"></param>
        /// <param name="guid">Guid string of pane</param>
        /// <param name="item">Name of item pane</param>
        /// <returns>matched if one of conditions is true</returns>
        public bool match(Events.OWP.IMatching[] filters, string raw, string guid, string item)
        {
            if(raw == null || filters == null || filters.Length < 1) {
                return false;
            }

            foreach(Events.OWP.IMatching filter in filters)
            {
                if(!checkPane(filter, guid, item)) {
                    continue;
                }

                switch(filter.Type)
                {
                    case Events.OWP.ComparisonType.Default: {
                        if(mDefault(filter.Phrase, ref raw)) {
                            return true;
                        }
                        continue;
                    }
                    case Events.OWP.ComparisonType.Regexp: {
                        if(mRegexp(filter.Phrase, ref raw)) {
                            return true;
                        }
                        continue;
                    }
                    case Events.OWP.ComparisonType.Wildcards: {
                        if(mWildcards(filter.Phrase, ref raw)) {
                            return true;
                        }
                        continue;
                    }
                }
            }

            return false;
        }

        protected bool mRegexp(string pattern, ref string raw)
        {
            try {
                return Regex.Match(raw, pattern/*, RegexOptions.IgnoreCase*/).Success;
            }
            catch(Exception ex) {
                // all incorrect syntax should be simply false
                Log.Warn("OWPMatcher: {0}", ex.Message);
            }
            return false;
        }

        protected bool mWildcards(string pattern, ref string raw)
        {
            //TODO: rapid alternative https://bitbucket.org/3F/sandbox/src/master-C%2B%2B/cpp/text/wildcards/wildcards/versions/essential/AlgorithmEss.h
            //_
            string stub = Regex.Escape(pattern).Replace("\\*", ".*?").Replace("\\+", ".+?").Replace("\\?", ".");
            return mRegexp(stub, ref raw);
        }

        protected bool mDefault(string pattern, ref string raw)
        {
            return raw.Contains(pattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="guid">Guid string of pane</param>
        /// <param name="item">Name of item pane</param>
        /// <returns></returns>
        protected bool checkPane(Events.OWP.IMatching filter, string guid, string item)
        {
            if(guid.CompareGuids(GuidList.OWP_SBE_STRING) || item == Settings.OWP_ITEM_VSSBE) {
                return false; // TODO: it may be as a Logging event from vsSBE later
            }

            if(String.IsNullOrEmpty(filter.PaneGuid) && String.IsNullOrEmpty(filter.PaneName)) {
                return false;
            }

            if(!String.IsNullOrEmpty(filter.PaneGuid) && guid.CompareGuids(filter.PaneGuid)) {
                return true;
            }

            if(!String.IsNullOrEmpty(filter.PaneName) && filter.PaneName == item) {
                return true;
            }

            return false;
        }
    }
}
