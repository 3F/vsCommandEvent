/*
 * Copyright (c) 2013-2016  Denis Kuzmin (reg) <entry.reg@gmail.com>
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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using net.r_eg.vsCE.Configuration;

namespace net.r_eg.vsCE
{
    // Managed Package Registration
    [PackageRegistration(UseManagedResourcesOnly = true)]

    // To register the informations needed to in the Help/About dialog of Visual Studio
    [InstalledProductRegistration("#110", "#112", Version.numberWithRevString, IconResourceID = 400)]

    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]

    //  To be automatically loaded when a specified UI context is active
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]

    // Package Guid
    [Guid(GuidList.PACKAGE_STRING)]

    public sealed class vsCommandEventPackage: Package, IDisposable, IVsSolutionEvents
    {
        /// <summary>
        /// For IVsSolutionEvents events
        /// http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivssolution.aspx
        /// </summary>
        private IVsSolution spSolution;

        /// <summary>
        /// Contains the cookie for advising IVsSolution
        /// http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivssolution.advisesolutionevents.aspx
        /// </summary>
        private uint _pdwCookieSolution;

        /// <summary>
        /// For work with ErrorList pane of Visual Studio.
        /// </summary>
        private VSTools.ErrorList.IPane errorList;

        /// <summary>
        /// Listener of the OutputWindowsPane
        /// </summary>
        private Receiver.Output.OWP owpListener;

        /// <summary>
        /// The command for menu - Tools / vsCommandEvent
        /// </summary>
        private MenuCommand _menuItemMain;
        
        /// <summary>
        /// main form of settings
        /// </summary>
        private UI.WForms.EventsFrm configFrm;

        /// <summary>
        /// DTE2 Context
        /// </summary>
        public DTE2 Dte2
        {
            get {
                return (DTE2)Package.GetGlobalService(typeof(SDTE));
            }
        }

        /// <summary>
        /// Binder of main events
        /// </summary>
        public IBinder PackageBinder
        {
            get;
            private set;
        }

        /// <summary>
        /// Priority call with SVsSolution.
        /// Part of IVsSolutionEvents - that the solution has been opened (Before initializing projects).
        /// http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivssolutionevents.onafteropensolution.aspx
        /// </summary>
        /// <param name="pUnkReserved"></param>
        /// <param name="fNewSolution"></param>
        /// <returns></returns>
        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            try {
                return PackageBinder.solutionOpened(pUnkReserved, fNewSolution);
            }
            catch(Exception ex) {
                Log.Error("Problem with loading solution: '{0}'", ex.Message);
            }
            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// Priority call with SVsSolution.
        /// Part of IVsSolutionEvents - that a solution has been closed.
        /// http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivssolutionevents.onafterclosesolution.aspx
        /// </summary>
        /// <param name="pUnkReserved"></param>
        /// <returns></returns>
        public int OnAfterCloseSolution(object pUnkReserved)
        {
            try {
                return PackageBinder.solutionClosed(pUnkReserved);
            }
            catch(Exception ex) {
                Log.Error("Problem with closing solution: '{0}'", ex.Message);
            }
            return VSConstants.S_FALSE;
        }

        private void initAppEvents()
        {
            (new Logger.Initializer()).configure();
            ((EnvDTE80.Events2)Dte2.Events).get_WindowVisibilityEvents().WindowShowing -= attachToOutputWindow;
            ((EnvDTE80.Events2)Dte2.Events).get_WindowVisibilityEvents().WindowShowing += attachToOutputWindow;

            loadData();
            PackageBinder = new Binder(Dte2);

            _menuItemMain.Visible = true;

            owpListener = new Receiver.Output.OWP(PackageBinder.Environment);
            owpListener.attachEvents();
            owpListener.Receiving += (object sender, Receiver.Output.PaneArgs e) => {
                ((Bridge.IEvent)PackageBinder).onBuildRaw(e.Raw, e.Guid, e.Item);
            };
        }

        /// <summary>
        /// Loads data with common configuration.
        /// </summary>
        private void loadData()
        {
            try
            {
                var config      = new Config();
                var userConfig  = new UserConfig();
                Settings.CfgManager.addAndUse(config, userConfig, ContextType.Common);
                
                config.load();
                Log.Trace("Config Link: '{0}'", config.Link);

                userConfig.load();
                Log.Trace("UserConfig Link: '{0}'", userConfig.Link);
            }
            catch(Exception ex) {
                Log.Fatal("Can't load data with common configuration: '{0}'", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Safe variant for attaching pane in NoSolution context.
        /// </summary>
        /// <param name="wnd"></param>
        private void attachToOutputWindow(Window wnd)
        {
            if(wnd == null) {
                Log.Warn("attachToOutputWindow: wnd is null");
                return;
            }

            try {
                if(wnd.ObjectKind == EnvDTE.Constants.vsWindowKindOutput || wnd.Type == vsWindowType.vsWindowTypeOutput) {
                    Log._.paneAttach(GetOutputPane(GuidList.OWP_SBE, Settings.OWP_ITEM_VSSBE));
                    Log.Trace("Pane has been attached: '{0}'", GuidList.OWP_SBE);
                    ((Events2)Dte2.Events).get_WindowVisibilityEvents().WindowShowing -= attachToOutputWindow;
                }
            }
            catch(Exception ex) {
                Log.Error("Failed attachToOutputWindow: `{0}`", ex.Message);
            }
        }

        /// <summary>
        /// Handler of showing the main window if clicked # Build / Command Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _menuMainCallback(object sender, EventArgs e)
        {
            try
            {
                if(UI.Util.focusForm(configFrm)) {
                    return;
                }
                configFrm = new UI.WForms.EventsFrm(PackageBinder);
                configFrm.Show();
            }
            catch(Exception ex) {
                Log.Error("Failed UI: `{0}`", ex.Message);
            }
        }

        private void _menuCfgUnwarnCallback(object sender, EventArgs e)
        {
            if(errorList != null) {
                errorList.clear();
            }
        }

        private void onLogReceived(object sender, Logger.MessageArgs e)
        {
            if(Log._.isError(e.Level)) {
                errorList.error(e.Message);
            }
            else if(Log._.isWarn(e.Level)) {
                errorList.warn(e.Message);
            }
        }

        #region unused

        int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region maintenance

        protected override void Initialize()
        {
            Trace.WriteLine(String.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", ToString()));
            base.Initialize();

            try
            {
                errorList       = new VSTools.ErrorList.Pane(this);
                Log._.Received  -= onLogReceived;
                Log._.Received  += onLogReceived;

                OleMenuCommandService mcs = (OleMenuCommandService)GetService(typeof(IMenuCommandService));

                // Top Menu
                _menuItemMain = new MenuCommand(_menuMainCallback, new CommandID(GuidList.CMD_MAIN, PkgCmdIDList.CMD_MAIN));
                _menuItemMain.Visible = false;
                mcs.AddCommand(_menuItemMain);

                mcs.AddCommand(
                    new MenuCommand(
                        _menuCfgUnwarnCallback, 
                        new CommandID(GuidList.CMD_MAIN, PkgCmdIDList.CMD_UNWARN)
                    )
                );

                // To listen events that fired as a IVsSolutionEvents
                spSolution = (IVsSolution)ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution));
                spSolution.AdviseSolutionEvents(this, out _pdwCookieSolution);

                initAppEvents();
            }
            catch(Exception ex)
            {
                string msg = string.Format("{0}\n{1}\n\n-----\n{2}",
                                "Something went wrong -_-",
                                "Try to restart IDE or reinstall current plugin in Extension Manager.", 
                                ex.ToString());

                Debug.WriteLine(msg);
                
                int res;
                Guid id = Guid.Empty;
                IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));

                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(
                    uiShell.ShowMessageBox(
                           0,
                           ref id,
                           "Initialize vsCommandEvent",
                           msg,
                           string.Empty,
                           0,
                           OLEMSGBUTTON.OLEMSGBUTTON_OK,
                           OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                           OLEMSGICON.OLEMSGICON_WARNING,
                           0,
                           out res));
            }
        }

        #endregion

        #region IDisposable

        // To detect redundant calls
        private bool disposed = false;

        // To correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;
            //...

            try
            {
                if(configFrm != null && !configFrm.IsDisposed) {
                    configFrm.Close();
                }

                if(errorList != null) {
                    ((IDisposable)errorList).Dispose();
                }

                Log._.paneDetach((IVsOutputWindow)GetGlobalService(typeof(SVsOutputWindow)));
            }
            catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            if(spSolution != null && _pdwCookieSolution != 0) {
                spSolution.UnadviseSolutionEvents(_pdwCookieSolution);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
