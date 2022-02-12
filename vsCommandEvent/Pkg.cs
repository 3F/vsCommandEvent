/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using net.r_eg.vsCE.Configuration;

#if VSSDK_15_AND_NEW
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;
#endif

namespace net.r_eg.vsCE
{
#if VSSDK_15_AND_NEW
    // Managed Package Registration
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]

    // To be automatically loaded when a specified UI context is active
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
#else
    // Managed Package Registration
    [PackageRegistration(UseManagedResourcesOnly = true)]

    // To be automatically loaded when a specified UI context is active
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
#endif

    // Information for Visual Studio Help/About dialog.
    [InstalledProductRegistration("#110", "#112", Version.S_NUM, IconResourceID = 400)]

    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]

    // Package Guid
    [Guid(GuidList.PACKAGE_STRING)]
    public sealed class Pkg:

#if VSSDK_15_AND_NEW
         AsyncPackage,
#else
         Package,
#endif

        IVsSolutionEvents, IPkg, IDisposable
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
        /// The command for: Tools / { main app tool }
        /// </summary>
        private MainToolCommand mainToolCmd;

        /// <summary>
        /// Listener of the OutputWindowsPane
        /// </summary>
        private Receiver.Output.OWP owpListener;

        private readonly object sync = new object();

        /// <summary>
        /// Reserved for future use with IVsSolutionEvents
        /// </summary>
        private readonly object pUnkReserved = new object();

        /// <summary>
        /// DTE2 Context
        /// </summary>
        public DTE2 Dte2 => (DTE2)Package.GetGlobalService(typeof(SDTE));

        public IEvLevel PackageBinder
        {
            get;
            private set;
        }

        /// <summary>
        /// For work with ErrorList pane of Visual Studio.
        /// </summary>
        public VSTools.ErrorList.IPane ErrorList
        {
            get;
            private set;
        }

        public CancellationToken CancellationToken
        {
            get
            {
#if VSSDK_15_AND_NEW
                return DisposalToken;
#else
                return CancellationToken.None;
#endif
            }
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
            Monitor.Enter(sync);

            try {
                return PackageBinder.solutionOpened(pUnkReserved, fNewSolution);
            }
            catch(Exception ex) {
                Log.Fatal("Problem when loading solution: " + ex.Message);
            }
            finally {
                Monitor.Exit(sync);
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
            Monitor.Enter(sync);

            try {
                return PackageBinder.solutionClosed(pUnkReserved);
            }
            catch(Exception ex) {
                Log.Fatal("Problem when closing solution: " + ex.Message);
            }
            finally {
                Monitor.Exit(sync);
            }

            return VSConstants.S_FALSE;
        }

#if VSSDK_15_AND_NEW

        /// <summary>
        /// Finds or creates tool window.
        /// </summary>
        /// <param name="type">tool window type</param>
        /// <param name="create">try to create tool when true</param>
        /// <param name="id">tool window id</param>
        /// <returns></returns>
        public async Task<ToolWindowPane> getToolWindowAsync(Type type, bool create = true, int id = 0)
        {
            return await FindToolWindowAsync
            (
                type, id, create, DisposalToken
            );
        }

        /// <summary>
        /// AsyncPackage.GetServiceAsync
        /// </summary>
        /// <param name="type">service type.</param>
        /// <returns></returns>
        public async Task<object> getSvcAsync(Type type)
        {
            return await GetServiceAsync(type);
        }

#else

        /// <summary>
        /// Finds or creates tool window.
        /// </summary>
        /// <param name="type">tool window type</param>
        /// <param name="create">try to create tool when true</param>
        /// <param name="id">tool window id</param>
        /// <returns></returns>
        public ToolWindowPane getToolWindow(Type type, bool create = true, int id = 0)
        {
            return FindToolWindow(type, id, create);
        }

        /// <summary>
        /// Package.GetService
        /// </summary>
        /// <param name="type">service type.</param>
        /// <returns></returns>
        public object getSvc(Type type) => GetService(type);

#endif

        public Pkg()
        {
            Trace.WriteLine($"Plugin is activated: { ToString() }");
        }

#if VSSDK_15_AND_NEW

        /// <summary>
        /// Modern 15+ Initialization of the package; this method is called right after the package is sited.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Trace.WriteLine($"Entering InitializeAsync() of: { ToString() }");

            //await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            try
            {
                initAppEvents(cancellationToken);

                mainToolCmd = await MainToolCommand.InitAsync(this, PackageBinder);
                
                // VS bug: https://github.com/microsoft/extendvs/issues/68
                // MSVS Shell.15.0 15.7.27703
                // MSVS Threading 15.8.209
                //_ = Task.Run(async () =>
                //{
                    // // this line fixes related bug in new MSVS Shell.15.0 15.9.28307
                    // // when tool is already attached when starting VS.
                    // // do not use true value in non-UI thread ........................v
                    // var tool = await getToolWindowAsync(StatusToolCommand.ToolType, false);

                    // await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

                    // sToolCmd = await StatusToolCommand.InitAsync(this, Event, tool);

                    // https://github.com/3F/vsSolutionBuildEvent/pull/45#discussion_r291835939
                    if(Dte2.Solution.IsOpen) OnAfterOpenSolution(pUnkReserved, 0);
                //});

                spSolution = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
                spSolution?.AdviseSolutionEvents(this, out _pdwCookieSolution);
            }
            catch(Exception ex)
            {
                IVsUIShell uiShell = await GetServiceAsync(typeof(SVsUIShell)) as IVsUIShell;
                _showCriticalVsMsg(uiShell, ex);
            }
        }

#else

        /// <summary>
        /// Old VS10" - VS15" Synchronous Initialization of the package;
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine($"Entering Initialize() of: { ToString() }");
            base.Initialize();

            try
            {
                initAppEvents(CancellationToken.None);

                mainToolCmd = MainToolCommand.Init(this, PackageBinder);

                // https://github.com/3F/vsSolutionBuildEvent/pull/45#discussion_r291835939
                if(Dte2.Solution.IsOpen) OnAfterOpenSolution(pUnkReserved, 0);

                spSolution = GetService(typeof(SVsSolution)) as IVsSolution;
                spSolution.AdviseSolutionEvents(this, out _pdwCookieSolution);
            }
            catch(Exception ex)
            {
                IVsUIShell uiShell = GetService(typeof(SVsUIShell)) as IVsUIShell;
                _showCriticalVsMsg(uiShell, ex);
            }
        }

#endif

        private void initAppEvents(CancellationToken cancellationToken)
        {
            ErrorList = new VSTools.ErrorList.Pane(this, cancellationToken);

            Log._.Received  -= onLogReceived;
            Log._.Received  += onLogReceived;

            new Logger.Initializer().configure();

            if(!attachMainPane()) {
                ((Events2)Dte2.Events).get_WindowVisibilityEvents().WindowShowing -= attachToOutputWindow;
                ((Events2)Dte2.Events).get_WindowVisibilityEvents().WindowShowing += attachToOutputWindow;
            }

            loadData();
            PackageBinder = new EvLevel(Dte2);

            owpListener = new Receiver.Output.OWP(PackageBinder.Environment);
            owpListener.attachEvents();
            owpListener.Receiving += (object sender, Receiver.Output.PaneArgs e) => {
                ((Bridge.IEvent)PackageBinder).onBuildRaw(e.Raw, e.Guid, e.Item);
            };
        }

        private void _showCriticalVsMsg(IVsUIShell uiShell, Exception ex)
        {
#if VSSDK_15_AND_NEW
            ThreadHelper.ThrowIfNotOnUIThread();
#endif
            string msg = String.Format
            (
                "{0}\n{1}\n\n-----\n{2}", 
                "Something went wrong -_-",
                "Try to restart IDE / reinstall plugin / or please contact with us!", 
                ex.ToString()
            );

            Debug.WriteLine(msg);

            Guid id = Guid.Empty;
            ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox
            (
                0,
                ref id,
                $"Initialize { ToString() }",
                msg,
                String.Empty,
                0,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                OLEMSGICON.OLEMSGICON_WARNING,
                0,
                out int res
            ));
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
        /// Safe for attaching pane in NoSolution context.
        /// </summary>
        /// <param name="wnd"></param>
        private void attachToOutputWindow(Window wnd)
        {
            if(wnd == null) {
                Log.Warn("attachToOutputWindow: wnd is null");
                return;
            }

            if(wnd.ObjectKind == EnvDTE.Constants.vsWindowKindOutput || wnd.Type == vsWindowType.vsWindowTypeOutput) {
                attachMainPane();
                ((Events2)Dte2.Events).get_WindowVisibilityEvents().WindowShowing -= attachToOutputWindow;
            }
        }

        private bool attachMainPane()
        {
            try
            {
                Log._.paneAttach(GetOutputPane(GuidList.OWP_SBE, Settings.OWP_ITEM_VSSBE));
                Log.Trace($"Pane has been attached: '{GuidList.OWP_SBE}'");
                return true;
            }
            catch(Exception ex) {
                Log.Error("Failed attachToOutputWindow: `{0}`", ex.Message);
            }
            return false;
        }

        private void onLogReceived(object sender, Logger.MessageArgs e)
        {
            if(Log._.isError(e.Level)) {
                ErrorList.error(e.Message);
            }
            else if(Log._.isWarn(e.Level)) {
                ErrorList.warn(e.Message);
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

        #region IDisposable

        private bool disposed = false;

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

            try
            {
                mainToolCmd?.Dispose();

                if(ErrorList != null) {
                    ((IDisposable)ErrorList).Dispose();
                }

#if VSSDK_15_AND_NEW
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(DisposalToken);
#endif

                if(spSolution != null && _pdwCookieSolution != 0) {
                    spSolution.UnadviseSolutionEvents(_pdwCookieSolution);
                }

                Log._.paneDetach((IVsOutputWindow)GetGlobalService(typeof(SVsOutputWindow)));

#if VSSDK_15_AND_NEW
            });
#endif

            }
            catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
