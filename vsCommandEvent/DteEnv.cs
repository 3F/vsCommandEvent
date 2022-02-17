/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using net.r_eg.SobaScript.Z.VS.Dte;
using net.r_eg.vsCE.Actions;
using net.r_eg.vsCE.SobaScript.Components;

namespace net.r_eg.vsCE
{
    internal sealed class DteEnv: IDteCeEnv
    {
        private readonly IEnvironment env;

        private readonly Lazy<DTEOperation> dteo;

        private readonly _DteCommand _dtec = new();

        private readonly object sync = new();

        /// <summary>
        /// Ability of work with DTE Commands.
        /// </summary>
        public bool IsAvaialbleDteCmd => env.Events != null;

        /// <summary>
        /// The last received command from EnvDTE.
        /// </summary>
        public IDteCommand LastCmd => _dtec;

        /// <summary>
        /// Execute command through EnvDTE.
        /// </summary>
        /// <param name="cmd"></param>
        public void Execute(string cmd)
            => dteo.Value.exec(new string[] { cmd }, false);

        public void Raise(string guid, int id, object customIn, object customOut)
            => env.raise(guid, id, customIn, customOut);

        public void Dispose() => DetachCommandEvents();

        public DteEnv(IEnvironment env)
        {
            this.env = env ?? throw new ArgumentNullException(nameof(env));

            dteo = new Lazy<DTEOperation>(() => 
                new DTEOperation(env, Events.SolutionEventType.General)
            );

            AttachCommandEvents();
        }

        private void AttachCommandEvents()
        {
            if(!IsAvaialbleDteCmd)
            {
                Log.Info($"{ToString()} aren't available for current context.");
                return;
            }

            lock(sync)
            {
                DetachCommandEvents();
                env.AggregatedEvents.BeforeExecute += OnCommandEventBefore;
                env.AggregatedEvents.AfterExecute  += OnCommandEventAfter;
            }
        }

        private void DetachCommandEvents()
        {
            if(env.AggregatedEvents == null) return;

            env.AggregatedEvents.BeforeExecute -= OnCommandEventBefore;
            env.AggregatedEvents.AfterExecute  -= OnCommandEventAfter;
        }

        private void OnCommandEventBefore(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
            => CommandEvent(true, guid, id, customIn, customOut);

        private void OnCommandEventAfter(string guid, int id, object customIn, object customOut)
            => CommandEvent(false, guid, id, customIn, customOut);

        private void CommandEvent(bool pre, string guid, int id, object customIn, object customOut)
        {
            _dtec.Guid      = guid;
            _dtec.Id        = id;
            _dtec.CustomIn  = customIn;
            _dtec.CustomOut = customOut;
            _dtec.Pre       = pre;
        }

        private sealed class _DteCommand: IDteCommand
        {
            public string Guid { get; set; }

            public int Id { get; set; }

            public object CustomIn { get; set; }

            public object CustomOut { get; set; }

            public bool Cancel { get; set; }

            public bool Pre { get; set; }
        }
    }
}
