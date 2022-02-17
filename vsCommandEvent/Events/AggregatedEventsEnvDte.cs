/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Collections.Generic;
using net.r_eg.SobaScript;

namespace net.r_eg.vsCE.Events
{
    public sealed class AggregatedEventsEnvDte
    {
        internal const string E_DOC = "{2555243A-2A69-4335-BAD6-DDE9DFFE90F2}";
        internal const int E_DOC_ID_OPENING = 0x100;
        internal const int E_DOC_ID_CLOSING = 0x101;

        internal const string E_SLN = "{AD4AD581-801F-4399-B986-27FE2D308BDD}";
        internal const int E_SLN_OPEN           = 0x100;
        internal const int E_SLN_RENAME         = 0x101;
        internal const int E_SLN_CLOSE_BEFORE   = 0x200;
        internal const int E_SLN_CLOSE_AFTER    = 0x201;
        internal const int E_SLN_CLOSE_QUERY    = 0x202;
        internal const int E_SLN_PRJ_ADD        = 0x300;
        internal const int E_SLN_PRJ_DEL        = 0x301;
        internal const int E_SLN_PRJ_REN        = 0x302;

        internal const string E_OW = "{600FCA14-172C-42F3-AC91-1BC5F32CF896}";
        internal const int E_OW_ADD     = 0x100;
        internal const int E_OW_UPD     = 0x101;
        internal const int E_OW_CLEAR   = 0x102;

        internal const string E_WND = "{69F5F698-996B-4293-9FE7-4202564FE6E5}";
        internal const int E_WND_CREATE = 0x100;
        internal const int E_WND_CLOSE  = 0x101;

        internal const string E_DBG = "{4885535D-A7F9-46AB-A285-8E4D76F4C5B0}";
        internal const int E_DBG_RUN    = 0x100;
        internal const int E_DBG_BREAK  = 0x101;
        internal const int E_DBG_DESIGN = 0x102;

        internal static readonly Dictionary<string, string> ExtraEvents = new()
        {
            { E_DOC,    "@Document" },
            { E_OW,     "@OutputWindow" },
            { E_SLN,    "@Solution" },
            { E_WND,    "@Window" },
            { E_DBG,    "@Debugger" },
        };

        private readonly EnvDTE.CommandEvents cmdEvents;
        private readonly EnvDTE.DocumentEvents docEvents;
        private readonly EnvDTE.OutputWindowEvents owEvents;
        private readonly EnvDTE.SolutionEvents slnEvents;
        private readonly EnvDTE.WindowEvents windowEvents;
        private readonly EnvDTE.DebuggerEvents dbgEvents;

        private static volatile AggregatedEventsEnvDte instance;
        private static readonly object sync = new();

        public delegate void BeforeExecuteEventHandler(string guid, int id, object customIn, object customOut, ref bool cancel);

        public delegate void AfterExecuteEventHandler(string guid, int id, object customIn, object customOut);

        internal event BeforeExecuteEventHandler BeforeExecute = delegate (string guid, int id, object customIn, object customOut, ref bool cancel) { };

        internal event AfterExecuteEventHandler AfterExecute = delegate (string guid, int id, object customIn, object customOut) { };

        internal static bool FindExtra(string guid, out string found) => ExtraEvents.TryGetValue(guid, out found);

        internal static AggregatedEventsEnvDte GetInstance(IEnvironment env = null)
        {
            if(instance != null) return instance;
            lock(sync)
            {
                if(instance == null) instance = new(env);
                return instance;
            }
        }

        private AggregatedEventsEnvDte(IEnvironment env)
        {
            if(env?.Events == null)
            {
                Log.Info("Current context has only limited types.");
                return; //this can be for emulated DTE2 context
            }

            #region Up

            cmdEvents = env.Events.CommandEvents; // important! protects from GC
            cmdEvents.BeforeExecute += onCmdBeforeExecute;
            cmdEvents.AfterExecute += onCmdAfterExecute;

            // @Document

            docEvents = env.Events.DocumentEvents;
            docEvents.DocumentOpening += onDocumentOpening;
            docEvents.DocumentClosing += onDocumentClosing;

            // @Solution

            slnEvents = env.Events.SolutionEvents;
            slnEvents.Opened += onSlnEventsOpened;
            slnEvents.BeforeClosing += onSlnEventsBeforeClosing;
            slnEvents.QueryCloseSolution += onSlnEventsQueryCloseSolution;
            slnEvents.AfterClosing += onSlnEventsAfterClosing;
            slnEvents.Renamed += onSlnEventsRenamed;
            slnEvents.ProjectAdded += onSlnEventsProjectAdded;
            slnEvents.ProjectRemoved += onSlnEventsProjectRemoved;
            slnEvents.ProjectRenamed += onSlnEventsProjectRenamed;

            // @OutputWindow

            owEvents = env.Events.OutputWindowEvents;
            owEvents.PaneAdded += onOwpAdded;
            owEvents.PaneClearing += onOwpClearing;
            owEvents.PaneUpdated += onOwpUpdated;

            // @Window

            windowEvents = env.Events.WindowEvents;
            windowEvents.WindowClosing += onWindowClosing;
            windowEvents.WindowCreated += onWindowCreated;
            //windowEvents.WindowMoved += onWindowMoved;
            //windowEvents.WindowActivated += onWindowActivated;

            // @Debugger

            dbgEvents = env.Events.DebuggerEvents;
            dbgEvents.OnEnterBreakMode += onDbgEnterBreakMode;
            dbgEvents.OnEnterDesignMode += onDbgEnterDesignMode;
            dbgEvents.OnEnterRunMode += onDbgEnterRunMode;
            //dbgEvents.OnContextChanged += onDbContextChanged;
            //dbgEvents.OnExceptionNotHandled += onDbgExceptionNotHandled;
            //dbgEvents.OnExceptionThrown += onDbgExceptionThrown;

            #endregion
        }

        #region @Debugger

        private void onDbgEnterRunMode(EnvDTE.dbgEventReason reason) => AfterExecute(E_DBG, E_DBG_RUN, reason, null);

        private void onDbgEnterDesignMode(EnvDTE.dbgEventReason reason) => AfterExecute(E_DBG, E_DBG_DESIGN, reason, null);

        private void onDbgEnterBreakMode(EnvDTE.dbgEventReason reason, ref EnvDTE.dbgExecutionAction ExecutionAction)
            => AfterExecute(E_DBG, E_DBG_BREAK, reason, ExecutionAction); //TODO: ref state

        #endregion

        #region @Window

        private void onWindowCreated(EnvDTE.Window window) => onWindow(E_WND_CREATE, window);

        private void onWindowClosing(EnvDTE.Window window) => onWindow(E_WND_CLOSE, window);

        private void onWindow(int id, EnvDTE.Window window)
        {
            if(window == null)
            {
                AfterExecute(E_WND, id, null, null);
                return;
            }

            EnvDTE.Document doc = TryGetCOMProp(() => window.Document);
            EnvDTE.Window lnk = TryGetCOMProp(() => window.LinkedWindowFrame);

            AfterExecute
            (
                E_WND, id,
                TryGetCOMProp(() => window.Caption),
                ParamPacker.Pack
                (
                    "Kind", TryGetCOMProp(() => window.Kind),
                    "Type", TryGetCOMProp(() => window.Type),
                    "Pos", PackRect(window),
                    "WindowState", TryGetCOMProp(() => window.WindowState),
                    "HWnd", TryGetCOMProp(() => window.HWnd),
                    "Visible", TryGetCOMProp(() => window.Visible),
                    "AutoHides", TryGetCOMProp(() => window.AutoHides),
                    "IsFloating", TryGetCOMProp(() => window.IsFloating),
                    "Linkable", TryGetCOMProp(() => window.Linkable),

                    "LinkedWindowFrame.Caption", TryGetCOMProp(() => lnk?.Caption),
                    "LinkedWindowFrame.Kind", TryGetCOMProp(() => lnk?.Kind),
                    "LinkedWindowFrame.Type", TryGetCOMProp(() => lnk?.Type),
                    "LinkedWindowFrame.HWnd", TryGetCOMProp(() => lnk?.HWnd),
                    "LinkedWindowFrame.Pos", PackRect(lnk),

                    "Document.Name", TryGetCOMProp(() => doc?.Name),
                    "Document.Kind", TryGetCOMProp(() => doc?.Kind),
                    "Document.Type", TryGetCOMProp(() => doc?.Type),
                    "Document.Language", TryGetCOMProp(() => doc?.Language),
                    "Document.ReadOnly", TryGetCOMProp(() => doc?.ReadOnly),
                    "Document.Saved", TryGetCOMProp(() => doc?.Saved),
                    "Document.IndentSize", TryGetCOMProp(() => doc?.IndentSize),
                    "Document.TabSize", TryGetCOMProp(() => doc?.TabSize),
                    "Document.Path", TryGetCOMProp(() => doc?.Path),
                    "Document.FullName", TryGetCOMProp(() => doc?.FullName)
                )
            );
        }

        #endregion

        #region @Solution

        private void onSlnEventsRenamed(string oldName)
            => AfterExecute(E_SLN, E_SLN_RENAME, oldName, null);

        private void onSlnEventsQueryCloseSolution(ref bool fCancel)
            => BeforeExecute(E_SLN, E_SLN_CLOSE_QUERY, null, null, ref fCancel);

        private void onSlnEventsProjectRenamed(EnvDTE.Project project, string oldName)
            => onSlnEventsProject(E_SLN_PRJ_REN, project, oldName);

        private void onSlnEventsProjectRemoved(EnvDTE.Project project) => onSlnEventsProject(E_SLN_PRJ_DEL, project);

        private void onSlnEventsProjectAdded(EnvDTE.Project project) => onSlnEventsProject(E_SLN_PRJ_ADD, project);

        private void onSlnEventsProject(int id, EnvDTE.Project project, string oldName = null)
        {
            if(project == null)
            {
                AfterExecute(E_SLN, id, null, null);
                return;
            }

            EnvDTE.Configuration cfg = TryGetCOMProp(() => project.ConfigurationManager?.ActiveConfiguration);
            EnvDTE.CodeModel cm = TryGetCOMProp(() => project.CodeModel);

            AfterExecute
            (
                E_SLN, id,
                TryGetCOMProp(() => project.Name),
                ParamPacker.Pack
                (
                    "OldName", oldName,
                    "IsDirty", TryGetCOMProp(() => project.IsDirty),
                    "Saved", TryGetCOMProp(() => project.Saved),
                    "Kind", TryGetCOMProp(() => project.Kind),
                    "UniqueName", TryGetCOMProp(() => project.UniqueName),
                    "FileName", TryGetCOMProp(() => project.FileName),
                    "FullName", TryGetCOMProp(() => project.FullName),

                    "ActiveConfiguration.IsBuildable", TryGetCOMProp(() => cfg?.IsBuildable),
                    "ActiveConfiguration.IsDeployable", TryGetCOMProp(() => cfg?.IsDeployable),
                    "ActiveConfiguration.IsRunable", TryGetCOMProp(() => cfg?.IsRunable),
                    "ActiveConfiguration.Name", TryGetCOMProp(() => cfg?.ConfigurationName),
                    "ActiveConfiguration.Platform", TryGetCOMProp(() => cfg?.PlatformName),

                    "CodeModel.Language", TryGetCOMProp(() => cm?.Language),
                    "CodeModel.IsCaseSensitive", TryGetCOMProp(() => cm?.IsCaseSensitive),
                    "CodeModel.CodeElements.Count", TryGetCOMProp(() => cm?.CodeElements.Count)
                )
            );
        }

        private void onSlnEventsAfterClosing() => AfterExecute(E_SLN, E_SLN_CLOSE_AFTER, null, null);

        private void onSlnEventsOpened() => AfterExecute(E_SLN, E_SLN_OPEN, null, null);

        private void onSlnEventsBeforeClosing() => AfterExecute(E_SLN, E_SLN_CLOSE_BEFORE, null, null);

        #endregion

        #region @OutputWindow

        private void onOwpUpdated(EnvDTE.OutputWindowPane pPane) => onOwp(E_OW_UPD, pPane);

        private void onOwpClearing(EnvDTE.OutputWindowPane pPane) => onOwp(E_OW_CLEAR, pPane);

        private void onOwpAdded(EnvDTE.OutputWindowPane pPane) => onOwp(E_OW_ADD, pPane);

        private void onOwp(int id, EnvDTE.OutputWindowPane pPane)
        {
            if(pPane == null)
            {
                AfterExecute(E_OW, id, null, null);
                return;
            }

            AfterExecute
            (
                E_OW, id,
                TryGetCOMProp(() => pPane.Name), 
                ParamPacker.Pack
                (
                    "Guid", TryGetCOMProp(() => pPane.Guid)
                )
            );
        }

        #endregion

        #region @Commands

        private void onCmdBeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
            => BeforeExecute(Guid, ID, CustomIn, CustomOut, ref CancelDefault);

        private void onCmdAfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
            => AfterExecute(Guid, ID, CustomIn, CustomOut);

        #endregion

        #region @Document

        private void onDocumentClosing(EnvDTE.Document doc)
        {
            if(doc == null)
            {
                AfterExecute(E_DOC, E_DOC_ID_CLOSING, null, null);
                return;
            }

            AfterExecute
            (
                E_DOC, E_DOC_ID_CLOSING, 
                TryGetCOMProp(() => doc.FullName), 
                ParamPacker.Pack
                (
                    "Language", TryGetCOMProp(() => doc.Language),
                    "Saved", TryGetCOMProp(() => doc.Saved),
                    "Type", TryGetCOMProp(() => doc.Type),
                    "ReadOnly", TryGetCOMProp(() => doc.ReadOnly),
                    "IndentSize", TryGetCOMProp(() => doc.IndentSize),
                    "TabSize", TryGetCOMProp(() => doc.TabSize),
                    "Kind", TryGetCOMProp(() => doc.Kind),
                    "Name", TryGetCOMProp(() => doc.Name),
                    "Path", TryGetCOMProp(() => doc.Path)
                )
            );
        }

        private void onDocumentOpening(string path, bool readOnly) 
            => AfterExecute(E_DOC, E_DOC_ID_OPENING, path, Value.From(readOnly));

        #endregion

        private static T TryGetCOMProp<T>(Func<T> cb)
        {
            try { return cb(); } catch { return default; } // COM, possible detaching/switching context etc.
        }

        internal static string PackRect(EnvDTE.Window w)
        {
            if(w == null) return null;
            return ParamPacker.PackRect
            (
                TryGetCOMProp(() => w.Left),
                TryGetCOMProp(() => w.Top),
                TryGetCOMProp(() => w.Width),
                TryGetCOMProp(() => w.Height)
            );
        }
    }
}
