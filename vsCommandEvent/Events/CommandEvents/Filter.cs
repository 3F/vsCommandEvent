/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

namespace net.r_eg.vsCE.Events.CommandEvents
{
    public class Filter: IFilter
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public object CustomIn { get; set; }

        public object CustomOut { get; set; }

        public bool Cancel { get; set; }

        public bool Pre { get; set; } = true;

        public bool Post { get; set; }

        public string Description { get; set; }

        public bool IgnoreCustomIn { get; set; }

        public bool IgnoreCustomOut { get; set; }
    }
}
