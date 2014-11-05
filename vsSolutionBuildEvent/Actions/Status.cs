﻿/* 
 * Boost Software License - Version 1.0 - August 17th, 2003
 * 
 * Copyright (c) 2013-2014 Developed by reg [Denis Kuzmin] <entry.reg@gmail.com>
 * 
 * Permission is hereby granted, free of charge, to any person or organization
 * obtaining a copy of the software and accompanying documentation covered by
 * this license (the "Software") to use, reproduce, display, distribute,
 * execute, and transmit the Software, and to prepare derivative works of the
 * Software, and to permit third-parties to whom the Software is furnished to
 * do so, all subject to the following:
 * 
 * The copyright notices in the Software and this entire statement, including
 * the above license grant, this restriction and the following disclaimer,
 * must be included in all copies of the Software, in whole or in part, and
 * all derivative works of the Software, unless such copies or derivative
 * works are solely in the form of machine-executable object code generated by
 * a source language processor.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
 * SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
 * FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE. 
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using net.r_eg.vsSBE.Events;

namespace net.r_eg.vsSBE.Actions
{
    public class Status: IStatus
    {
        /// <summary>
        /// Thread-safe getting the instance of Status class
        /// </summary>
        public static IStatus _
        {
            get { return _lazy.Value; }
        }
        private static readonly Lazy<Status> _lazy = new Lazy<Status>(() => new Status());

        /// <summary>
        /// Contains the all execution status by event type
        /// </summary>
        protected ConcurrentDictionary<SolutionEventType, SynchronizedCollection<StatusType>> states;

        /// <summary>
        /// New status for Event type
        /// </summary>
        /// <param name="tevent">Event type</param>
        /// <param name="tstatus">Execution status</param>
        public void add(SolutionEventType tevent, StatusType tstatus)
        {
            if(!states.ContainsKey(tevent)) {
                states[tevent] = new SynchronizedCollection<StatusType>();
            }
            states[tevent].Add(tstatus);
        }

        /// <summary>
        /// Flushing of all execution statuses by Event type
        /// </summary>
        /// <param name="tevent"></param>
        public void flush(SolutionEventType tevent)
        {
            states[tevent] = new SynchronizedCollection<StatusType>();
        }

        /// <summary>
        /// Flushing of all execution statuses
        /// </summary>
        public void flush()
        {
            states = new ConcurrentDictionary<SolutionEventType, SynchronizedCollection<StatusType>>();
        }

        /// <summary>
        /// Updating status for Event type
        /// </summary>
        /// <param name="tevent">Event type</param>
        /// <param name="index">Position in list</param>
        /// <param name="tstatus">new status</param>
        public void update(SolutionEventType tevent, int index, StatusType tstatus)
        {
            try {
                states[tevent][index] = tstatus;
            }
            catch(Exception ex) {
                Log.nlog.Debug("Updating status: '{0}'", ex.Message);
            }
        }

        /// <summary>
        /// Getting the Execution status by Event type and position in list
        /// </summary>
        /// <param name="tevent">Event type</param>
        /// <param name="index">Position in list</param>
        /// <returns>Executed status</returns>
        public StatusType get(SolutionEventType tevent, int index)
        {
            Debug.Assert(states != null);
            try {
                return states[tevent][index];
            }
            catch(Exception) {
                return StatusType.NotFound;
            }
        }

        /// <summary>
        /// Getting the Execution statuses by Event type
        /// </summary>
        /// <param name="tevent">Event type</param>
        /// <returns>List of Executed statuses</returns>
        public SynchronizedCollection<StatusType> get(SolutionEventType tevent)
        {
            return states[tevent];
        }

        private Status()
        {
            flush();
        }
    }
}
