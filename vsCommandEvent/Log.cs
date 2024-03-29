﻿/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using net.r_eg.vsCE.Logger;
using net.r_eg.vsCE.VSTools.OW;
using NLog;

namespace net.r_eg.vsCE
{
    /// <summary>
    /// Main logger for Package
    /// </summary>
    internal class Log: ILog
    {
        /// <summary>
        /// When message has been received.
        /// </summary>
        public event EventHandler<MessageArgs> Received = delegate(object sender, MessageArgs e) { };

        /// <summary>
        /// DTE context
        /// </summary>
        protected EnvDTE.DTE dte;

        /// <summary>
        /// To displaying messages.
        /// </summary>
        protected IPane upane;

        /// <summary>
        /// Undelivered messages.
        /// </summary>
        protected Queue<string> undelivered = new Queue<string>();

        /// <summary>
        /// Size of buffer for undelivered messages.
        /// </summary>
        protected int undBuffer = 2048;

        /// <summary>
        /// Get instance of the NLog logger
        /// </summary>
        public NLog.Logger NLog
        {
            get {
                return nlog;
            }
        }
        protected NLog.Logger nlog = LogManager.GetLogger(GuidList.PACKAGE_LOGGER);

        /// <summary>
        /// Thread-safe getting the instance of Log class
        /// </summary>
        public static ILog _
        {
            get { return _lazy.Value; }
        }
        private static readonly Lazy<Log> _lazy = new Lazy<Log>(() => new Log());

        /// <summary>
        /// Initialization of the IVsOutputWindowPane
        /// note: probably slow, 
        ///       and be careful with using inside `Initialize()` method or constructor of main package, 
        ///       may be inner exception for COM object in VS (tested on VS2013 with docked to output panel)
        ///       Otherwise, use the IVsUIShell.FindToolWindow (again, only with __VSFINDTOOLWIN.FTW_fFindFirst)
        /// </summary>
        /// <param name="name">Name of the pane</param>
        /// <param name="ow"></param>
        /// <param name="dteContext"></param>
        public void paneAttach(string name, IVsOutputWindow ow, EnvDTE.DTE dteContext)
        {
            dte = dteContext;

            if(upane != null) {
                Log.Debug("paneAttach-COM: pane is already attached.");
                return;
            }
            upane = new PaneCOM(ow, name);
        }

        /// <summary>
        /// Direct access from existing instance
        /// </summary>
        /// <param name="owp"></param>
        public void paneAttach(IVsOutputWindowPane owp)
        {
            if(upane != null) {
                Log.Debug("paneAttach-direct: pane is already attached.");
                return;
            }
            upane = new PaneCOM(owp);
        }

        /// <summary>
        /// Attach pane with EnvDTE.OutputWindowPane
        /// </summary>
        /// <param name="name">Name of the pane</param>
        /// <param name="dte2"></param>
        public void paneAttach(string name, EnvDTE80.DTE2 dte2)
        {
#if SDK15_OR_HIGH
            ThreadHelper.ThrowIfNotOnUIThread();
#endif
            dte = (EnvDTE.DTE)dte2;
            if(upane != null) {
                Log.Debug("paneAttach-DTE: pane is already attached.");
                return;
            }
            upane = new PaneDTE(dte2, name);
        }

        /// <summary>
        /// Detaching OWP by IVsOutputWindow.
        /// </summary>
        /// <param name="ow"></param>
        public void paneDetach(IVsOutputWindow ow)
        {
            Guid id = (upane != null)? upane.Guid : GuidList.OWP_SBE;
            paneDetach();

#if SDK15_OR_HIGH
            ThreadHelper.ThrowIfNotOnUIThread(); //TODO: upgrade to 15
#endif

            if(ow != null) {
                ow.DeletePane(ref id);
            }
        }

        /// <summary>
        /// Detaching OWP.
        /// </summary>
        public void paneDetach()
        {
            upane   = null;
            dte     = null;
        }

        /// <summary>
        /// Writes raw message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>self reference</returns>
        public ILog raw(string message)
        {
            write(message);
            return this;
        }

        /// <summary>
        /// Writes raw message + line terminator.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>self reference</returns>
        public ILog rawLn(string message)
        {
            return raw(message + System.Environment.NewLine);
        }

        /// <summary>
        /// Show messages if it's possible.
        /// </summary>
        public void show()
        {
            try
            {
#if SDK15_OR_HIGH
                ThreadHelper.ThrowIfNotOnUIThread();
#endif
                if(dte != null)
                {
                    dte.ExecuteCommand("View.Output"); //TODO:
                }

                if(upane != null) {
                    upane.Activate();
                }
            }
            catch(Exception ex) {
                Log.Debug("Log: error of showing '{0}'", ex.Message);
            }
        }

        /// <summary>
        /// To clear all available messages if it's possible.
        /// </summary>
        /// <param name="force">Including undelivered etc.</param>
        public void clear(bool force)
        {
            if(force) {
                undelivered.Clear();
            }

            if(upane != null) {
                upane.Clear();
            }
        }

        /// <summary>
        /// Checks specific level on error type.
        /// </summary>
        /// <param name="level"></param>
        public bool isError(string level)
        {
            if(String.IsNullOrWhiteSpace(level)) {
                return false;
            }

            // TODO:
            Func<LogLevel, bool> _is = delegate(LogLevel t) {
                return level.Equals($"{t}", StringComparison.OrdinalIgnoreCase);
            };

            return _is(LogLevel.Error) || _is(LogLevel.Fatal);
        }

        /// <summary>
        /// Checks specific level on warning type.
        /// </summary>
        /// <param name="level"></param>
        public bool isWarn(string level)
        {
            if(String.IsNullOrWhiteSpace(level)) {
                return false;
            }

            // TODO: 
            return level.Equals($"{LogLevel.Warn}", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Entry point for NLog messages.
        /// https://github.com/nlog/nlog/wiki/MethodCall-target
        /// </summary>
        public static void nprint(string level, string message, string stamp)
        {
            LogLevel oLevel = LogLevel.FromString(level);

//#if !DEBUG
            if(oLevel < LogLevel.Info && !Settings._.DebugMode) {
                return;
            }
//#endif

            var log = _lazy.Value;
            log.write(log.format(level, message, stamp), level);
        }

        /// <summary>
        /// Writes message at the Trace level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Trace(string message, params object[] args)
        {
            _.NLog.Trace(message, args);
        }

        /// <summary>
        /// Writes message at the Debug level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Debug(string message, params object[] args)
        {
            _.NLog.Debug(message, args);
        }

        /// <summary>
        /// Writes message at the Warn level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Warn(string message, params object[] args)
        {
            _.NLog.Warn(message, args);
        }

        /// <summary>
        /// Writes message at the Info level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Info(string message, params object[] args)
        {
            _.NLog.Info(message, args);
        }

        /// <summary>
        /// Writes message at the Error level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error(string message, params object[] args)
        {
            _.NLog.Error(message, args);
        }

        /// <summary>
        /// Writes message at the Fatal level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Fatal(string message, params object[] args)
        {
            _.NLog.Fatal(message, args);
        }

        /// <summary>
        /// Used format for messages.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="stamp"></param>
        /// <returns>formatted</returns>
        protected virtual string format(string level, string message, string stamp)
        {
            DateTime dt = new DateTime(long.Parse(stamp));
            return String.Format("{0} [{1}]: {2}{3}",
                                dt.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern + ".ffff"),
                                level,
                                message,
                                System.Environment.NewLine);
        }

        /// <summary>
        /// Checks status of ignoring level.
        /// </summary>
        /// <param name="level">Level for cheking.</param>
        /// <returns></returns>
        protected bool ignoreLevel(string level)
        {
            if(String.IsNullOrEmpty(level)) {
                return false;
            }

            var cfg = Settings.CfgManager.UserConfig;

            if(cfg == null || cfg.Data == null || !cfg.Data.Global.LogIgnoreLevels.ContainsKey(level)) {
                return false;
            }
            return cfg.Data.Global.LogIgnoreLevels[level];
        }

        /// <summary>
        /// Where to write.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        protected virtual void write(string message, string level = null)
        {
            if(ignoreLevel(level)) {
                return;
            }

            //if(Thread.CurrentThread.Name != LoggingEvent.IDENT_TH) {
                Received(this, new MessageArgs() { Message =  message,  Level = (level)?? String.Empty });
            //}

            try {
                if(deliver(message)) {
                    return;
                }
            }
            catch(COMException ex) {
                message = String.Format("Log - COMException '{0}' :: Message - '{1}'", ex.Message, message);
            }

            conwrite(message, level);

#if DEBUG
            System.Diagnostics.Debug.Write(message);
#endif
        }

        protected virtual void conwrite(string message, string level)
        {
            if(Log._.isError(level)) {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(Log._.isWarn(level)) {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.Write(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Use OWP for write operation.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>true value if message has been sent.</returns>
        protected bool owpSend(string message)
        {
            if(upane != null) {
                upane.OutputString(message);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delivering message.
        /// Including the all undelivered before.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>true value if message has been delivered.</returns>
        protected bool deliver(string message)
        {
            if(upane == null) {
                holdMessage(message);
                return false;
            }
            
            while(undelivered.Count > 0) {
                owpSend(undelivered.Dequeue());
            }

            owpSend(message);
            return true;
        }

        /// <summary>
        /// Hold undelivered message.
        /// </summary>
        /// <param name="msg"></param>
        protected void holdMessage(string msg)
        {
            if(undelivered.Count > undBuffer) {
                undelivered.Dequeue();
            }
            undelivered.Enqueue(msg);
        }

        protected Log() { }
    }
}
