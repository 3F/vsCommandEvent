/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using net.r_eg.EvMSBuild;
using net.r_eg.Varhead;

namespace net.r_eg.vsCE
{
    internal static class MSBuild
    {
        internal static IEvMSBuild MakeEvaluator(IEnvironment env, IUVars uvars)
            => PostAction(env, new EvMSBuilder(uvars));

        internal static IEvMSBuild MakeEvaluator(IEnvironment env)
            => PostAction(env, new EvMSBuilder());

        private static IEvMSBuild PostAction(IEnvironment env, IEvMSBuild instance)
        {
            //instance.GlobalPropertyChanged += (object sender, PropertyArgs e)
            //    => env?.CoreCmdSender?.fire(GetRawCommand(new[] 
            //    {
            //        e.Removed ? "property.del" : "property.set",
            //        e.name,
            //        e.value
            //    }));

            return instance;
        }

        //private static CoreCommandArgs GetRawCommand(object[] cmd)
        //    => new CoreCommandArgs() { Type = CoreCommandType.RawCommand, Args = cmd };
    }
}
