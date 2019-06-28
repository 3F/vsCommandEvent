﻿/*
 * Copyright (c) 2013-2016,2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Threading;
using Microsoft.VisualStudio.Shell;

#if !VSSDK_15_AND_NEW
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
#endif

namespace net.r_eg.vsSBE.VSTools.ErrorList
{
    public class Pane: IPane, IDisposable
    {
        protected ErrorListProvider provider;

        protected CancellationToken cancellationToken;

        /// <summary>
        /// To add new error in ErrorList.
        /// </summary>
        /// <param name="message"></param>
        public void error(string message)
        {
            task(message, TaskErrorCategory.Error);
        }

        /// <summary>
        /// To add new warning in ErrorList.
        /// </summary>
        /// <param name="message"></param>
        public void warn(string message)
        {
            task(message, TaskErrorCategory.Warning);
        }

        /// <summary>
        /// To add new information in ErrorList.
        /// </summary>
        /// <param name="message"></param>
        public void info(string message)
        {
            task(message, TaskErrorCategory.Message);
        }

        /// <summary>
        /// To clear all messages.
        /// </summary>
        public void clear()
        {
            provider.Tasks.Clear();
        }

        public Pane(IServiceProvider sp, CancellationToken ct)
            : this(sp)
        {
            cancellationToken = ct;
        }

        public Pane(IServiceProvider sp)
        {
            provider = new ErrorListProvider(sp);
        }

        protected void task(string msg, TaskErrorCategory type = TaskErrorCategory.Message)
        {
            // prevents possible bug from `Process.ErrorDataReceived` because of NLog

#if VSSDK_15_AND_NEW
            ThreadHelper.JoinableTaskFactory.RunAsync(async () => 
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
#else
            Task.Factory.StartNew(() =>
            {
#endif
                provider.Tasks.Add(new ErrorTask() {
                    Text = msg,
                    Document = Settings.APP_NAME_SHORT,
                    Category = TaskCategory.User,
                    Checked = true,
                    IsCheckedEditable = true,
                    ErrorCategory = type,
                });

#if VSSDK_15_AND_NEW
            });
#else
            }, 
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.Default);
#endif
        }

#region IDisposable

        // To detect redundant calls
        private bool disposed = false;

        // To correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;
            //...

            if(provider != null) {
                provider.Dispose();
            }
        }

#endregion
    }
}
