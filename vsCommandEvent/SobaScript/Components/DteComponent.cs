/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.Components;
using net.r_eg.SobaScript;
using net.r_eg.SobaScript.Exceptions;
using net.r_eg.SobaScript.Mapper;
using net.r_eg.SobaScript.SNode;
using OriginDteComponent = net.r_eg.SobaScript.Z.VS.DteComponent;

namespace net.r_eg.vsCE.SobaScript.Components
{
    /// <summary>
    /// <see cref="OriginDteComponent"/> CE edition
    /// </summary>
    [Component("DTE", "DTE commands and events.\nRelated to assembly-wrapped COM library containing the objects and members for Visual Studio core automation.")]
    internal class DteComponent: OriginDteComponent
    {
        private const string M_RAISE = "raise";

        private readonly IDteCeEnv envce;

        public override string Eval(string data)
        {
            var point = EntryPoint(data);
            string subtype = point.Key;
            string request = point.Value;

            LSender.Send(this, $"`{ToString()}`: `{subtype}` -> `{request}`", MsgLevel.Trace);

            return subtype switch
            {
                M_RAISE => StRaise(new PM(request)),
                _ => base.Eval(data),
            };
        }

        public DteComponent(ISobaScript soba, IDteCeEnv env)
            : base(soba, env)
        {
            envce = env;
        }

        /// <summary>
        /// Raise Command.
        /// e.g: #[DTE raise(guid, id, customIn, customOut)]
        /// </summary>
        [Method
        (
            M_RAISE,
            "Raise Command ID for EnvDTE.",
            new[] { "guid", "id", "customIn", "customOut" },
            new[] { "Scope by Guid", "The command ID", "Mixed input value.", "Mixed output value." },
            CValType.Void,
            CValType.String, CValType.Integer, CValType.Mixed, CValType.Mixed
        )]
        protected string StRaise(IPM pm)
        {
            if(!pm.FinalEmptyIs(0, LevelType.Method, M_RAISE)) {
                throw new IncorrectNodeException(pm);
            }

            RArgs args = pm.FirstLevel.Args;

            if(args?.Length != 4 
                || args[0].type != ArgumentType.StringDouble
                || args[1].type != ArgumentType.Integer)
            {
                throw new PMArgException(args, "(string, integer, mixed, mixed)");
            }

            string guid         = (string)args[0].data;
            int id              = (int)args[1].data;
            object customIn     = Extract(args[2]);
            object customOut    = Extract(args[3]);

            LSender.Send(this, $"{M_RAISE}('{guid}', '{id}', '{customIn}', '{customOut}')");
            Raise(guid, id, ref customIn, ref customOut);
            return Value.Empty;
        }

        protected virtual void Raise(string guid, int id, ref object customIn, ref object customOut)
        {
            envce.Raise(guid, id, ref customIn, ref customOut);
        }

        private static object Extract(Argument arg)
        {
            if(arg.type == ArgumentType.Object)
            {
                return Value.Extract((RArgs)arg.data);
            }
            return arg.data;
        }
    }
}
