/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System.Collections.Generic;
using net.r_eg.SobaScript;

namespace net.r_eg.vsCE.Events
{
    public sealed class AggregatedEventsEnvDte
    {
        internal const string E_DOC = "{2555243A-2A69-4335-BAD6-DDE9DFFE90F2}";
        internal const int E_DOC_ID_OPENING = 0x100;
        internal const int E_DOC_ID_CLOSING = 0x101;

        internal static readonly Dictionary<string, string> ExtraEvents = new()
        {
            { E_DOC, "@DocumentEvents" }
        };

        private readonly EnvDTE.CommandEvents cmdEvents;
        private readonly EnvDTE.DocumentEvents docEvents;

        public delegate void BeforeExecuteEventHandler(string guid, int id, object customIn, object customOut, ref bool cancel);

        public delegate void AfterExecuteEventHandler(string guid, int id, object customIn, object customOut);

        public event BeforeExecuteEventHandler BeforeExecute = delegate (string guid, int id, object customIn, object customOut, ref bool cancel) { };

        public event AfterExecuteEventHandler AfterExecute = delegate (string guid, int id, object customIn, object customOut) { };

        public AggregatedEventsEnvDte(IEnvironment env)
        {
            if(env?.Events == null)
            {
                Log.Info("Current context has only limited types.");
                return; //this can be for emulated DTE2 context
            }

            cmdEvents = env.Events.CommandEvents; // important! protects from GC
            cmdEvents.BeforeExecute += onCmdBeforeExecute;
            cmdEvents.AfterExecute += onCmdAfterExecute;

            docEvents = env.Events.DocumentEvents;
            docEvents.DocumentOpening += onDocumentOpening;
            docEvents.DocumentClosing += onDocumentClosing;
        }

        private void onCmdBeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
            => BeforeExecute(Guid, ID, CustomIn, CustomOut, ref CancelDefault);

        private void onCmdAfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
            => AfterExecute(Guid, ID, CustomIn, CustomOut);

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
                Value.From(doc.FullName), 
                ParamPacker.Pack
                (
                    "Language", doc.Language,
                    "Saved", doc.Saved,
                    "Kind", doc.Kind,
                    "Type", doc.Type,
                    "ReadOnly", doc.ReadOnly,
                    "IndentSize", doc.IndentSize,
                    "TabSize", doc.TabSize,
                    "Name", doc.Name,
                    "Path", doc.Path
                )
            );
        }

        private void onDocumentOpening(string path, bool readOnly)
        {
            object input  = path;
            object output = Value.From(readOnly);
            AfterExecute(E_DOC, E_DOC_ID_OPENING, input, output);
        }
    }
}
