/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using net.r_eg.SobaScript;
using net.r_eg.SobaScript.Z.Core;
using net.r_eg.SobaScript.Z.Ext;
using net.r_eg.SobaScript.Z.Ext.IO;
using net.r_eg.SobaScript.Z.VS;
using net.r_eg.Varhead;
using net.r_eg.vsCE.SobaScript.Components;
using DteCeComponent = net.r_eg.vsCE.SobaScript.Components.DteComponent;

namespace net.r_eg.vsCE
{
    //TODO: new arch for SobaScript + E-MSBuild
    internal sealed class Bootloader
    {
        public static Bootloader _
        {
            get;
            private set;
        }

        public IEnvironment Env => EvLevel.Environment;

        public IEvLevel EvLevel
        {
            get;
            private set;
        }

        public IUVars UVars
        {
            get;
            private set;
        }

        public ISobaScript Soba
        {
            get;
            private set;
        }

        public static Bootloader Init(IEvLevel evl)
        {
            if(_ == null) {
                _ = new Bootloader(evl);
            }
            return _;
        }

        public static ISobaScript Configure(ISobaScript soba, IEnvironment env)
        {
            if(soba == null) {
                throw new ArgumentNullException(nameof(soba));
            }

            IEncDetector detector = new EncDetector();

            var fc = new FileComponent(soba, detector, new Exer(Settings.WPath, detector));
            var zc = new SevenZipComponent(soba, new SzArchiver(), Settings.WPath);
            var nc = new NuGetComponent(soba, Settings.WPath);

            Settings._.WorkPathUpdated += (object sender, DataArgs<string> e) =>
            {
                fc.Exer.BasePath = e.Data;
                zc.BasePath = e.Data;
                nc.BasePath = e.Data;
            };

            //NOTE: custom order makes sense for vsSBE

            soba.Register(new TryComponent(soba));
            soba.Register(new CommentComponent());
            soba.Register(new BoxComponent(soba));
            soba.Register(new ConditionComponent(soba));
            soba.Register(new UserVariableComponent(soba));
            soba.Register(new OwpComponent(soba, new OwpEnv(env)));
            soba.Register(new DteCeComponent(soba, new DteEnv(env)));
            soba.Register(new InternalComponent(soba, env, fc.Exer));
            soba.Register(new EvMSBuildComponent(soba));
            soba.Register(new BuildComponent(soba, new BuildEnv(env)));
            soba.Register(new FunctionComponent(soba));
            soba.Register(fc);
            soba.Register(nc);
            soba.Register(zc);

            return soba;
        }

        public static ISobaScript Reset(ISobaScript soba, bool unsetUVars)
        {
            if(soba == null) {
                throw new ArgumentNullException(nameof(soba));
            }

            soba.Unregister();

            if(unsetUVars) {
                soba.UVars.UnsetAll();
            }

            return soba;
        }

        public ISobaScript Configure(ISobaScript soba)
            => Configure(soba, Env);

        private Bootloader(IEvLevel evl)
        {
            EvLevel = evl ?? throw new ArgumentNullException(nameof(evl));

            UVars = new UVars();

            Soba = Configure(
                new Soba(MSBuild.MakeEvaluator(evl.Environment, UVars), UVars)
            );
        }
    }
}
