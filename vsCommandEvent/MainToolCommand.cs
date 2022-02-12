/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

#if VSSDK_15_AND_NEW
using System.Threading.Tasks;
#endif

namespace net.r_eg.vsCE
{
    /// <summary>
    /// Build / { Main App }
    /// </summary>
    internal sealed class MainToolCommand: IDisposable
    {
        private readonly MenuCommand mcmd;

        private readonly IEvLevel apievt;

        private readonly IPkg pkg;

        private UI.WForms.EventsFrm configFrm;

        public static MainToolCommand Instance
        {
            get;
            private set;
        }

        public void closeConfigForm()
        {
            UI.Util.closeTool(configFrm);
        }

#if VSSDK_15_AND_NEW

        public static async Task<MainToolCommand> InitAsync(IPkg pkg, IEvLevel evt)
        {
            if(Instance != null) {
                return Instance;
            }

            // Switch to the main thread - the call to AddCommand in MainToolCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(pkg.CancellationToken);

            Instance = new MainToolCommand
            (
                pkg,
                await pkg.getSvcAsync(typeof(IMenuCommandService)) as OleMenuCommandService,
                evt
            );

            return Instance;
        }

#else

        public static MainToolCommand Init(IPkg pkg, IEvLevel evt)
        {
            if(Instance != null) {
                return Instance;
            }

            Instance = new MainToolCommand
            (
                pkg,
                pkg.getSvc(typeof(IMenuCommandService)) as OleMenuCommandService, 
                evt
            );

            return Instance;
        }

#endif

        /// <param name="pkg"></param>
        /// <param name="svc">Command service to add command to, not null.</param>
        /// <param name="evt">Supported public events, not null.</param>
        private MainToolCommand(IPkg pkg, OleMenuCommandService svc, IEvLevel evt)
        {
            this.pkg    = pkg ?? throw new ArgumentNullException(nameof(pkg));
            svc         = svc ?? throw new ArgumentNullException(nameof(svc));
            apievt      = evt ?? throw new ArgumentNullException(nameof(evt));

            MenuCommand _GetMenu(int cmd, EventHandler act) => new MenuCommand
            (
                act, new CommandID(GuidList.CMD_MAIN, cmd)
            )
            { Visible = true, Supported = true, Enabled = true };

            mcmd = _GetMenu(PkgCmdIDList.CMD_MAIN, onAction);
            svc.AddCommand(mcmd);

            svc.AddCommand
            (
                _GetMenu(PkgCmdIDList.CMD_UNWARN, (object sender, EventArgs e) => pkg.ErrorList?.clear())
            );
        }

        private void onAction(object sender, EventArgs e)
        {
            try
            {
                if(UI.Util.focusForm(configFrm)) {
                    return;
                }
                configFrm = new UI.WForms.EventsFrm(Bootloader._);
                configFrm.Show();
            }
            catch(Exception ex) {
                Log.Error("Failed UI: `{0}`", ex.Message);
            }
        }

        private void free()
        {
            if(configFrm != null && !configFrm.IsDisposed) {
                configFrm.Close();
            }
        }

#region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            free();
        }

#endregion
    }
}
