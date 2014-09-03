﻿/*
 * Copyright (c) 2013 Developed by reg <entry.reg@gmail.com>
 * Distributed under the Boost Software License, Version 1.0
 * (See accompanying file LICENSE or copy at http://www.boost.org/LICENSE_1_0.txt)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.r_eg.vsSBE.Events;

namespace net.r_eg.vsSBE.UI
{
    /// <summary>
    /// Predefined operations for gui
    /// TODO: move to SBE ~
    /// </summary>
    internal class DefCommandsDTE
    {
        public static List<TOperation> operations()
        {
            List<TOperation> dte = new List<TOperation>();

            dte.Add(new TOperationQ(new string[]{"Build.Cancel"}, "Stop building"));
            dte.Add(new TOperationQ(new string[]{"Build.Cancel", "Build.RebuildSolution"}, "Rebuild Solution"));
            dte.Add(new TOperationQ(new string[]{"Debug.Start"}, "Run project"));
            dte.Add(new TOperationQ(new string[]{"Debug.StartWithoutDebugging"}, "Run Without Debugging"));

            return dte;
        }

        /// <summary>
        /// TOperation cannot contain a constructor to serialize
        /// .. benefits the principle LSP =_=
        /// </summary>
        private class TOperationQ: TOperation
        {
            public TOperationQ(string[] cmd, string caption)
            {
                this.cmd     = cmd;
                this.caption = caption;
            }
        }
    }
}
