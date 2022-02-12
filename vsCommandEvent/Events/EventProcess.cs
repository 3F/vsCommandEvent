/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.MvsSln.Extensions;

namespace net.r_eg.vsCE.Events
{
    public class EventProcess: IEventProcess
    {
        /// <inheritdoc cref="IEventProcess.Waiting"/>
        public bool Waiting { get; set; } = true;

        /// <inheritdoc cref="IEventProcess.Hidden"/>
        public bool Hidden { get; set; } = true;

        /// <inheritdoc cref="IEventProcess.TimeLimit"/>
        public int TimeLimit { get; set; } = 30;

        public static bool operator ==(EventProcess a, EventProcess b) => a.Equals(b);

        public static bool operator !=(EventProcess a, EventProcess b) => !(a == b);

        public override bool Equals(object obj)
        {
            if(obj is null || !(obj is EventProcess)) {
                return false;
            }

            var b = (EventProcess)obj;

            return Waiting == b.Waiting
                    && Hidden == b.Hidden
                    && TimeLimit == b.TimeLimit;
        }

        public override int GetHashCode()
        {
            return 0.CalculateHashCode
            (
                Waiting,
                Hidden,
                TimeLimit
            );
        }
    }
}
